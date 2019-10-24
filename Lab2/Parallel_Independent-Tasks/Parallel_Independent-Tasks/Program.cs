using System;
using System.Collections.Generic;
using System.Threading;

namespace Parallel_Independent_Tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix _first = new Matrix(3,3);
            Matrix _second = new Matrix(3,3);
            int _threadsNr = 2;

            for (int i = 0; i < _first.Row * _first.Column; i++)
                _first.ListElements[i]=i+1;

            for (int i = 0; i < _second.Row * _second.Column; i++)
                _second.ListElements[i]=i+1;

            Console.WriteLine(_first.ToString());
            Console.WriteLine(_second.ToString());

            MatrixSum _matrixSum = new MatrixSum(_first, _second, _threadsNr);

            //Dictionary<int, Dictionary<int, int>> - thread nr, <original pos, value>
            var _firstMatrixSplitByThreads = _matrixSum.ThreadElements(_first);
            var _secondMatrixSplitByThreads = _matrixSum.ThreadElements(_second);

            List<Thread> _threads = new List<Thread>();
            foreach (int k in _firstMatrixSplitByThreads.Keys)
            {
                //Thread tid = new Thread(new ThreadStart(() =>
                //_matrixSum.AddMatrices(_firstMatrixSplitByThreads[k], _secondMatrixSplitByThreads[k])));
                Thread tid = new Thread(new ThreadStart(() =>
               _matrixSum.MultiplyMatrices(_firstMatrixSplitByThreads[k], _secondMatrixSplitByThreads[k])));
                tid.Start();
                tid.Join();
            }


            Console.WriteLine(_matrixSum.Result.ToString());
            Console.ReadLine();
        }
    }
}
