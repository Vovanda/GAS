using System.Runtime.CompilerServices;

namespace GAS.Common
{
    public interface IRnd
    {
        int Next();
        uint NextUInt();
        double NextDouble();
        float NextFloat();
    }

    public static class IRndExtension
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Next(this IRnd rnd, int a, int b)
        {
            return a + (int)((b - a) * rnd.NextDouble());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double NextDouble(this IRnd rnd, int a, int b)
        {
            return a + rnd.NextDouble() * (b - a);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float NextFloat(this IRnd rnd, float a, float b)
        {
            return a + rnd.NextFloat() * (b - a);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int NextBoolInt(this IRnd rnd)
        {
            // x & 1 <=> x % 2
            return (int)(rnd.NextUInt() & 1); 
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool NextBool(this IRnd rnd)
        {
            return rnd.NextBoolInt() == 1;
        }
    }
}
