using System;
using System.Collections.Generic;
using System.Text;

namespace PolynomMultiplication
{
    public class Polynom
    {
        private List<int> pol;
        int len;
        int degree;

        public List<int> Pol { get => pol; set => pol = value; }
        public int Len { get => len; set => len = value; }
        public int Degree { get => degree; set => degree = value; }

        public Polynom(List<int> p)
        {
            Pol = p;
            Len = p.Count;
            Degree = p.Count - 1;
        }
        public void printPolyinom()
        {
            for (int i = 0; i < Len; i++)
            {
                Console.Write(Pol[i]);
                if (i != 0)
                {
                    Console.Write("x^" + i);
                }
                if (i != Len - 1)
                {
                    Console.Write(" + ");
                }
            }
        }

    }
}
