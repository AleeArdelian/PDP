using System;
using System.Collections.Generic;
using System.Threading;

namespace PolynomMultiplication
{
    class Program
    {
        static void printPoly(int[] poly, int n)
        {
            for (int i = 0; i < n; i++)
            {
                Console.Write(poly[i]);
                if (i != 0)
                {
                    Console.Write("x^" + i);
                }
                if (i != n - 1)
                {
                    Console.Write(" + ");
                }
            }
        }

        static void Main(string[] args)
        {
            int[] A = { 5, 0, 10, 6 }; // polynomial 5 + 10x^2 + 6x^3 
            int[] B = { 1, 2, 4 };  // polynomial 1 + 2x + 4x^2 

            ResultPolyinom resultPol = new ResultPolyinom(A, B, A.Length, B.Length, 2);
            
            //Sequencial
            //resultPol.multiplySeq();

            List<Tuple<int, int>> threadPos = resultPol.threadElements();
            int threadCount = threadPos.Count;
            

            //Seq with threads
            using (ManualResetEvent resetEvent = new ManualResetEvent(false))
            {
                foreach (var tup in threadPos)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
                    {
                        resultPol.multipySeqWithThreads(x);
                        if (Interlocked.Decrement(ref threadCount) == 0)
                            resetEvent.Set();
                    }), tup);
                }
                resetEvent.WaitOne();
            }

            Console.Write("First polynomial  ");
            printPoly(A, A.Length);  
            Console.Write("\nSecond polynomial  ");
            printPoly(B, B.Length);
            Console.Write("\nProduct polynomial  ");
            printPoly(resultPol.Result, resultPol.Len);
        }
    }
}
