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
        private object _lock = new object();

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
        public List<Product> ShoppingList()
        {
            var _shoppingList = new List<Product>();
            
            Random r = new Random(Thread.CurrentThread.ManagedThreadId);
            var _listCount = r.Next(1, 5);
            for (int i = 0; i < _listCount; i++)
            {
                Product _product = new Product();
                var _index = r.Next(ProductRepository.ProductNames.Count);
                _product.Name = ProductRepository.ProductNames[_index];
                _product.Quantity = r.Next(1,10);

                var _price = ProductRepository.OriginalStoreProducts.Find(x => x.Name == _product.Name);
                _product.Price = _price.Price * _product.Quantity;

                var _alreadyExists = _shoppingList.Find(x => x.Name == _product.Name);
                if (_alreadyExists != null)
                    _alreadyExists.Quantity += _product.Quantity;
                else
                    _shoppingList.Add(_product);
            }
            return _shoppingList;
        }
        public void BuyProducts(Mutex _mutex)
        {
            Bill _bill = new Bill();
            List<Product> _shoppingList = ShoppingList();

            Console.WriteLine("Thread: " + Thread.CurrentThread.ManagedThreadId + " created a shopping list \n");

            foreach (var _product in _shoppingList)
                    _bill.AddProduct(_product);
            
            var _billToBuy = ProductRepository.CheckBill(_bill, _mutex);
            var _billToAdd = new Bill();
            BillRepository.TotalBills.Add(_billToAdd);

            foreach (var _prod in _billToBuy.SoldProducts)
            {
                var _bought = ProductRepository.StoreProducts.Find(x => x.Name == _prod.Name);
                _mutex.WaitOne();
                _bought.Quantity -= _prod.Quantity;
                _billToAdd.SoldProducts.Add(_prod);
                _billToAdd.TotalPrice += _prod.Price;
                Console.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " bought " + _prod.Name + " of quantity " + _prod.Quantity + "\n");

                _mutex.ReleaseMutex();
            }
        }

        public void Check(Mutex _mutex)
        {
            _mutex.WaitOne();
            Console.WriteLine("-----------IN CHECK---------------");

            var _originalStoreProducts = ProductRepository.OriginalStoreProducts;
            var _updatedStore = ProductRepository.StoreProducts;

            var _allSoldProductsOnBills = BillRepository.AllUniqueProductsSoldInEntireRepository();
            var _totalPrice = 0;
            foreach (var _product in _allSoldProductsOnBills)
            {
                _totalPrice += _product.Price;
                var originalP = _originalStoreProducts.Find(x => x.Name == _product.Name);
                var updatedP = _updatedStore.Find(x => x.Name == _product.Name);
                if (!(updatedP.Quantity == originalP.Quantity - _product.Quantity))
                {
                    if (BillRepository.TotalPrice == _totalPrice)
                    {
                        Console.WriteLine("SOMETHING IS NOT RIGHT :( ");
                        return;
                    }
                }
            }
            Console.WriteLine("YOU DID IT :)");
            _mutex.ReleaseMutex();
        }
    }
}
