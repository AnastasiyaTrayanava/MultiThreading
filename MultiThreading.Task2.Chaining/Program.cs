/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            var rand = new Random();

            var numberOne = Task.Run(() =>
            {
                return Enumerable.Repeat(0, 10).Select(x => rand.Next(0, 10)).ToArray();
            });
            PrintArray(numberOne.Result);
            var numberTwo = numberOne.ContinueWith(x =>
            {
                var randomInteger = rand.Next(10);
                return x.Result.Select(a => a * randomInteger).ToArray();
            });
            PrintArray(numberTwo.Result);
            var numberThree = numberTwo.ContinueWith(x =>
            {
                var arr = numberTwo.Result;
                Array.Sort(arr);
                return arr;
            });
            PrintArray(numberThree.Result);
            var numberFour = numberThree.ContinueWith(x => x.Result.Average());
            Console.WriteLine(numberFour.Result);

            Console.ReadLine();
        }

        private static void PrintArray(int[] arr)
        {
            var joined = string.Join(" ", arr);
            Console.WriteLine(joined);
        }
    }
}
