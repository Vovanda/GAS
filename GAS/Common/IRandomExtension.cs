﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAS.Common
{

    public static class IRandomExtension
    {
        public static int Next(this IRandom rnd, int a, int b)
        {
            double value = rnd.NextDouble();
            return (int)(a + ((b - a) * value));
        }

        public static double NextDouble(this IRandom rnd, int a, int b)
        {
            return a + rnd.NextDouble() * (b - a);
        }

        public static float NextFloat(this IRandom rnd, float a, float b)
        {
            return a + rnd.NextFloat() * (b - a);
        }


        public static int NextBoolInt(this IRandom rnd)
        {
            return (int)(rnd.NextUInt() % 2);
        }

        public static bool NextBool(this IRandom rnd)
        {
            return (rnd.NextBoolInt() == 0) ? false : true;
        }
    }
}