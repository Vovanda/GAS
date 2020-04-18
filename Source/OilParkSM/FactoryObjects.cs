using System;
using System.Collections.Generic;
using System.Text;

namespace OilParkSM
{
    internal class Pipe : OilParkItem
    {
        public Pipe(int id, FOParam param, int times)
            : base(id, param, times) { }
    }

    internal class Tank : OilParkItem
    {
        public Tank(int id, FOParam param, int times)
            : base(id, param, times) { }
    }
}
