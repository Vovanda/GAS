using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class OilParkModel
    {
        List<GraphNode> Sources = new List<GraphNode>();
        List<GraphNode> Receivers = new List<GraphNode>();
        List<GraphNode> Tanks = new List<GraphNode>();
        List<GraphNode> Pipes = new List<GraphNode>();
        List<Link> Links = new List<Link>();

        public OilParkModel(List<GraphNode> Nodes, List<Link> Links)
        {

        }

        public async Task<float> Start(float[] shares)
        {
            return Task.Run(() => {

            });
        }

        private float Calculate(float[] shares)
        {


        }


    }
}
