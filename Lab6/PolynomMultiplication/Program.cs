using System;

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
            resultPol.multiply();



            foreach (Tuple<int, int> tup in resultPol.threadElements())
                Console.WriteLine(tup.Item1 + " " + tup.Item2 + "/n");




            //Console.WriteLine("First polynomial");
            //printPoly(A, A.Length);
            //Console.WriteLine("\nSecond polynomial");
            //printPoly(B, B.Length);
            //Console.WriteLine("\nProduct polynomial ");
            //printPoly(resultPol.Result, resultPol.Len);
        }
    }
}
