using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Model.Tests
{
    [TestClass]
    public class LinearModelTests
    {
        [TestMethod]
        public void DirectDistributionTest()
        {
            var nodes = new List<GraphNode>();
            var links = new List<Link>();
            int count = 0;
            for (int i = 0; i <= 100; i++)
            {
                var node = new GraphNode(count++);                
                var link = new Link(node, 0, int.MaxValue) { Id = count++ };
                
                if (links.Any())
                {
                    node.SetInLink(links.Last());
                }
                links.Add(link);
                node.SetOutLink(link);
                node.UpdateShares(new int []{1});
                nodes.Add(node);
            }
            var lastNode = new GraphNode(count++);
            lastNode.SetInLink(links.Last());
            nodes[0].Start(100);
            Thread.Sleep(1000);
            float act = lastNode.GetIncomeByTime(0);
            Assert.AreEqual(100, act);
        }
    }
}
