using System;
using System.Collections.Generic;
using System.Threading;

namespace PolynomMultiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> P1 = new List<int>(){ 1, 2, 3 }; // polynomial 5 + 10x^2 + 6x^3 
            List<int> P2 = new List<int>() { 1, 2, 3,4 };  // polynomial 1 + 2x + 4x^2 
            Polynom A = new Polynom(P1);
            Polynom B = new Polynom(P2);      

            ResultPolyinom polyOper = new ResultPolyinom(A, B, 2);

            //Sequencial 
            //Polynom resultPol = polyOper.multiplySeq();

            //Seq with threads
            //using (ManualResetEvent resetEvent = new ManualResetEvent(false))
            //{
            //    List<Tuple<int, int>> threadPos = polyOper.threadElements();
            //    int threadCount = threadPos.Count;
            //    foreach (var tup in threadPos)
            //    {
            //        ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
            //        {
            //            polyOper.multipySeqWithThreads(x);
            //            if (Interlocked.Decrement(ref threadCount) == 0)
            //                resetEvent.Set();
            //        }), tup);
            //    }
            //    resetEvent.WaitOne();
            //}

            //Karatsuba seq
            //Polynom resultPol = ResultPolyinom.multiplyKaratsubaSeq(A,B);

            //Karatsuba threads
            Polynom resultPol = ResultPolyinom.multiplicationKaratsubaParallelizedForm(A,B).Result;


            Console.Write("First polynomial  ");
            A.printPolyinom();
            Console.Write("\nSecond polynomial  ");
            B.printPolyinom();
            Console.Write("\nProduct polynomial  ");
            //polyOper.Result.printPolyinom();
            resultPol.printPolyinom();
        }
    }
}
