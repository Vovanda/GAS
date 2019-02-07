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

            Console.WriteLine(Sum._(i => B[i], 0, 4));
            Constraint constraint = new Constraint(() => first < 19);


            Console.ReadKey(true);
        }
    }
}
