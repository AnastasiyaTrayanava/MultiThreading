/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static object _loqee = new object();
        private static  List<int> _sharedCollection;
        private static Mutex _mutex = new Mutex();
        private static ReaderWriterLockSlim _readerWriterLock = new ReaderWriterLockSlim();
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            // feel free to add your code
            
            _sharedCollection = new List<int>();

            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(AddElements, i);
                ThreadPool.QueueUserWorkItem(PrintCollection);
            }

            Console.ReadLine();
        }

        private static void PrintCollection(object obj)
        {
            lock (_loqee)
            {
                Console.WriteLine("Shared collection values: " + string.Join(' ', _sharedCollection));

            }
        }

        private static void AddElements(object obj)
        {
            lock (_loqee)
            {
                _sharedCollection.Add((int)obj);
            }
        }
    }
}
