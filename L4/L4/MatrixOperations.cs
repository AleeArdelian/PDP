using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4
{
    public class MatrixOperations
    {
        private Matrix _first;
        private Matrix _second;
        private Matrix _intermediar;
        private IntermediarMatrix _intermediarTasksList;

        private Matrix _third;
        private Matrix _result;
        int _nrThreads;


        public MatrixOperations(Matrix f, Matrix s, Matrix t, int nr)
        {
            _first = f;
            _second = s;
            _third = t;
            _intermediar = new Matrix(f.Row, s.Column);
            //the matrix from multiplying f and s will have f.row rows and s.col columns            
            //after multiplying with the third matrix f.row and t.column columns
            _intermediarTasksList = new IntermediarMatrix(f.Row, s.Column);

            _result = new Matrix(f.Row, t.Column);
            _nrThreads = nr;
        }

        public Matrix First { get => _first; }
        public Matrix Second { get => _second; }
        public IntermediarMatrix IntermediarTaskList { get => _intermediarTasksList; }
        public Matrix Third { get => _third; }
        public Matrix Result { get => _result; }
        public int NrThreads { get => _nrThreads; }
        public Matrix Intermediar { get => _intermediar; set => _intermediar = value; }

        public void PopulateMatrices()
        {
            for (int i = 0; i < _first.Row * _first.Column; i++)
                _first.ListElements[i] = i + 1;

            for (int i = 0; i < _second.Row * _second.Column; i++)
                _second.ListElements[i] = i + 1;

            //for (int i = 0; i < _intermediar.Row * _intermediar.Column; i++)
            //    _intermediar.ListElements[i] = 0;

            for (int i = 0; i < _third.Row * _third.Column; i++)
                _third.ListElements[i] = i + 1;

            for (int i = 0; i < _result.Row * _result.Column; i++)
                _result.ListElements[i] = 0;
        }

        public List<int> DivideMatrix(Matrix _matrix)
        {
            List<int> _threadElementNr = new List<int>();
            var _matrixCount = _matrix.Column * _matrix.Row;
            var _whole = _matrixCount / NrThreads;
            var _left = _matrixCount % NrThreads;

            for (int i = 0; i < NrThreads; i++)
            {
                _threadElementNr.Add(_whole);
                if (_left > 0)
                {
                    _threadElementNr[i] += 1;
                    _left--;
                }
            }
            return _threadElementNr;
        }

        public Dictionary<int, List<Tuple<int, int>>> ThreadElements(Matrix m)
        {
            List<int> _nrElementsForThreads = DivideMatrix(m); // 2 threads with 5 and 4 elems
            Dictionary<int, List<Tuple<int, int>>> _matrixDividedInThreads = new Dictionary<int, List<Tuple<int, int>>>();
            int index = 0;
            for (int i = 0; i < _nrElementsForThreads.Count; i++)
            {
                var _nrElements = _nrElementsForThreads[i]; //nr of elements in thread
                List<Tuple<int, int>> _threadElementsPositions = m.GetElementsBetweenIndexes(index, _nrElements + index);
                _matrixDividedInThreads.Add(i, _threadElementsPositions);
                index = _nrElements;
            }
            return _matrixDividedInThreads; // list with index and tuples of positions for the elements in one thread
        }

        public void MultiplyMatrices(List<Tuple<int, int>> _intermediarThreadElements)
        {
            foreach (var x in _intermediarThreadElements)
            {
                _intermediarTasksList.PutByPositions(x.Item1, x.Item2, new Task<int>(() => MultiplyLineWithColumn(x)));

            }
            foreach (var x in _intermediarThreadElements)
            {
                _intermediarTasksList.GetByPositions(x.Item1, x.Item2).Start();
            }
        }

        public int MultiplyLineWithColumn(Tuple<int, int> _resultMatrixPositions)
        {
            int sum = 0;
            for (int i = 0; i < _first.Column; i++)
            {
                sum += _first.GetByPositions(_resultMatrixPositions.Item1, i) * _second.GetByPositions(i, _resultMatrixPositions.Item2);
            }
            //tcs.SetResult(sum);
            return sum;
            //_intermediarTasksList.PutByPositions(_resultMatrixPositions.Item1, _resultMatrixPositions.Item2, tcs.Task);
        }

        public async void MultipyFinalMatrix(List<Tuple<int, int>> _resultThreadElements, Matrix _result)
        {
            int sum = 0;
            foreach (var x in _resultThreadElements)
            {
                for (int i = 0; i < _intermediarTasksList.Column; i++)
                {
                    var a = await _intermediarTasksList.GetByPositions(x.Item1, i);
                    var b = _third.GetByPositions(i, x.Item2);

                    sum += a * b;
                }
                _result.PutByPositions(x.Item1, x.Item2, sum);
            }
        }

    }
}
