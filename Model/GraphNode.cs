using System;
using System.Collections;
using System.Collections.Generic;

namespace Model
{
    public class GraphNode
    {
        private readonly Dictionary<string, Link> _linksToPreviousNodes = new Dictionary<string, Link>();
        // [%/100]
        private readonly List<float> _nextNodesCapasitys = new List<float>();


        public readonly string ID;        

        public GraphNode(string id)
        {
            this.ID = id;
        }

        public GraphNode GetParent(string id)
        {
            return this._parents[id];
        }


        public void Add(GraphNode item)
        {
            Link link = new Link(item);
        }

        public void SetChildrenCapasity(string id, float capasity)
        {
            if(_nextNodesCapasitys.ContainsKey(id))
            {
                _nextNodesCapasitys[id] = capasity;
            }
        }
        // [%/100] 
        public float CapasityIn { get; set; }
        public float CapasityOut { get; set; }

        // [t/h]
        public float MinCapasityIn  { get; set; }
        public float MaxCapasityIn  { get; set; }

        public float MinCapasityOut { get; set; }
        public float MaxCapasityOut { get; set; }

        public void GetFlow(float minCapasity, float maxCapasity, out float result)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this._parents.Count; }
        }
    }
}
