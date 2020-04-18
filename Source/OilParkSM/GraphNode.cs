using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Model
{
    internal class GraphNode
    {
        public GraphNode(int id, FactoryObjectParam param, int times)
        {
            Id = id;
            ObjectInfo = param;
            _nextNodes = new List<GraphNode>();
            _incomes = new List<float>[times];
            _outcome = new float[times];
            _mass = new float[(times + 1)];
            _mass[0] = param.StartMass;
            for (int i=0; i < times; i++)
            {
                _incomes[i] = new List<float>();
            }
            _times = times;
            Quality = new Dictionary<string, float[][]>();
            foreach(var qualityName in OilParkModel.Qualitys)
            {               
                Quality[qualityName] = new float[times + 1][];
                var qualityValue = param.QualityStart != null ? param.QualityStart[qualityName] : 0;
                Quality[qualityName][0] = Enumerable.Repeat(qualityValue, param.qualityTimeSplits).ToArray(); 
                for(int i = 1; i < times+1; i++)
                {
                    Quality[qualityName][i] = new float[param.qualityTimeSplits];
                }
            }
        }

        internal void AddNextNode(GraphNode node) => _nextNodes.Add(node);

        internal void AddNextNodes(IEnumerable<GraphNode> nodes) => _nextNodes.AddRange(nodes);

        internal List<GraphNode> GetNextNodes() => _nextNodes;

        internal GraphNode GetNextNode(int idx) => _nextNodes[idx];

        internal int NextNodesCount => _nextNodes.Count;

        internal FactoryObjectParam ObjectInfo { get; }

        internal void AddIncome(int time, float income, Dictionary<string, float[]> quality)
        {            
            if (_qualityIncomes == null)
            {
                _qualityIncomes = quality;
            }
            else
            {
                foreach(var qualityName in OilParkModel.Qualitys)
                {
                    for (int i = 0; i < ObjectInfo.qualityTimeSplits; i++)
                    {
                        float Qvalue = _qualityIncomes[qualityName][i];
                        _qualityIncomes[qualityName][i] = (Qvalue * Income(time) + quality[qualityName][i] * income) / (Income(time) + income);
                    }
                }
            }
            _incomes[time].Add(income);
        }

        internal void SetOutcome(int time, float outcomeValue)
        {  
            _outcome[time] = outcomeValue;
            float incomeOnTime = Income(time);
            float massOnTime = _mass[time];
            _mass[time+1] = massOnTime + (incomeOnTime - outcomeValue) * timeSpan;

            float timeRange = timeSpan / ObjectInfo.qualityTimeSplits;

            float addedMass = (incomeOnTime - outcomeValue) * timeRange;
            float massIncome = incomeOnTime * timeRange;
            if (massOnTime > 0 || massIncome > 0)
            {
                foreach (var qualityName in OilParkModel.Qualitys)
                {
                    float Qvalue = Quality[qualityName][time][ObjectInfo.qualityTimeSplits - 1];
                    for (int i = 0; i < ObjectInfo.qualityTimeSplits; i++)
                    {
                        Quality[qualityName][time + 1][i] = (Qvalue * massOnTime + _qualityIncomes[qualityName][i] * massIncome) / (massOnTime + massIncome);
                        Qvalue = Quality[qualityName][time + 1][i];
                        massOnTime += addedMass;
                    }
                }
            }
        }

        internal bool NodeActive(int time) => _preNodesCount == _incomes[time].Count;

        internal void PreNodesCountInc() => _preNodesCount++;

        internal float Income(int time) => _incomes[time].Sum();

        internal float GetMassOnEndTime(int time)
        {
            return _mass[time];
        }

        internal void UpdateState()
        {
            _qualityIncomes = null;
        }

        internal void Clear()
        {
            for (int t = 0; t < _times; t++)
            {
                _incomes[t].Clear();
                _outcome[t] = 0;
                _mass[t + 1] = 0;
            }
        }
        
        internal int Id { get; }

        internal Dictionary<string, float[][]> Quality { get; set; }

        private List<GraphNode> _nextNodes;
            
        private int _preNodesCount = 0;
     
        private Dictionary<string, float[]> _qualityIncomes;
        
        private List<float>[] _incomes;

        private float[] _outcome;

        private float[] _mass;
       
        //hour
        private float timeSpan = 1;
        
        private int _times;
    } 
}
