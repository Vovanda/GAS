using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class FactoryObjectParam
    {
        public FactoryObjectType Type { get; set; }
        //Capasity restriction [ton/hour]
        public float CMinIn { get; set; }
        public float CMaxIn { get; set; }

        public float CMinOut { get; set; }
        public float CMaxOut { get; set; }

        public float MinMass { get; set; }

        public float StartMass { get; set; }

        //public float MaxLevel { get; set; }

        HashSet<int> UnavailableTime { get; set; }

        public Dictionary<string, float> QualityStart { get; set; }

        public int qualityTimeSplits = 50;
        
    }

    public enum FactoryObjectType
    {
        Tank,
        Pipe
    }
}
