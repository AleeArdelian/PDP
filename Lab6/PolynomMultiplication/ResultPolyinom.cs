using System;
using System.Collections.Generic;
using System.Text;

namespace PolynomMultiplication
{
    class ResultPolyinom
    {
        private int[] A;
        private int[] B;
        private int[] result;
        private int len;
        private int nrThreads;
        private int aLength;
        private int bLength;
        private List<Tuple<int, int>> threadElems;

        public int[] Result { get => result; set => result = value; }
        public int Len { get => len; set => len = value; }
        public int[] A1 { get => A; set => A = value; }
        public int[] B1 { get => B; set => B = value; }
        public int NrThreads { get => nrThreads; set => nrThreads = value; }
        public int ALength { get => aLength; set => aLength = value; }
        public int BLength { get => bLength; set => bLength = value; }

        public ResultPolyinom(int[] a, int[] b, int aLen, int bLen, int nrThr = 0)
        {
            A1 = a;
            B1 = b;
            ALength = aLen;
            BLength = bLen;
            Len = aLen + bLen - 1;
            NrThreads = nrThr;
            Result = new int[Len];
        }

        public void multiply()
        {
            for (int i = 0; i < len; i++)
            {
                result[i] = 0;
            }
            for (int i = 0; i < ALength; i++)
            {
                for (int j = 0; j < BLength; j++)
                {
                    result[i + j] += A[i] * B[j];
                }
            }
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
            int pos = 1;
            foreach( int elNr in numberOfElementsForThreads)
            {
                numberOfElemsAndStartPosition.Add(new Tuple<int, int>(elNr, pos-1));
                pos = elNr;
            }

            return numberOfElemsAndStartPosition;
        }

        public void multiplyWithThreads(int nrElem)
        {
            for (int i = 0; i < len; i++)
            {
                result[i] = 0;
            }
            for (int i = 0; i < ALength; i++)
            {
                for (int j = 0; j < BLength; j++)
                {
                    result[i + j] += A[i] * B[j];
                }
            }
        }
    }
}
