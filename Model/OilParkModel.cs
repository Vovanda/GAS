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

        readonly List<Task<float>> _tasks;


        public OilParkModel(List<GraphNode> Nodes, List<Link> Links)
        {
            _tasks = new List<Task<float>>();
        }

        public async Task<float> Start(Dictionary<int, float> shares)
        {
            var task = Task.Run(() => Calculate(shares));
            _tasks.Add(task);
            return await task;
        }

        private float Calculate(Dictionary<int, float> shares)
        {
            foreach (var sourceObject in Sources)
            {
                sourceObject.Start(100, shares);
            }
            return 0;
        }
    }
}
