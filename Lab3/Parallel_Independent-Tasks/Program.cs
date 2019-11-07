using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Parallel_Independent_Tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            Matrix _first = new Matrix(3,3);
            Matrix _second = new Matrix(3,3);

            Console.Write("Enter number of threads: ");
            int _nrThreads = Convert.ToInt32(Console.ReadLine());

            Console.Write("Choose 0 for multiplication and 1 for addition: ");
            int _isSum = Convert.ToInt32(Console.ReadLine());

            Console.Write("Choose 0 for simple threads, 1 for tasks and 2 for threadpool: ");
            int _threadsType = Convert.ToInt32(Console.ReadLine());

            MatrixOperations _matrixSum = new MatrixOperations(_first, _second, _nrThreads, _isSum);
            _matrixSum.PopulateMatrices();
            var _resultMatrixSplitByThreads = _matrixSum.ThreadElements(_matrixSum.Result);

            int _matrixElementCount = _resultMatrixSplitByThreads.Keys.Count;

            if (_threadsType == 0) //threads
            {
                if (_isSum == 1)
                {
                    foreach (int k in _resultMatrixSplitByThreads.Keys)
                    {
                        Thread tid = new Thread(new ThreadStart(() =>
                        _matrixSum.AddMatrices(_resultMatrixSplitByThreads[k])));
                        tid.Start();
                        tid.Join();
                    }
                }
                else
                {
                    foreach (int k in _resultMatrixSplitByThreads.Keys)
                    {
                        Thread tid = new Thread(new ThreadStart(() =>
                        _matrixSum.MultiplyMatrices(_resultMatrixSplitByThreads[k])));
                        tid.Start();
                        tid.Join();
                    }
                }
                Console.WriteLine(_first.ToString());
                Console.WriteLine(_second.ToString());
                Console.WriteLine(_matrixSum.Result.ToString());
            }
            if(_threadsType == 1) //tasks
            {
                if (_isSum == 1)
                {
                    foreach (int k in _resultMatrixSplitByThreads.Keys)
                    {
                        Task task = new Task(() => { _matrixSum.AddMatrices(_resultMatrixSplitByThreads[k]); });
                        task.Start();
                        task.Wait();
                    }
                }
                else
                {
                    foreach (int k in _resultMatrixSplitByThreads.Keys)
                    {
                        Task<bool> task = _matrixSum.MultiplyMatricesAsync(_resultMatrixSplitByThreads[k]);
                        task.Wait();
                        
                    }
                }
                Console.WriteLine(_first.ToString());
                Console.WriteLine(_second.ToString());
                Console.WriteLine(_matrixSum.Result.ToString());
            }
            if (_threadsType == 2) //threadpool
            {
                using (ManualResetEvent resetEvent = new ManualResetEvent(false))
                {
                    foreach (int k in _resultMatrixSplitByThreads.Keys)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(x =>
                        {
                            if (_isSum == 1)
                                _matrixSum.AddMatrices(x);
                            else
                                _matrixSum.MultiplyMatrices(x);
                            if (Interlocked.Decrement(ref _matrixElementCount) == 0)
                                resetEvent.Set();
                        }), _resultMatrixSplitByThreads[k]);
                    }
                    resetEvent.WaitOne();
                }
                Console.WriteLine(_first.ToString());
                Console.WriteLine(_second.ToString());
                Console.WriteLine(_matrixSum.Result.ToString());
            }
            
            Console.ReadLine();
        }
    }
}
