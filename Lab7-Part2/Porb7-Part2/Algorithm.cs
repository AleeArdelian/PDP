using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Porb7_Part2
{
    public class Algorithm
    {
        private string[] nr;

        public Algorithm(string[] numbers)
        {
            nr = numbers;
        }

        public Queue<int> gueueDigits(String number)
        {
            Queue<int> nr = new Queue<int>();
            for(int i = number.Length-1; i>=0; i--)
            {
                char c = number[i];
                nr.Enqueue((int)Char.GetNumericValue(c));
            }
            return nr;
        }

        public int getNextDigit(Queue<int> queue)
        {
            if (queue.Count ==0)
                return 0;
            else
                return queue.Dequeue();
        }

        public Queue<int> addTwo(Queue<int> left, Queue<int> right)
        {
            Queue<int> result = new Queue<int>();
            int carry = 0;
            while (left.Count !=0 || right.Count != 0)
            {
                int sum = getNextDigit(left) + getNextDigit(right) + carry;
                result.Enqueue(sum % 10);
                carry = sum / 10;
            }
            if (carry != 0)
            {
                result.Enqueue(carry);
            }
            return result;
        }

        public static void printQueueAsNumber(Queue<int> queue)
        {
            StringBuilder sb = new StringBuilder();
            foreach (int i in queue)
            {
                sb.Append(i);
            }

            string convertedString = sb.ToString();
            char[] charArray = sb.ToString().ToCharArray();
            Array.Reverse(charArray);

            string s = new string(charArray);

            Console.WriteLine(s);
        }
        public void run()
        {
            Queue<int>[] queues = new Queue<int>[nr.Length];
            for (int j = 0; j < nr.Length; j++)
            {
                queues[j] = gueueDigits(nr[j]);
            } 

            int totalNumberOfTasks = queues.Length - 1;//(int)(queues.Length / Math.Pow(2, d + 1));

                //using (ManualResetEvent resetEvent = new ManualResetEvent(false))
                //{
                for (int i = 0; i < queues.Length-1; i++)
                {
                    int k = i;
                    var task = Task.Run( () => { 
                        var x = k+1;
                        var y = k;
                        var z = k+1;
                        queues[x] = addTwo(queues[y], queues[z]);
                        //if (Interlocked.Decrement(ref totalNumberOfTasks) == 0)
                        //    resetEvent.Set();
                        });
                task.Wait();
                //    resetEvent.WaitOne();
                }
            Queue<int> result = queues[queues.Length - 1];
            printQueueAsNumber(result);
        } 
    }
}
