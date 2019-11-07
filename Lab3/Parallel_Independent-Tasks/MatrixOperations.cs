using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Parallel_Independent_Tasks
{
    public class MatrixOperations
    {
        private Matrix _first;
        private Matrix _second;
        private Matrix _result;
        int _nrThreads;

        public MatrixOperations(Matrix f, Matrix s, int nr, int _isSum)
        {
            _first = f;
            _second = s;
            _nrThreads = nr;
            if (_isSum == 1)
                _result = new Matrix(_first.Row, _first.Column);
            else
                _result = new Matrix(_second.Row, _second.Column);
        }

        public Matrix First { get => _first; }
        public Matrix Second { get => _second; }
        public Matrix Result { get => _result; }
        public int NrThreads { get => _nrThreads; }


        public void PopulateMatrices()
        {
            for (int i = 0; i < _first.Row * _first.Column; i++)
                _first.ListElements[i] = i + 1;

            for (int i = 0; i < _second.Row * _second.Column; i++)
                _second.ListElements[i] = i + 1;

            for (int i = 0; i < _result.Row * _result.Column; i++)
                _result.ListElements[i] = 0;
        }
        public List<int> DivideMatrix()
        {
            List<int> _threadElementNr = new List<int>();
            var _matrixCount = Result.Column * Result.Row;
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
            List<int> _nrElementsForThreads = DivideMatrix(); // 2 threads with 5 and 4 elems
            Dictionary<int, List<Tuple<int, int>>> _matrixDividedInThreads = new Dictionary<int, List<Tuple<int, int>>>();
            int index = 0;
            for(int i=0; i<_nrElementsForThreads.Count; i++)
            {
                var _nrElements = _nrElementsForThreads[i]; //nr of elements in thread
                List<Tuple<int,int>> _threadElementsPositions = m.GetElementsBetweenIndexes(index, _nrElements+index);
                _matrixDividedInThreads.Add(i, _threadElementsPositions);
                index = _nrElements+index;
            }
            return _matrixDividedInThreads; // list with index and tuples of positions for the elements in one thread
        }

        public void AddMatrices(Object _resultThreadObj)
        {
            List<Tuple<int, int>> _resultThreadElements = (List<Tuple<int, int>>)_resultThreadObj;
            for (int i=0; i< _resultThreadElements.Count; i++)
            {
                var _valueFirst = _first.GetByPositions(_resultThreadElements[i].Item1, _resultThreadElements[i].Item2);
                var _valueSecond = _second.GetByPositions(_resultThreadElements[i].Item1, _resultThreadElements[i].Item2);
                _result.PutByPositions(_resultThreadElements[i].Item1, _resultThreadElements[i].Item2, _valueFirst+_valueSecond);
            }
        }

        public void MultiplyMatrices(Object _resultThreadObj)
        {
            List<Tuple<int, int>> _resultThreadElements = (List<Tuple<int, int>>)_resultThreadObj;
            foreach (Tuple<int,int> x in _resultThreadElements)
            {
                int sum = 0;
                for ( int i = 0; i< _first.Column; i++)
                {
                    sum += _first.GetByPositions(x.Item1, i) * _second.GetByPositions(i, x.Item2);
                }
                _result.PutByPositions(x.Item1, x.Item2, sum);
            }
        }


        public async Task<bool> MultiplyMatricesAsync(Object _resultThreadObj)
        {
            List<Tuple<int, int>> _resultThreadElements = (List<Tuple<int, int>>)_resultThreadObj;
            foreach (Tuple<int, int> x in _resultThreadElements)
            {
                int sum = 0;
                for (int i = 0; i < _first.Column; i++)
                {
                    sum += _first.GetByPositions(x.Item1, i) * _second.GetByPositions(i, x.Item2);
                }
                _result.PutByPositions(x.Item1, x.Item2, sum);
            }
            return true;
        }
    }
}
