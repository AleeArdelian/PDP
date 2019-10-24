using System;
using System.Collections.Generic;
using System.Text;

namespace Parallel_Independent_Tasks
{
    public class Matrix
    {
        private int _row;
        private int _column;
        private int[] _listElements;

        public Matrix(int r, int c)
        {
            _row = r;
            _column = c;
            _listElements = new int[_row * _column];
        }
        public int Row { get => _row; set => _row = value; }
        public int Column { get => _column; set => _column = value; }

        public int[] ListElements { get => _listElements; set => _listElements = value; }
        public List<Tuple<int,int>> GetElementsBetweenIndexes(int a, int b)
        {
            List<Tuple<int, int>> _elements = new List<Tuple<int, int>>();
            for (int i = a; i < b; i++)
                _elements.Add(new Tuple<int,int>(i/_column, i%_column));
            return _elements;
        }
        public string GetElementsToString()
        {
            string x = "";
            foreach (int el in ListElements)
                x += el + " ";
            return x;
        }
        public override string ToString()
        {
            return "Matrix: " + GetElementsToString();
        }

        public int GetByPositions(int i, int j)
        {
            return _listElements[_column * i + j];
        }

        public void PutByPositions(int i, int j, int value)
        {
            var pos = _column * i + j;
            _listElements[pos] =value;
        }
    }
}
