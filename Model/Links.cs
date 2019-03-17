using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    internal class Link
    {
        public Link(double minCapasity, double maxCapasity) : this(null,  minCapasity,  maxCapasity)
        {

        }

        public Link(GraphNode previousNode, double minCapasity, double maxCapasity) 
        {
            P
        }

        public GraphNode PreviousNode { get; set; }
        public float Capacity { get; set; }
        public float MinCapasity { get; }
        public float MaxCapasity { get; }
        public string Id { get; }
        public float GetFlow()
        {
            float result;
            PreviousNode.GetFlow(MinCapasity, MaxCapasity, out result);
            return result;
        }
    }
}
