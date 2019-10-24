using Non_cooperative_threads.Model;
using System.Collections.Generic;
using System.Threading;

namespace Non_cooperative_threads.Repositories
{
    public class BillRepository
    {
        private List<Bill> _totalBills;
        private int _totalPrice;
        private Mutex _mutex;
        
        public BillRepository()
        {
            _totalBills = new List<Bill>();
            _totalPrice = 0;

        }
        public List<Bill> TotalBills
        {
            get => _totalBills;
            set
            {
                _totalBills = value;
            }
        }
        public int TotalPrice
        {
            get => _totalPrice;
            set
            {
                _totalPrice = value;
            }
        }
        public void AddBill( Bill b)
        {
            _totalBills.Add(b);
            _totalPrice += b.TotalPrice;
        }

        public List<Product> AllUniqueProductsSoldInEntireRepository()
        {
            List<Product> _productsSoldInAllBills = new List<Product>();
            foreach (var _bill in _totalBills)
            {
                foreach (Product p in _bill.SoldProducts)
                {
                    var _found = _productsSoldInAllBills.Find(x => x.Name == p.Name);
                    if (_found != null)
                        _found.Quantity += p.Quantity;
                    else
                        _productsSoldInAllBills.Add(p);
                }
            }
            return _productsSoldInAllBills;
        }
    }
}
