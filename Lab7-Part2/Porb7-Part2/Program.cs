using System;
using System.Text;

namespace Porb7_Part2
{
    class Program
    {
        public static string[] generateBigNumberArray(int powerOfTwo)
        {
            Random random = new Random();
            //compute the total number of elements from the list of big numbers
            int noOfElements = (int)Math.Pow(2, powerOfTwo);
            string[] generatedS = new string[noOfElements];
            for (int j = 0; j < noOfElements; j++)
            {
                StringBuilder bigNumber = new StringBuilder();
                //choose a random size of the big number between 5 and 15
                int bigNoLength = random.Next(16) + 5;
                //generate the digits of the big number
                for (int i = 0; i < bigNoLength; i++)
                {
                    bigNumber.Append(random.Next(10));
                }
                generatedS[j] = bigNumber.ToString();
            }
            return generatedS;
        }

        public static void printArray(string[] s)
        {
            StringBuilder str = new StringBuilder("s: ");
            for (int i = 0; i < s.Length; i++)
            {
                str.Append(s[i]).Append(" ");
            }
            Console.WriteLine(str.ToString());
        }

        static void Main(string[] args)
        {
            string[] s = generateBigNumberArray(4);
            Algorithm p2 = new Algorithm(s);
            Console.WriteLine("Initial list: ");
            printArray(s);

            Console.WriteLine("Final sum: ");
            p2.run();

        }
    }
}
