using System;
using System.Collections.Generic;
using System.Linq;

namespace TestMathModel
{
    internal class Sum
    {
        public static double _(Func<int, double> expression, int count) => _(expression, Enumerable.Range(0, count).ToHashSet());

        public static double _(Func<int, double> expression, int start, int end) => _(expression, Enumerable.Range(start, end - start).ToHashSet());

        public static double _(Func<int, double> expression, ISet<int> set)
        {
            double result = 0;
            if (set.Any())
            {
                using (var e = set.GetEnumerator())
                {
                    checked
                    {
                        while (e.MoveNext())
                        {
                            result += expression(e.Current);
                        }
                    }
                }
            }
            return result;
        }
    }
}
