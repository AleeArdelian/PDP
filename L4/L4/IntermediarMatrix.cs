using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4
{
    public class IntermediarMatrix
    {
        private int _row;
        private int _column;
        private Task<int>[] _listElements;

        public IntermediarMatrix(int r, int c)
        {
            _row = r;
            _column = c;
            _listElements = new Task<int>[_row * _column];
        }

        public int Row { get => _row; set => _row = value; }
        public int Column { get => _column; set => _column = value; }
        public Task<int>[] ListElements { get => _listElements; set => _listElements = value; }

        public List<Tuple<int, int>> GetElementsBetweenIndexes(int a, int b)
        {
            List<Tuple<int, int>> _elements = new List<Tuple<int, int>>();
            for (int i = a; i < b; i++)
                _elements.Add(new Tuple<int, int>(i / Column, i % Column));
            return _elements;
        }
        public string GetElementsToString()
        {
            string x = "";
            foreach (var el in ListElements)
                x += el + " ";
            return x;
        }
        public override string ToString()
        {
            return "Matrix: " + GetElementsToString();
        }

        public Task<int> GetByPositions(int i, int j)
        {
            return ListElements[Column * i + j];
        }

        public void PutByPositions(int i, int j, Task<int> value)
        {
            var pos = Column * i + j;
            ListElements[pos] = value;
        }
    }
}
