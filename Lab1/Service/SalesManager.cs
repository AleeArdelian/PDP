using Non_cooperative_threads.Model;
using Non_cooperative_threads.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Non_cooperative_threads.Service
{
    public class SalesManager
    {
        private BillRepository _billRepository;
        private ProductRepository _productRepository;

        public BillRepository BillRepository
        {
            get => _billRepository;
            set
            {
                _billRepository = value;
            }
        }
        public ProductRepository ProductRepository
        {
            get => _productRepository;
            set
            {
                _productRepository = value;
            }
        }

        public SalesManager()
        {
            _billRepository = new BillRepository();
            _productRepository = new ProductRepository();
        }
        public void PopulateRandom()
        {
            Random r = new Random();
            for (int i=0; i< ProductRepository.ProductNames.Count; i++)
            {  
                var price = r.Next(5, 50);
                var quant = r.Next(10, 100);
                Product p = new Product();
                p.Name = ProductRepository.ProductNames[i];
                p.Price = price;
                p.Quantity = quant;
                _productRepository.AddProduct(p);
                Console.WriteLine(p);
            }
        }
        public void PrintProductRepo()
        {
            foreach (Product p in ProductRepository.StoreProducts)
                Console.WriteLine(p);
        }
        public Dictionary<string, int> ShoppingList()
        {
            var _shoppingList = new Dictionary<string, int>();
            
            Random r = new Random(Thread.CurrentThread.ManagedThreadId);
            var _listCount = r.Next(1, 5);
            for (int i = 0; i < _listCount; i++)
            {
                var _index = r.Next(ProductRepository.ProductNames.Count);
                var _productName = ProductRepository.ProductNames[_index];
                var _quant = r.Next(1,10);

                if (_shoppingList.ContainsKey(_productName))
                    _shoppingList[_productName] += _quant;
                else
                    _shoppingList.Add(_productName, _quant);
            }
            return _shoppingList;
        }
        public void BuyProducts(Mutex _mutex)
        {
            Bill _bill = new Bill();

            Dictionary<string, int> _shoppingList = ShoppingList();
            Console.WriteLine("Thread: " + Thread.CurrentThread.ManagedThreadId + " created a shopping list \n");

            foreach (var _product in _shoppingList)
            {
                var _boughtProduct = _productRepository.BuyProduct(_product.Key, _product.Value, _mutex);
                if (_boughtProduct.Quantity == 0)
                    Console.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " cannot buy " + _boughtProduct.Name + ". The quantity is 0");
                else
                {
                    Console.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " bought " + _boughtProduct.Name + " of quantity " + _boughtProduct.Quantity + "\n");
                    
                    _mutex.WaitOne();
                    _bill.AddProduct(_boughtProduct);
                    _bill.TotalPrice += _boughtProduct.Price;
                    _mutex.ReleaseMutex();
                }
            }
            
            _mutex.WaitOne();
            BillRepository.TotalBills.Add(_bill);
            BillRepository.TotalPrice += _bill.TotalPrice;

            _mutex.ReleaseMutex();
        }

        public void Check(Mutex _mutex)
        {
            Console.WriteLine("-----------IN CHECK---------------");
            _mutex.WaitOne();
            var _originalStoreProducts = _productRepository.OriginalStoreProducts;
            var _updatedStore = _productRepository.StoreProducts;

            var _allSoldProductsOnBills = BillRepository.AllUniqueProductsSoldInEntireRepository();
            foreach(var _product in _allSoldProductsOnBills)
            {
                var originalP = _originalStoreProducts.Find(x => x.Name == _product.Name);
                var updatedP = _updatedStore.Find(x => x.Name == _product.Name);
                if (!(updatedP.Quantity == originalP.Quantity - _product.Quantity))
                {
                    Console.WriteLine("NORMAL CA NU MERGE FMM");
                    _mutex.ReleaseMutex();
                    return;

                }
            }       
            Console.WriteLine("ASA CEVA NU SE POATE");
            _mutex.ReleaseMutex();
        }
    }
}
