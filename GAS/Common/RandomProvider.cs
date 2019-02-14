using System;
using System.Threading;

namespace GAS.Common
{
    public static class RandomProvider
    {
        private static long seed = Environment.TickCount;

        private static ThreadLocal<IRandom> randomWrapper = new ThreadLocal<IRandom>(() =>
            new XORShiftRandom(new ParkMillerLCG((uint)Interlocked.Increment(ref seed)))
        );

        public static IRandom GetThreadRandom()
        {
            return randomWrapper.Value;
        }
    }
}
