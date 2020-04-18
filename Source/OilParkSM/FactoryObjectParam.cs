using System.Collections.Generic;

namespace OilParkSM
{
    //FactoryObjectParam
    public class FOParam
    {
        public FOParam(FOType type)
        {
            Type = type;
        }

        public float MassBegin;

        public Dictionary<string, float> QualityBegin;

        public readonly FOType Type;

        public Сonstraint Mass;

        public Сonstraint FlowRateIn;

        public Сonstraint FlowRateOut;
                
        public HashSet<int> UnavailableTime;       
    }

    //FactoryObjectType
    public enum FOType
    {
        Tank,
        Pipe
    }

    public struct Сonstraint
    {
        float Min;
        float Max;
    }
}
