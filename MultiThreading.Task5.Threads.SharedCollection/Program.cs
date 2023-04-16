/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static List<int> _sharedCollection;
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            _sharedCollection = new List<int>();

            for (var i = 0; i < 10; i++)
            {
                var numberOne = Task.Run(() => AddElements(i));
                numberOne.Wait();
                var numberTwo = numberOne.ContinueWith(PrintCollection);
                numberTwo.Wait();
            }

            Console.ReadLine();
        }

        private static void PrintCollection(object obj)
        {
            Console.WriteLine("Shared collection values: " + string.Join(' ', _sharedCollection));
        }

        private static void AddElements(object obj)
        {
            _sharedCollection.Add((int)obj);
        }
    }
}
