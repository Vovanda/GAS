using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class GraphNode
    {
        // Объявляем делегат
        public delegate void IsCalculatedHandler(float value, float share);
        // Событие, возникающее при окончании расчета расчета потока  
        public event IsCalculatedHandler FlowIsCalculated;

        //private readonly HashSet<Link> _linksToPeviousNodes = new HashSet<Link>();

        private List<int> _AllIdsLinksToPreNodes = new List<int>();

        private List<int> _NonActiveLinks = new List<int>();

        private readonly Dictionary<int, float> linksValues;

        //relative values

        private readonly List<int> _nextLinksIds = new List<int>();
        private readonly Dictionary<int, float> _nextLinksShares = new Dictionary<int, float>();
        
        public readonly string ID;        

        public GraphNode(string id) : this(id, null, null) { }

        public GraphNode(string id, IEnumerable<Link> linksIn, IEnumerable<Link> linksOut)
        {
            linksValues = new Dictionary<int, float>();
            _AllIdsLinksToPreNodes = new List<int>();
            _NonActiveLinks = new List<int>();
            this.ID = id;
            foreach(var inLink in linksIn)
            {
                SetInLink(inLink);
            }

            foreach (var outLink in linksOut)
            {
                SetInLink(outLink);
            }
        }

        public void SetInLink(Link link)
        {
            link.FlowIsCalculated += Link_FlowIsCalculated;
                
            _AllIdsLinksToPreNodes.Add(link.Id);
            _NonActiveLinks.Add(link.Id);
        }

        public void SetOutLink(Link link)
        {
            _nextLinksShares.Add(link.Id, 0);
            _nextLinksIds.Add(link.Id);
        }

        public void UpdateShares(int[] share)
        {
            //TODO: Оптимизировать код
            if(share.Length == _nextLinksIds.Count)
            {
                for(int i=0; i<= _nextLinksIds.Count; i++ )
                {
                    _nextLinksShares[_nextLinksIds[i]] = share[i];
                }
            }
        }

        public void Link_FlowIsCalculated(float value, int linkId)
        {
            linksValues[linkId] = value;
            _NonActiveLinks.Remove(linkId);
            if (!_NonActiveLinks.Any())
            {
                _NonActiveLinks = new List<int>(_AllIdsLinksToPreNodes);
                //TODO: переписать на просто уведомление
                FlowIsCalculated(GetFlow(), );
            }
        }        

        //relative values [percent/100] 
        public float CapasityIn { get; set; }
        public float CapasityOut { get; set; }
                
        //Capasity restriction [ton/hour]
        public float CMinIn { get; set; }
        public float CMaxIn  { get; set; }

        public float CMinOut { get; set; }
        public float CMaxOut { get; set; }

        public float GetFlow()
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this._parents.Count; }
        }
    }
}
