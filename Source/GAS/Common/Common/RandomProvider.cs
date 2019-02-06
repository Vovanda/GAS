using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace GAS.Common
{
    public static class RandomProvider
    {
        private static long seed = Environment.TickCount;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IRnd GetSimpleLCG() => new ParkMillerLCG((uint)Interlocked.Increment(ref seed));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IRnd GetXORShiftRnd() => new XORShiftRandom(GetSimpleLCG());

        private static readonly ThreadLocal<IRnd> randomWrapper = new ThreadLocal<IRnd>(() => GetXORShiftRnd());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IRnd GetRnd()
        {
            return randomWrapper.Value;
        }
    }
}
