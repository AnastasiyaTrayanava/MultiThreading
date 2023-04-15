/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        private static int _threadCount = 0;
        private static int _threadPoolCount = 0;
        private static SemaphoreSlim _semaphore = new SemaphoreSlim(10);
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            Console.WriteLine("Thread+Join: ");
            var thread = new Thread(CreateThreadsRecursively);
            thread.Start(10);
            thread.Join();

            Console.WriteLine("ThreadPool+Semaphore: ");
            ThreadPool.QueueUserWorkItem(CreateThreadPoolRecursively, 10);

            Console.ReadLine();
        }

        private static void CreateThreadPoolRecursively(object state)
        {
            if (_threadPoolCount >= 10)
            {
                return;
            }

            _threadPoolCount++;

            var number = (int)state;
            number--;
            ThreadPool.QueueUserWorkItem(CreateThreadPoolRecursively, number);

            _semaphore.Wait();

            Console.WriteLine(number);
        }

        private static void CreateThreadsRecursively(object state)
        {
            if (_threadCount >= 10 )
            {
                return;
            }

            var thread = new Thread(CreateThreadsRecursively);
            _threadCount++;

            var number = (int)state;
            number--;
            thread.Start(number);
            thread.Join();

            // bug: counting is backwards
            Console.WriteLine(number);
        }
    }
}
