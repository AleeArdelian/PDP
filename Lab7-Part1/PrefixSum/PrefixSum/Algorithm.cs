using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PrefixSum
{
    public class Algorithm
    {
        private int[] prefixSum;
        private int[] result;
        private int len;

        public Algorithm(int[] arr)
        {
            prefixSum = arr;
            len = arr.Length;
            result = new int[len];
        }

        public void up()
        {
            for (int d = 0; d < Math.Log(len) / Math.Log(2); d++)
            {
                int taskNumber = (int)(len / Math.Pow(2, d + 1));

                using (ManualResetEvent resetEvent = new ManualResetEvent(false))
                {
                    for (int i = 0; i < len; i += (int)Math.Pow(2, d + 1))
                    {
                        int I = i;
                        int D = d;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
                        {
                            prefixSum[(int)(I + Math.Pow(2, D + 1) - 1)] =
                            prefixSum[(int)(I + Math.Pow(2, D) - 1)] + prefixSum[(int)(I + Math.Pow(2, D + 1) - 1)];
                            if (Interlocked.Decrement(ref taskNumber) == 0)
                                resetEvent.Set();
                        }));
                    }
                    resetEvent.WaitOne();
                }

            }
        }

        public void down()
        {

            int last = prefixSum[len - 1];
            prefixSum[len - 1] = 0;

            for (int d = (int)(Math.Log(len) / Math.Log(2))-1 ; d >=0 ; d--)
            { 
                int taskNumber = (int)(len / Math.Pow(2, d + 1));

                using (ManualResetEvent resetEvent = new ManualResetEvent(false))
                {
                    for (int i = 0; i < len; i += (int)Math.Pow(2, d + 1))
                    {
                        int I = i;
                        int D = d;
                        ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
                        {
                            int temp = prefixSum[(int)(I + Math.Pow(2, D)) - 1];
                            prefixSum[(int)(I + Math.Pow(2, D)) - 1] = prefixSum[(int)(I + Math.Pow(2, D + 1)) - 1];
                            prefixSum[(int)(I + Math.Pow(2, D + 1)) - 1] += temp;
                            if (Interlocked.Decrement(ref taskNumber) == 0)
                                resetEvent.Set();
                        }));
                    }
                    resetEvent.WaitOne();
                }
            }
            for (int j = 0; j < result.Length - 1; j++)
            {
                result[j] = prefixSum[j + 1];
            }
            result[result.Length - 1] = last;
        }

        public void printInitialArray()
        {
            foreach (var x in prefixSum)
                Console.Write(x + "  ");
            Console.WriteLine();
        }

        public void printResultArray()
        {
            foreach (var x in result)
                Console.Write(x + "  ");
            Console.WriteLine();
        }
    }
}
