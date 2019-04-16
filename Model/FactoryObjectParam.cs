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

        public float Level { get; set; }

        HashSet<int> UnavailableTime { get; set; }
    }

     public enum FactoryObjectType
    {
        Tank,
        Pipe
    }
}
