using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PolynomMultiplication
{
    class ResultPolyinom
    {
        private Polynom a;
        private Polynom b;
        private Polynom result;
        private int len;
        private int nrThreads;
        private int aLength;
        private int bLength;
        private List<Tuple<int, int>> threadElems;

        public Polynom Result { get => result; set => result = value; }
        public int Len { get => len; set => len = value; }
        public Polynom A { get => a; set => a = value; }
        public Polynom B { get => b; set => b = value; }
        public int NrThreads { get => nrThreads; set => nrThreads = value; }
        public int ALength { get => aLength; set => aLength = value; }
        public int BLength { get => bLength; set => bLength = value; }

        public ResultPolyinom(Polynom a, Polynom b, int nrThr = 0)
        {
            A = a;
            B = b;
            ALength = a.Len;
            BLength = b.Len;
            Len = ALength + BLength - 1;
            NrThreads = nrThr;
            List<int> r = new List<int>();
            for (int i = 0; i < len; i++)
                r.Add(0);
            Result = new Polynom(r);
        }

        public void printPolyinom()
        {
            for (int i = 0; i < Len; i++)
            {
                Console.Write(Result.Pol[i]);
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
        public Polynom multiplySeq()
        {
            List<int> newRes = new List<int>();
            for (int i = 0; i < Len; i++)
                newRes.Add(0); 

            
            for (int i = 0; i < ALength; i++)
                for (int j = 0; j < BLength; j++)
                    newRes[i + j]= newRes[i+j] + (A.Pol[i] * B.Pol[j]);
            return new Polynom(newRes);
        }
        public void multipySeqWithThreads(object tuple)
        {
            Tuple<int, int> elNumberAndPos = (Tuple<int, int>)tuple;
            for (int i = elNumberAndPos.Item2; i < elNumberAndPos.Item2+ elNumberAndPos.Item1; i++)
                for (int j = 0; j < BLength; j++)
                    Result.Pol[i + j] = Result.Pol[i + j] + (A.Pol[i] * B.Pol[j]);
        }
        public List<Tuple<int,int>> threadElements()
        {
            List<int> numberOfElementsForThreads = new List<int>();
            List<Tuple<int, int>> numberOfElemsAndStartPosition = new List<Tuple<int, int>>();

            var _whole = ALength / NrThreads;
            var _left = ALength % NrThreads;


            for (int i = 0; i < NrThreads; i++)
            {
                numberOfElementsForThreads.Add(_whole);
                if (_left > 0)
                {
                    numberOfElementsForThreads[i] += 1;
                    _left--;
                }
            }
            int pos = 0;
            foreach( int elNr in numberOfElementsForThreads)
            {
                numberOfElemsAndStartPosition.Add(new Tuple<int, int>(elNr, pos));
                pos = elNr;
            }

            return numberOfElemsAndStartPosition;
        }
        public static Polynom multiplyKaratsubaSeq(Polynom A, Polynom B)
        {
            ResultPolyinom re = new ResultPolyinom(A, B);
            if (A.Degree < 2 || B.Degree < 2)
            {
                return re.multiplySeq();
            }

            int len = Math.Max(A.Degree, B.Degree) / 2;
            Polynom lowP1 = new Polynom(A.Pol.GetRange(0, len));
            Polynom highP1 = new Polynom(A.Pol.GetRange(len, A.Degree));
            Polynom lowP2 = new Polynom(B.Pol.GetRange(0, len));
            Polynom highP2 = new Polynom(B.Pol.GetRange(len, B.Degree));

            Polynom z1 = multiplyKaratsubaSeq(lowP1, lowP2);
            Polynom z2 = multiplyKaratsubaSeq(add(lowP1, highP1), add(lowP2, highP2));
            Polynom z3 = multiplyKaratsubaSeq(highP1, highP2);

            //calculate the final result
            Polynom r1 = shift(z3, 2 * len);
            Polynom r2 = shift(subtract(subtract(z2, z3), z1), len);
            Polynom result = add(add(r1, r2), z1);
            return result;
        }

        public static async Task<Polynom> multiplicationKaratsubaParallelizedForm(Polynom A, Polynom B)
        {
            //E impartit deja de 4 ori si pentru ca e recursiv, nu mai împarțim in mai mult pt ca nu încape pe stack-ul
            // intern
            ResultPolyinom re = new ResultPolyinom(A, B);
            if (A.Degree < 2 || B.Degree < 2)
            {
                return re.multiplySeq();
            }

            int len = Math.Max(A.Degree, B.Degree) / 2;
            Polynom lowP1 = new Polynom(A.Pol.GetRange(0, len));
            Polynom highP1 = new Polynom(A.Pol.GetRange(len, A.Degree));
            Polynom lowP2 = new Polynom(B.Pol.GetRange(0, len));
            Polynom highP2 = new Polynom(B.Pol.GetRange(len, B.Degree));

            var f1 = Task.Run(() => multiplicationKaratsubaParallelizedForm(lowP1, lowP2));
            var f2 = Task.Run(() => multiplicationKaratsubaParallelizedForm(add(lowP1, highP1), add(lowP2, highP2)));
            var f3 = Task.Run(() => multiplicationKaratsubaParallelizedForm(lowP1, lowP2));


            Polynom z1 = f1.Result;
            Polynom z2 = f2.Result;
            Polynom z3 = f3.Result;

            Polynom r1 = shift(z3, 2 * len);
            Polynom r2 = shift(subtract(subtract(z2, z3), z1), len);
            Polynom result = add(add(r1, r2), z1);

            return result;
        }

        public static Polynom add(Polynom A, Polynom B)
        {
            int minDegree = Math.Min(A.Degree, B.Degree);
            int maxDegree = Math.Max(A.Degree, B.Degree);
            List<int> polinom = new List<int>(maxDegree + 1);

            //Add the 2 polynomials
            for (int i = 0; i <= minDegree; i++)
            {
                polinom.Add(A.Pol[i] + B.Pol[i]);
            }

            addRemainingCoefficients(A, B, minDegree, maxDegree, polinom);

            return new Polynom(polinom);
        }
        public static Polynom shift(Polynom A, int offset)
        {
            List<int> polinom = new List<int>();
            for (int i = 0; i < offset; i++)
            {
                polinom.Add(0);
            }
            for (int i = 0; i < A.Len; i++)
            {
                polinom.Add(A.Pol[i]);
            }
            return new Polynom(polinom);
        }

        private static void addRemainingCoefficients(Polynom A, Polynom B, int minDegree, int maxDegree,
                                                 List<int> coefficients)
        {
            if (minDegree != maxDegree)
            {
                if (maxDegree == A.Degree)
                {
                    for (int i = minDegree + 1; i <= maxDegree; i++)
                    {
                        coefficients.Add(A.Pol[i]);
                    }
                }
                else
                {
                    for (int i = minDegree + 1; i <= maxDegree; i++)
                    {
                        coefficients.Add(B.Pol[i]);
                    }
                }
            }
        }
        public static Polynom subtract(Polynom A, Polynom B)
        {
            int minDegree = Math.Min(A.Degree, B.Degree);
            int maxDegree = Math.Max(A.Degree, B.Degree);
            List<int> polinom = new List<int>(maxDegree + 1);

            //Subtract the 2 polynomials
            for (int i = 0; i <= minDegree; i++)
            {
                polinom.Add(A.Pol[i] - B.Pol[i]);
            }

            addRemainingCoefficients(A, B, minDegree, maxDegree, polinom);

            //remove coefficients starting from biggest power if coefficient is 0

            int k = polinom.Count - 1;
            while (polinom[k] == 0 && k > 0)
            {
                polinom.RemoveAt(k);
                k--;
            }

            return new Polynom(polinom);
        }
    }
}
