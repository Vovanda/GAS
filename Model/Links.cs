using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Link
    {
        // Объявляем делегат
        public delegate void IsCalculatedHandler(float value, int id);
        // Событие, возникающее при окончании расчета расчета потока  
        public event IsCalculatedHandler FlowIsCalculated;

        public Link(GraphNode previousNode, double minCapasity, double maxCapasity) 
        {
            PreviousNode = previousNode;
            PreviousNode.FlowIsCalculated += OnFLowCalculated;
        }

        public void OnFLowCalculated()
        {
            //Посылка уведомления о получении значения потока
            FlowIsCalculated?.BeginInvoke(GetFlow(), Id, null, null);
        }
        
        public GraphNode PreviousNode { get; private set; }

        //relative values [percent/100] 
        public float Share { get; set; }

        public float MinCapasity { get; }
        public float MaxCapasity { get; }

        public int Id { get; set; }
        public float GetFlow() => PreviousNode.IncomeFlow * PreviousNode.GetLinkShare(Id);
    }
}
