using System;
using System.Collections.Generic;
using System.Linq;
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
        GraphNode[] graphNodes;
        readonly List<Task<float>> _tasks;

        public OilParkModel((List<int> NextObjList, FactoryObjectParam Params)[] linksMap)
        {
            graphNodes = new GraphNode[linksMap.Length];
            for (int i = 0; i < linksMap.Length; i++)
            {
                GraphNode node = null;
                if (linksMap[i].Params.Type == FactoryObjectType.Tank)
                {
                    node = new Tank(i) { ObjectInfo = linksMap[i].Params };
                }
                else if (linksMap[i].Params.Type == FactoryObjectType.Pipe)
                {

                    node = new Pipe(i) { ObjectInfo = linksMap[i].Params };
                }

                for (int j = 0; j <= linksMap[i].NextObjList.Count; j++)
                {
                    Link link = new Link(node) { NextObjId = linksMap[i].NextObjList[j] };
                    Links.Add(link);
                    node.SetOutLink(link);
                }
                graphNodes[i] = node;
            }

            for (int i = 0; i < graphNodes.Length; i++)
            {
                var inputLinks = Links.Where(x => x.NextObjId == i);
                graphNodes[i].SetInLinks(inputLinks);
            }
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
