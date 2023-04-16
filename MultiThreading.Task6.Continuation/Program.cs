/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            CriteriaOne();
            CriteriaTwo();
            CriteriaThree();
            CriteriaFour();

            Console.ReadLine();
        }

        static void CriteriaOne()
        {
            Console.WriteLine("Criteria #1:");

            var numberOne = Task.Run(() =>
            {
                Console.WriteLine("Executing parent task.");
            });
            var numberTwo = numberOne.ContinueWith(x =>
            {
                Console.WriteLine("Executing secondary task.");
            });

            Task.WaitAll();
        }

        static void CriteriaTwo()
        {
            Console.WriteLine("Criteria #2:");

            var numberOne = Task.Run(() =>
            {
                Console.WriteLine("Throwing exception...");
                throw new ArgumentException("Incorrect data");
            });
            var numberTwo = numberOne.ContinueWith(x =>
            {
                Console.WriteLine("Executing secondary task after parent failure.");
            }, TaskContinuationOptions.OnlyOnFaulted);

            Task.WaitAll();
        }

        static void CriteriaThree()
        {
            Console.WriteLine("Criteria #3:");

            var numberOne = Task.Run(() =>
            {
                Console.WriteLine("Throwing exception...");
                throw new ArgumentException("Incorrect data");
            });
            var numberTwo = numberOne.ContinueWith(x =>
            {
                Console.WriteLine("Executing secondary task after parent failure and reusing a thread.");
            }, TaskContinuationOptions.OnlyOnFaulted & TaskContinuationOptions.ExecuteSynchronously);

            Task.WaitAll();
        }

        static void CriteriaFour()
        {
            var tokenSource = new CancellationTokenSource();
            var ct = tokenSource.Token;
            Console.WriteLine("Criteria #4:");

            var numberOne = Task.Run(() =>
            {
                Console.WriteLine("Executing parent task.");
            }, ct);

            tokenSource.Cancel();

            var numberTwo = numberOne.ContinueWith(x =>
            {
                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine("Task cancelled");
                }
                Console.WriteLine("Creating a task outside of thread pool for parent task.");
            }, TaskContinuationOptions.OnlyOnCanceled);

            Task.WaitAll();
        }
    }
}
