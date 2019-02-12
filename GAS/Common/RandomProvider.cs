using System;
using System.Threading;

namespace GAS.Common
{
    public static class RandomProvider
    {
        private static int seed = Environment.TickCount;

        private static ThreadLocal<ParkMillerLCG> randomWrapper = new ThreadLocal<ParkMillerLCG>(() =>
            new ParkMillerLCG(Interlocked.Increment(ref seed))
        );

        public static ParkMillerLCG GetThreadRandom()
        {
            return randomWrapper.Value;
        }
    }
}
