using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class OilParkModel
    {
        static public string[] Qualitys = new[] { "ОЧ", "Плотность" };

        GraphNode[] graphNodes;
        List<int> Tanks = new List<int>();
        List<int> Pipes = new List<int>();
        Dictionary<GraphNode, float[]> FirstNodes = new Dictionary<GraphNode, float[]>();
        Dictionary<GraphNode, Dictionary<string, float[]>> SourceQuality = new Dictionary<GraphNode, Dictionary<string, float[]>>();
        Dictionary<int, int> sharesIndex = new Dictionary<int, int>();
        List<int> Receivers = new List<int>();
        List<GraphNode> VariableFlowNodes = new List<GraphNode>();

        public OilParkModel((List<int> NextObjList, FactoryObjectParam Params, float[] FlowValues, Dictionary<string, float> QualityValues)[] linksMap, int times)
        {
            Times = times;

            graphNodes = new GraphNode[linksMap.Length];
            int shareIndex = 0;
            for (int i = 0; i < linksMap.Length; i++)
            {
                GraphNode node = null;
                if (linksMap[i].Params.Type == FactoryObjectType.Tank)
                {
                    node = new Tank(i, linksMap[i].Params, Times);
                    Tanks.Add(i);
                    VariableFlowNodes.Add(node);
                }
                else if (linksMap[i].Params.Type == FactoryObjectType.Pipe)
                {
                    node = new Pipe(i, linksMap[i].Params, Times);
                    Pipes.Add(i);
                    if (linksMap[i].NextObjList.Count > 1)
                    {
                        VariableFlowNodes.Add(node);
                    }
                }
                graphNodes[i] = node;
                if (linksMap[i].FlowValues != null)
                {
                    FirstNodes[graphNodes[i]] = linksMap[i].FlowValues;
                    SourceQuality[graphNodes[i]] = new Dictionary<string, float[]>();
                    foreach (var qualityName in OilParkModel.Qualitys)
                    {
                        float qualityValue = linksMap[i].QualityValues[qualityName];
                        SourceQuality[graphNodes[i]][qualityName] = Enumerable.Repeat(qualityValue, linksMap[i].Params.qualityTimeSplits).ToArray();
                    }
                    sharesIndex[CombineHashCode(i, i)] = shareIndex++;
                }
            }

            for (int i = 0; i < linksMap.Length; i++)
            {
                foreach (var nodeId in linksMap[i].NextObjList)
                {
                    graphNodes[i].AddNextNode(graphNodes[nodeId]);
                    graphNodes[nodeId].PreNodesCountInc();
                    if (linksMap[i].NextObjList.Count > 1 || (graphNodes[i].ObjectInfo.Type == FactoryObjectType.Tank && linksMap[i].NextObjList.Count == 1))
                    {
                        sharesIndex[CombineHashCode(i, nodeId)] = shareIndex++;
                    }
                }

                if (linksMap[i].NextObjList.Count == 0)
                {
                    Receivers.Add(i);
                }
            }
        }

        public void BFS(int time, float[] sharesOnTimeInterval)
        {
            Stack<GraphNode> queue = new Stack<GraphNode>();
            foreach (var node in FirstNodes.Keys)
            {
                int hash = CombineHashCode(node.Id, node.Id);
                int shareIdx = sharesIndex[hash];
                float share = sharesOnTimeInterval[shareIdx];
                float flow = FirstNodes[node][time] * share;
                node.AddIncome(time, flow, SourceQuality[node]);
                node.SetOutcome(time, flow);
                queue.Push(node);
            }

            while (queue.Count > 0)
            {
                var node = queue.Pop();
                int countChildren = node.NextNodesCount;
                int shareIdxHash = CombineHashCode(node.Id, node.Id);
                int shareIdx = sharesIndex.ContainsKey(shareIdxHash) ? sharesIndex[shareIdxHash] : -1;
                float share = shareIdx >= 0 ? sharesOnTimeInterval[shareIdx] : (node.NextNodesCount > 0) ? 1 : 0;
                float outcome = share * node.Income(time);
                node.SetOutcome(time, outcome);

                if (countChildren > 1)
                {
                    float a = 0;
                    for (int i = 0; i < countChildren; i++)
                    {
                        a += CombineHashCode(node.Id, node.GetNextNode(i).Id);
                    }

                    a = 1 / a;

                    for (int i = 0; i < countChildren; i++)
                    {
                        var child = node.GetNextNode(i);
                        int childShareIdxHash = CombineHashCode(node.Id, child.Id);
                        int childShareIdx = sharesIndex.ContainsKey(childShareIdxHash) ? sharesIndex[childShareIdxHash] : -1;
                        float childShare = childShareIdx >= 0 ? sharesOnTimeInterval[childShareIdx] : 1;
                        float childIncome = childShare * a * outcome;
                        Dictionary<string, float[]> quality = new Dictionary<string, float[]>();
                        foreach (var qualityName in OilParkModel.Qualitys)
                        {
                            quality[qualityName] = new float[child.ObjectInfo.qualityTimeSplits];
                            node.Quality[qualityName][time + 1].CopyTo(quality[qualityName], 0);
                        }

                        child.AddIncome(time, childIncome, quality);
                        if (child.NodeActive(time))
                        {
                            queue.Push(child);
                        }
                    }
                }
                else if (countChildren == 1)
                {
                    var child = node.GetNextNode(0);
                    Dictionary<string, float[]> quality = new Dictionary<string, float[]>();
                    foreach (var qualityName in OilParkModel.Qualitys)
                    {
                        quality[qualityName] = new float[child.ObjectInfo.qualityTimeSplits];
                        node.Quality[qualityName][time + 1].CopyTo(quality[qualityName], 0);
                    }
                    child.AddIncome(time, outcome, quality);
                    if (child.NodeActive(time))
                    {
                        queue.Push(child);
                    }
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < graphNodes.Length; i++)
            {
                graphNodes[i].Clear();
            }
        }

        public void UpdateNode()
        {
            for (int i = 0; i < graphNodes.Length; i++)
            {
                graphNodes[i].UpdateState();
            }
        }

        private int CombineHashCode(int firstHash, int secondHash)
        {
            // https://stackoverflow.com/questions/1646807/quick-and-simple-hash-code-combinations
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + firstHash;
                hash = hash * 31 + secondHash;
                return hash;
            }
        }

        public void StartModel(float[] sharesOnAllTime)
        {
            for (int t = 0; t < Times; t++)
            {
                float[] currentShareArray = new float[sharesIndex.Count];
                Array.Copy(sharesOnAllTime, sharesIndex.Count * t, currentShareArray, 0, sharesIndex.Count);
                BFS(t, currentShareArray);
                UpdateNode();
            }
        }

        public float GetValue()
        {
            float result = 0;
            foreach (var i in Receivers)
            {
                result += graphNodes[i].GetMassOnEndTime(Times);
            }
            return result;
        }

        public int Times { get; }
    }
}