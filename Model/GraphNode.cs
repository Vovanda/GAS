using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Model
{
    public class GraphNode
    {        
        public GraphNode(int id) : this(id, null, null) { }

        public GraphNode(int id, IEnumerable<Link> linksIn, IEnumerable<Link> linksOut)
        {
            _idsLinksToPreNodes = new List<int>();
            _nonActiveIncomeLinks = new List<int>();
            this.id = id;
            if (linksIn != null)
            {
                foreach (var inLink in linksIn)
                {
                    SetInLink(inLink);
                }
            }

            if (linksOut != null)
            {
                foreach (var outLink in linksOut)
                {
                    SetOutLink(outLink);
                }
            }
        }

        public void Start(float incomeFlow)
        {
            IncomeFlow = incomeFlow;
            ReversSharesSum = 1.0f / _nextLinksShares.Values.Sum();
            FlowIsCalculated();
            SetDefaultState();
        }

        public void SetInLink(Link link)
        {
            link.FlowIsCalculated += Link_FlowIsCalculated; 
                
            _idsLinksToPreNodes.Add(link.Id);
            _nonActiveIncomeLinks.Add(link.Id);
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
                for(int i=0; i< _nextLinksIds.Count; i++ )
                {
                    _nextLinksShares[_nextLinksIds[i]] = share[i];
                }
            }
        }

        public float GetLinkShare(int linkId) => _nextLinksShares[linkId] * ReversSharesSum;

        public void Link_FlowIsCalculated(float value, int linkId)
        {
            _incomeFlow += value;
            _nonActiveIncomeLinks.Remove(linkId);
            if (!_nonActiveIncomeLinks.Any())
            {
                if (_nextLinksShares.Any())
                {
                    ReversSharesSum = 1.0f / _nextLinksShares.Values.Sum();
                }
                IncomeFlow = _incomeFlow;
                Debug.WriteLine($"{id} : {IncomeFlow}");
                FlowIsCalculated?.Invoke();
                SetDefaultState();  
            }
        }
                
        public void SetDefaultState()
        {
            _incomeFlow = 0;
            _nonActiveIncomeLinks = new List<int>(_idsLinksToPreNodes);
        }

        public delegate void IsCalculatedHandler();
        
        public event IsCalculatedHandler FlowIsCalculated;

        //relative values [percent/100] 
        public float CapasityIn { get; set; }
        public float CapasityOut { get; set; }
                
        //Capasity restriction [ton/hour]
        public float CMinIn { get; set; }
        public float CMaxIn  { get; set; }

        public float CMinOut { get; set; }
        public float CMaxOut { get; set; }

        public float IncomeFlow { get; private set; }
        
        public readonly int id;

        private float ReversSharesSum { get; set; }

        private float _incomeFlow  = 0;

        private List<int> _idsLinksToPreNodes = new List<int>();

        private List<int> _nonActiveIncomeLinks = new List<int>();

        private readonly List<int> _nextLinksIds = new List<int>();

        //relative values
        private readonly Dictionary<int, float> _nextLinksShares = new Dictionary<int, float>();
    }
}
