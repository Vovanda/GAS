namespace GAS.Common
{
    public static class IUniformExtension
    {
        public static int Next(this IUniform rnd, int a, int b)
        {
            return a + (int)((b - a) * rnd.NextDouble());
        }

        public static double NextDouble(this IUniform rnd, int a, int b)
        {
            return a + rnd.NextDouble() * (b - a);
        }

        public static float NextFloat(this IUniform rnd, float a, float b)
        {
            return a + rnd.NextFloat() * (b - a);
        }
        
        public static int NextBoolInt(this IUniform rnd)
        {
            return (int)(rnd.NextUInt() % 2);
        }

        public static bool NextBool(this IUniform rnd)
        {
            return (rnd.NextBoolInt() == 0) ? false : true;
        }
    }
}
