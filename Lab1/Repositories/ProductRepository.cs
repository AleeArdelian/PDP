using Non_cooperative_threads.Model;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Non_cooperative_threads.Repositories
{
    public class ProductRepository
    {
        private List<string> _productNames;
        private List<Product> _storeProducts;
        private List<Product> _originalStoreProducts;
        
        public ProductRepository()
        {
            _storeProducts = new List<Product>();
            _originalStoreProducts = new List<Product>();
            _productNames = new List<string>() { "Milk", "Chicken wings", "Butter", "Soda", "Cheese", "Water" };
        }
        public List<Product> StoreProducts
        {
            get => _storeProducts;
            private set { _storeProducts = value;}
        }
        public List<string> ProductNames 
        {
            get => _productNames;
            set => _productNames = value;
        }
        public List<Product> OriginalStoreProducts
        { 
            get => _originalStoreProducts;
            private set => _originalStoreProducts = value; 
        }

        public void AddProduct(Product p)
        {
            _storeProducts.Add(p);
            Product newProd = new Product();
            newProd.Name = p.Name;
            newProd.Price = p.Price;
            newProd.Quantity = p.Quantity;
            _originalStoreProducts.Add(newProd);
        }
        public int GetPriceByName(string _name)
        {
            var _product = _storeProducts.Find(x => x.Name == _name);
            return _product.Price;
        }
        public Product GetProductByName(string _name)
        {
            return _storeProducts.Find(x => x.Name == _name);
        }
       
        public void BuyBill(Bill _bill, Mutex _mutex)
        {
            foreach ( var _prod in _bill.SoldProducts)
            {
                var _bought = _storeProducts.Find(x => x.Name == _prod.Name);
                QuantityStatus status = _bought.CheckQuantity(_prod.Quantity);

                if (status == QuantityStatus.Empty)
                {
                    _mutex.WaitOne();
                    _prod.Quantity = 0;
                    _prod.Price = 0;
                    _bill.TotalPrice += 0;
                    Console.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " cannot buy " + _prod.Name + ". The quantity is 0");
                    _mutex.ReleaseMutex();
                }
                else
                {
                    if (status == QuantityStatus.NotEnoughPieces)
                    {
                        _mutex.WaitOne();

                        _prod.Quantity = _bought.Quantity;
                        _bought.Quantity = 0;
                        _prod.Price = _bought.Price * _prod.Quantity;
                        _bill.TotalPrice += _prod.Price;
                        Console.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " bought " + _prod.Name + " of quantity " + _prod.Quantity + "\n");

                        _mutex.ReleaseMutex();
                    }
                    else
                    {
                        _mutex.WaitOne();

                        _bought.Quantity -= _prod.Quantity;
                        _bill.TotalPrice += _prod.Price;
                        Console.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " bought " + _prod.Name + " of quantity " + _prod.Quantity + "\n");

                        _mutex.ReleaseMutex();
                    }
                }
            }
        }
    }
}
