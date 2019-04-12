using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Model
{
    public class GraphNode
    {        
        public GraphNode(int id) : this(id, null, null) { }

        public GraphNode(int id, IEnumerable<Link> linksIn, IEnumerable<Link> linksOut)
        {
            ValueIsExist.Add(false);
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

        public void Start(float incomeFlow, Dictionary<int, float> shares)
        {
            IncomeFlow = incomeFlow;
            FlowsAtTime.Add(IncomeFlow);
            ValueIsExist.Add(true);
            ReversSharesSum = 1.0f / _nextLinksShares.Values.Sum();
            FlowIsCalculated?.BeginInvoke(shares, CallBack, null);
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

        public void UpdateLinksShares(int[] share)
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

        public void Link_FlowIsCalculated(float value, int linkId, Dictionary<int, float> shares)
        {
            if(ValueIsExist.Count <= time)
            {
                ValueIsExist.Add(false);
            }

            _incomeFlow += value;
            _nonActiveIncomeLinks.Remove(linkId);
            if (!_nonActiveIncomeLinks.Any())
            {
                if (_nextLinksShares.Any())
                {
                    ReversSharesSum = 1.0f / _nextLinksShares.Values.Sum();
                }
                IncomeFlow = _incomeFlow;
                FlowsAtTime.Add(IncomeFlow);
                ValueIsExist[time] = true;
                waitHandler.Set();
                Debug.WriteLine($"{id} : {IncomeFlow}");
                FlowIsCalculated?.BeginInvoke(shares, CallBack, null);
           
            }
        }
            
        public void CallBack(IAsyncResult ar)
        {
            SetDefaultState();
        }
    
        public void SetDefaultState()
        {
            _incomeFlow = 0;
            _nonActiveIncomeLinks = new List<int>(_idsLinksToPreNodes);
            ValueIsExist.Add(false);
            time++;
        }

        public delegate void IsCalculatedHandler(Dictionary<int, float> shares);
        
        public event IsCalculatedHandler FlowIsCalculated;

        //relative values [percent/100] 
        //public float CapasityIn { get; set; }
        public float CapasityOut { get; set; }
                
        //Capasity restriction [ton/hour]
        public float CMinIn { get; set; }
        public float CMaxIn  { get; set; }

        public float CMinOut { get; set; }
        public float CMaxOut { get; set; }

        public float IncomeFlow { get; private set; }
        
        public readonly int id;

        private float ReversSharesSum { get; set; }

        public float OutputFlow => IncomeFlow * CapasityOut;

        private float _incomeFlow  = 0;

        private List<int> _idsLinksToPreNodes = new List<int>();

        private List<int> _nonActiveIncomeLinks = new List<int>();

        private readonly List<int> _nextLinksIds = new List<int>();

        private List<bool> ValueIsExist = new List<bool>();

        private int time = 0;

        private List<float> FlowsAtTime = new List<float>();

        public  float GetIncomeByTime(int t)
        {
            if(ValueIsExist[t])
            {
                return FlowsAtTime[t];
            }
            else
            {
                bool result = waitHandler.WaitOne();
                return FlowsAtTime[t];
            }
        }

        public int OutLinkCount => _nextLinksShares.Count;

        //relative values
        private readonly Dictionary<int, float> _nextLinksShares = new Dictionary<int, float>();
        public AutoResetEvent waitHandler = new AutoResetEvent(false);
    }
}
