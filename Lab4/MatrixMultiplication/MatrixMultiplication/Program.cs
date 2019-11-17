using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MatrixMultiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix _first = new Matrix(2, 2);
            Matrix _second = new Matrix(2, 3);
            Matrix _third = new Matrix(3, 2);

            Console.Write("Enter number of threads: ");
            int _nrThreads = Convert.ToInt32(Console.ReadLine());

            var _matrixOpersations = new MatrixOperations(_first, _second, _third, _nrThreads);
            _matrixOpersations.PopulateMatrices();

            var _intermediateMatrixSplitByThreads = _matrixOpersations.ThreadElements(_matrixOpersations.Intermediar);
            var _resultMatrixSplitByThreads = _matrixOpersations.ThreadElements(_matrixOpersations.Result);

            //int _matrixElementCount = _resultMatrixSplitByThreads.Keys.Count;

            var _intermediarThreads = new List<Task>();
            var _resultThreads = new List<Task>();
            foreach (int k in _intermediateMatrixSplitByThreads.Keys)
            {
                _intermediarThreads.Add(new Task(() => { _matrixOpersations.MultiplyMatrices(_intermediateMatrixSplitByThreads[k]); }));
            }
            foreach (int k in _resultMatrixSplitByThreads.Keys)
            {
                _resultThreads.Add(new Task(() => { _matrixOpersations.MultipyFinalMatrix(_resultMatrixSplitByThreads[k], _matrixOpersations.Result); }));
            }

            foreach (var t in _intermediarThreads)
                t.Start();
            foreach (var t in _resultThreads)
                t.Start();




            //Console.WriteLine(_first.ToString());
            //Console.WriteLine(_second.ToString());
            //Console.WriteLine(_matrixOpersations.IntermediarTaskList.ToString());
            Console.ReadLine();
        }
    }
}
