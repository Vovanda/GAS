using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OilParkSM
{
    internal class OilParkItem
    {
        public OilParkItem(int id, FOParam param, int times)
        {
            Id = id;
            ObjectInfo = param;
            _nextItems = new List<OilParkItem>();
            _incomes = new List<float>[times];
            _outcome = new float[times];
            _mass = new float[(times + 1)];
            _mass[0] = param.MassBegin;

            for (int i=0; i < times; i++)
            {
                _incomes[i] = new List<float>();
            }

            _times = times;
            Quality = new Dictionary<string, float[][]>();
            foreach(var qualityName in OilParkModel.Qualitys)
            {               
                Quality[qualityName] = new float[times + 1][];
                var qualityValue = param.QualityBegin != null ? param.QualityBegin[qualityName] : 0;
                Quality[qualityName][0] = Enumerable.Repeat(qualityValue, OilParkModel.QualityTimeSplits).ToArray(); 
                for(int i = 1; i < times+1; i++)
                {
                    Quality[qualityName][i] = new float[OilParkModel.QualityTimeSplits];
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void AddNextItem(OilParkItem Item)
        {
            _nextItems.Add(Item);
            Item.PreItemsCountInc();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal IList<OilParkItem> GetNextItems() => _nextItems;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal OilParkItem GetNextItem(int idx) => _nextItems[idx];

        internal int NextItemsCount => _nextItems.Count;

        internal FOParam ObjectInfo { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
                    for (int i = 0; i < OilParkModel.QualityTimeSplits; i++)
                    {
                        float Qvalue = _qualityIncomes[qualityName][i];
                        _qualityIncomes[qualityName][i] = (Qvalue * Income(time) + quality[qualityName][i] * income) / (Income(time) + income);
                    }
                }
            }
            _incomes[time].Add(income);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void SetOutcome(int time, float outcomeValue)
        {  
            _outcome[time] = outcomeValue;
            float incomeOnTime = Income(time);
            float massOnTime = _mass[time];
            _mass[time+1] = massOnTime + (incomeOnTime - outcomeValue) * timeSpan;

            float timeRange = timeSpan / OilParkModel.QualityTimeSplits;

            float addedMass = (incomeOnTime - outcomeValue) * timeRange;
            float massIncome = incomeOnTime * timeRange;
            if (massOnTime > 0 || massIncome > 0)
            {
                foreach (var qualityName in OilParkModel.Qualitys)
                {
                    float Qvalue = Quality[qualityName][time][OilParkModel.QualityTimeSplits - 1];
                    for (int i = 0; i < OilParkModel.QualityTimeSplits; i++)
                    {
                        Quality[qualityName][time + 1][i] = (Qvalue * massOnTime + _qualityIncomes[qualityName][i] * massIncome) / (massOnTime + massIncome);
                        Qvalue = Quality[qualityName][time + 1][i];
                        massOnTime += addedMass;
                    }
                }
            }
        }

        internal bool ItemActive(int time) => _preItemsCount == _incomes[time].Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void PreItemsCountInc() => _preItemsCount++;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal float Income(int time)
        {
            float result = 0;
            for (int i = 0; i < _incomes[time].Count; i++)
            {
                result += _incomes[time][i];
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal float GetMassOnEndTime(int time)
        {
            return _mass[time];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void UpdateState()
        {
            _qualityIncomes = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        private IList<OilParkItem> _nextItems;
            
        private int _preItemsCount = 0;
     
        private IDictionary<string, float[]> _qualityIncomes;
        
        private IList<float>[] _incomes;

        private float[] _outcome;

        private float[] _mass;
       
        //hour
        private float timeSpan = 1;
        
        private int _times;
    } 
}
