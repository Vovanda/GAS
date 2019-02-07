using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMathModel
{
    class Program
    {
        static void Main(string[] args)
        {
            Sum Sum = new Sum().SetAdditionFunc((double a, double b) => a + b);
            double[] B = new double[] { 0, 3, 6, 10 };
            var first = Sum._(i => B[i], 0, 4);

            Constraints constraints = new Constraints();

            Func<IEnumerable<dynamic>, bool> expration1 = (x) =>
            {
                var param = x.ToArray();
                return Sum._(i => B[i], 0, 4) * param[0] < 0;
            };            

            ISet<dynamic> set1 = (new dynamic[] { -10, 30, 50 }).ToHashSet();

            constraints.Add(expration1, new[] { set1 });

            //act
            var constraintsResults = constraints.Calculate();

            foreach(var result in constraintsResults)
            {
                PrintResult(result);
            }

            Console.ReadKey();
        }

        static void PrintResult(IDictionary<string, bool> dictionary)
        {
            foreach (KeyValuePair<string, bool> kvp in dictionary)
            {
                Console.WriteLine("{0} | {1}", kvp.Key, kvp.Value);
            }
        }

    }
}
