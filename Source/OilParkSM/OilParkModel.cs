using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OilParkSM
{
    public class OilParkModel
    {
        public static  string[] Qualitys = new[] { "ОЧ", "Плотность" };

        public static int QualityTimeSplits = 10;

        OilParkItem[] parkItems;

        List<int> Tanks = new List<int>();
        List<int> Pipes = new List<int>();

        Dictionary<OilParkItem, float[]> FirstItems = new Dictionary<OilParkItem, float[]>();
        Dictionary<OilParkItem, Dictionary<string, float[]>> SourceQuality = new Dictionary<OilParkItem, Dictionary<string, float[]>>();
        Dictionary<int, int> sharesIndex = new Dictionary<int, int>();
        List<int> Receivers = new List<int>();
        List<OilParkItem> VariableFlowItems = new List<OilParkItem>();

        public OilParkModel((FOParam ObjectParams, int[] NextObjectsIdx,  float[] FlowValues, Dictionary<string, float> QualityValues)[] linksMap, int times)
        {
            Times = times;

            parkItems = new OilParkItem[linksMap.Length];
            int shareIndex = 0;
            for (int i = 0; i < linksMap.Length; i++)
            {
                OilParkItem Item = null;

                if (linksMap[i].ObjectParams.Type == FOType.Tank)
                {
                    Item = new Tank(i, linksMap[i].ObjectParams, Times);
                    Tanks.Add(i);
                    VariableFlowItems.Add(Item);
                }
                else if (linksMap[i].ObjectParams.Type == FOType.Pipe)
                {
                    Item = new Pipe(i, linksMap[i].ObjectParams, Times);
                    Pipes.Add(i);
                    if (linksMap[i].NextObjectsIdx.Length > 1)
                    {
                        VariableFlowItems.Add(Item);
                    }
                }

                parkItems[i] = Item;

                if (linksMap[i].FlowValues != null)
                {
                    FirstItems[parkItems[i]] = linksMap[i].FlowValues;
                    SourceQuality[parkItems[i]] = new Dictionary<string, float[]>();

                    for(int j = 0; j < Qualitys.Length; j++)
                    {
                        string qualityName = Qualitys[j];
                        float qualityValue = linksMap[i].QualityValues[qualityName];
                        SourceQuality[parkItems[i]][qualityName] = Enumerable.Repeat(qualityValue, OilParkModel.QualityTimeSplits).ToArray();
                    }
                    sharesIndex[CombineHashCodes(i, i)] = shareIndex++;
                }
            }

            for (int i = 0; i < linksMap.Length; i++)
            {
                for(int j = 0; j < linksMap[i].NextObjectsIdx.Length; j++)
                {
                    int ItemId = linksMap[i].NextObjectsIdx[j];
                    parkItems[i].AddNextItem(parkItems[ItemId]);

                    if (linksMap[i].NextObjectsIdx.Length > 1 || (parkItems[i].ObjectInfo.Type == FOType.Tank && linksMap[i].NextObjectsIdx.Length == 1))
                    {
                        sharesIndex[CombineHashCodes(i, ItemId)] = shareIndex++;
                    }
                }

                if (linksMap[i].NextObjectsIdx.Length == 0)
                {
                    Receivers.Add(i);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void BFS(int time, float[] sharesOnTimeInterval)
        {
            Stack<OilParkItem> queue = new Stack<OilParkItem>();
            foreach (var Item in FirstItems.Keys)
            {
                int hash = CombineHashCodes(Item.Id, Item.Id);
                int shareIdx = sharesIndex[hash];
                float share = sharesOnTimeInterval[shareIdx];
                float flow = FirstItems[Item][time] * share;
                Item.AddIncome(time, flow, SourceQuality[Item]);
                Item.SetOutcome(time, flow);
                queue.Push(Item);
            }

            while (queue.Count > 0)
            {
                var Item = queue.Pop();
                int countChildren = Item.NextItemsCount;
                int shareIdxHash = CombineHashCodes(Item.Id, Item.Id);

                float share = (Item.NextItemsCount > 0) ? 1 : 0;
                if (sharesIndex.TryGetValue(shareIdxHash, out int shareIdx))
                {
                    share = sharesOnTimeInterval[shareIdx];
                }

                float outcome = share * Item.Income(time);
                Item.SetOutcome(time, outcome);

                if (countChildren > 1)
                {
                    float childrenSharesSum = 0;
                    for (int i = 0; i < countChildren; i++)
                    {
                       
                        int idxHash = CombineHashCodes(Item.Id, Item.GetNextItem(i).Id);
                        childrenSharesSum += sharesOnTimeInterval[sharesIndex[idxHash]];
                    }
                    float sharesSumInv = 1 / childrenSharesSum;

                    for (int i = 0; i < countChildren; i++)
                    {
                        var child = Item.GetNextItem(i);
                        int childShareIdxHash = CombineHashCodes(Item.Id, child.Id);

                        float childShare = 1;
                        if (sharesIndex.TryGetValue(childShareIdxHash, out int childShareIdx))
                        {
                            childShare = sharesOnTimeInterval[childShareIdx];
                        }

                        float childIncome = childShare * sharesSumInv * outcome;
                        Dictionary<string, float[]> quality = new Dictionary<string, float[]>();

                        foreach (var qualityName in OilParkModel.Qualitys)
                        {
                            quality[qualityName] = new float[OilParkModel.QualityTimeSplits];
                            Item.Quality[qualityName][time + 1].CopyTo(quality[qualityName], 0);
                        }

                        child.AddIncome(time, childIncome, quality);
                        if (child.ItemActive(time))
                        {
                            queue.Push(child);
                        }
                    }
                }
                else if (countChildren == 1)
                {
                    var child = Item.GetNextItem(0);
                    Dictionary<string, float[]> quality = new Dictionary<string, float[]>();
                    foreach (var qualityName in OilParkModel.Qualitys)
                    {
                        quality[qualityName] = new float[OilParkModel.QualityTimeSplits];
                        Item.Quality[qualityName][time + 1].CopyTo(quality[qualityName], 0);
                    }

                    child.AddIncome(time, outcome, quality);

                    if (child.ItemActive(time))
                    {
                        queue.Push(child);
                    }
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < parkItems.Length; i++)
            {
                parkItems[i].Clear();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateItem()
        {
            for (int i = 0; i < parkItems.Length; i++)
            {
                parkItems[i].UpdateState();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int CombineHashCodes(int h1, int h2)
        {
            // https://referencesource.microsoft.com/#System.Numerics/System/Numerics/HashCodeHelper.cs

            return ((h1 << 5) + h1) ^ h2;
        }

        public void StartModel(float[] sharesOnAllTime)
        {
            for (int t = 0; t < Times; t++)
            {
                float[] currentShareArray = new float[sharesIndex.Count];
                Array.Copy(sharesOnAllTime, sharesIndex.Count * t, currentShareArray, 0, sharesIndex.Count);
                BFS(t, currentShareArray);
                UpdateItem();
            }
        }

        public float GetValue()
        {
            float result = 0;
            foreach (var i in Receivers)
            {
                result += parkItems[i].GetMassOnEndTime(Times);
            }
            return result;
        }

        public int Times { get; }
    }
}