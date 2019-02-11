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
            //Множество продуков
            var J = new HashSet<int>(){ 1, 2};
            //Множество компонентов
            var I = new HashSet<int>() { 1, 2, 3};
            //Множество планирования 
            var D = new HashSet<int>() {1};
            //Множество периодов планирования 
            var P = new HashSet<int>() { 1, 2, 3 };
            //Множество показателей качества 
            var L = new HashSet<int>() { 1, 2 };
            //Множество емкостей компонентов
            var E = new HashSet<int>() { 1, 2, 3 };
            //Множество интервалов планирования
            var TIME = new HashSet<int>() { 1, 2, 3 };
            //значение l-го ПК i-го компонента
            var PK = new [,] { {92, 0.76 }, {95, 0.6 }, {90, 0.8} };
            //мин значение l-го ПК j-го продукта
            var spec_min = new[,] { { 91, 0.7 }, { 93, 0.7 } };
            //мах значение l-го ПК j-го продукта
            var spec_max = new[,] { { 92, 0.8 }, { 94, 0.8 } };

            double[] B = new double[] { 0, 3, 6, 10 };
            var first = Sum._(i => B[i], 0, 4);

            Constraints constraints = new Constraints();

            Func<IEnumerable<double>, bool> expration1 = (x) =>
            {
                var param = x.ToArray();
                return Sum._(i => B[i], 0, 4) * param[0] < 0;
            };            

            var set1 = new [] { -10.0, 30.0, 50.0 }.ToHashSet();

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
