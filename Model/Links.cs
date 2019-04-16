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

        public Link(GraphNode previousNode) 
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
               
        public int Id { get; set; }
        public int NextObjId { get; set; }
        public float GetFlow() => PreviousNode.OutputFlow * PreviousNode.GetLinkShare(Id);
    }
}
