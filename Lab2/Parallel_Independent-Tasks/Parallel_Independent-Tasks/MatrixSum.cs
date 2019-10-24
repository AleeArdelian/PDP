using System;
using System.Collections.Generic;
using System.Text;

namespace Parallel_Independent_Tasks
{
    public class MatrixSum
    {
        private Matrix _first;
        private Matrix _second;
        private Matrix _result;
        int _nrThreads;

        public MatrixSum(Matrix f, Matrix s, int nr)
        {
            _first = f;
            _second = s;
            _result = new Matrix(f.Column, f.Row);
            _nrThreads = nr;
        }

        public Matrix First { get => _first; }
        public Matrix Second { get => _second; }
        public Matrix Result { get => _result; }
        public int NrThreads { get => _nrThreads; }

        public List<int> DivideMatrix()
        {
            List<int> _threadElementNr = new List<int>();
            var _matrixCount = First.Column * First.Row;
            var _whole = _matrixCount / NrThreads;
            var _left = _matrixCount % NrThreads;

            for (int i = 0; i < NrThreads; i++)
            {
                _threadElementNr.Add(_whole);
                if (_left > 0)
                {
                    _threadElementNr[i]+=1;
                    _left--;
                }
            }
            return _threadElementNr;
        }

        public Dictionary<int, List<Tuple<int, int>>> ThreadElements(Matrix m)
        {
            List<int> _nrElementsForThreads = DivideMatrix();
            Dictionary<int, List<Tuple<int, int>>> _matrixDividedInThreads = new Dictionary<int, List<Tuple<int, int>>>();
            int index = 0;
            for(int i=0; i<_nrElementsForThreads.Count; i++) //2 threads
            {
                var _nrElements = _nrElementsForThreads[i]; //nr of elements in thread
                List<Tuple<int,int>> _threadElementsPositions = m.GetElementsBetweenIndexes(index, _nrElements+index);
                _matrixDividedInThreads.Add(i, _threadElementsPositions);
                index = _nrElements;
            }
            return _matrixDividedInThreads;
        }

        public void AddMatrices(List<Tuple<int,int>> _firstThreadElements, List<Tuple<int, int>> _secondThreadElements)
        {
            for(int i=0; i< _firstThreadElements.Count; i++)
            {
                var _valueFirst = _first.GetByPositions(_firstThreadElements[i].Item1, _firstThreadElements[i].Item2);
                var _valueSecond = _second.GetByPositions(_secondThreadElements[i].Item1, _secondThreadElements[i].Item2);
                _result.PutByPositions(_firstThreadElements[i].Item1, _firstThreadElements[i].Item2, _valueFirst+_valueSecond);
            }
        }

        public void MultiplyMatrices(List<Tuple<int, int>> _firstThreadElements, List<Tuple<int, int>> _secondThreadElements)
        {
            foreach (Tuple<int,int> x in _firstThreadElements)
            {
                int sum = 0;
                for ( int i = 0; i< _first.Column; i++)
                {
                    sum += _first.GetByPositions(x.Item1, i) * _second.GetByPositions(i, x.Item2);
                }
                _result.PutByPositions(x.Item1, x.Item2, sum);
            }
        }
    }
}
