using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Link
    {
        // Объявляем делегат
        public delegate void IsCalculatedHandler(float value, int id, Dictionary<int, float> shares);
        // Событие, возникающее при окончании расчета расчета потока  
        public event IsCalculatedHandler FlowIsCalculated;

        public Link(GraphNode previousNode, double minCapasity, double maxCapasity) 
        {
            PreviousNode = previousNode;
            PreviousNode.FlowIsCalculated += OnFLowCalculated;
        }

        public void OnFLowCalculated(Dictionary<int, float> shares)
        {
            //Посылка уведомления о получении значения потока
            FlowIsCalculated?.BeginInvoke(GetFlow(), Id, shares, null, null);
        }
        
        public GraphNode PreviousNode { get; private set; }

        //relative values [percent/100] 

        public float MinCapasity { get; }
        public float MaxCapasity { get; }

        public int Id { get; set; }
        public float GetFlow() => PreviousNode.OutputFlow * PreviousNode.GetLinkShare(Id);
    }
}
