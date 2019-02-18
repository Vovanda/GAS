using System;
using System.Threading;

namespace GAS.Common
{
    public static class RandomProvider
    {
        private static long seed = Environment.TickCount;

        private static ThreadLocal<IUniform> randomWrapper = new ThreadLocal<IUniform>(() =>
            new XORShiftRandom(new ParkMillerLCG((uint)Interlocked.Increment(ref seed)))
        );

        public static IUniform GetThreadRandom()
        {
            return randomWrapper.Value;
        }
    }
}
