using System;
using System.Collections.Generic;
using System.Threading;

namespace PrefixSum
{
    class Program
    {         

        static void Main(string[] args)
        {
            Console.WriteLine("Give array lenght, power of 2: ");
            int size = Convert.ToInt32(Console.ReadLine());
            int[] prefixSum = new int[size];

            for (int i = 0; i < size; i++)
                prefixSum[i]=i + 1;
            Algorithm alg = new Algorithm(prefixSum);
            alg.printInitialArray();
            alg.up();
            alg.down();
            alg.printResultArray();

        }
    }
}
