using GAS.Common;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = RandomProvider.GetThreadRandom();
            var gauss = new Gauss(rnd);

            Console.WriteLine("Нормальное распределение с М=0, D=1");
            double g_count = 0;

            int g_count_1q = 0;
            int g_count_2q = 0;
            int g_count_3q = 0;
            double g_value;
            do
            {
                g_value = gauss.Next();
                g_count++;
                if(-1 <= g_value && g_value <= 1)
                {
                    g_count_1q++;
                }
                if (-2 <= g_value && g_value <= 2)
                {
                    g_count_2q++;
                }
                if (-3 <= g_value && g_value <= 3)
                {
                    g_count_3q++;
                }

                Console.WriteLine($"1: {g_count_1q / g_count * 100} 2:{ g_count_2q / g_count * 100} 3: {g_count_3q / g_count * 100}");

            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            Console.WriteLine("Равномерное распределение XorShift [0,1)");
            double XScount = 0;
            int XScount1 = 0;
            int XScount2 = 0;

            do
            {
                Console.WriteLine($"{rnd.NextDouble()}");
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            float some_num() => rnd.NextFloat(0, 100);

            var vector = new Vector(new[] { some_num(), some_num(), some_num() });


            Console.WriteLine($"vector {vector}");
            vector += 1;
            Console.WriteLine($"vector {vector}");

            var vector2 = vector + 2;
            Console.WriteLine($"vector2 {vector2}");

            vector += 1;
            Console.WriteLine($"vector {vector}");
            Console.WriteLine($"vector2 {vector2}");

            Console.ReadKey(true);
        }
    }
}
