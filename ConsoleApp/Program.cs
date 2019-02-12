using GAS.Common;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = RandomProvider.GetThreadRandom();
            do
            {
                Console.WriteLine($"{rnd.Next(10, 20)}");
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
