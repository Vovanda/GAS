using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Model
{
    internal class Tank : GraphNode
    {
        public Tank(int id, FactoryObjectParam param, int times) : base(id, param, times)
        {
        }
    }
}
