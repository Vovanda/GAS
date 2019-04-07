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

        public void OnFLowCalculated(float value, float share)
        {
            //Посылка уведомления о получении значения потока
            FlowIsCalculated(GetFlow(value,float share), Id);
        }
        
        public GraphNode PreviousNode { get; private set; }

        //relative values [percent/100] 
        public float Share { get; set; }

        public float MinCapasity { get; }
        public float MaxCapasity { get; }

        public int Id { get; }
        public float GetFlow(float value, float share) => value * share;
    }
}
