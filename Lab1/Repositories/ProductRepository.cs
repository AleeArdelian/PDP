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
        public Product BuyProduct(string pName, int quant, Mutex _mutex)
        {
            var _bought = _storeProducts.Find(x => x.Name == pName);
            var _billProduct = new Product();
            _billProduct.Name = pName;

            _mutex.WaitOne();
            QuantityStatus status = _bought.CheckQuantity(quant);
            _mutex.ReleaseMutex();

            if (status == QuantityStatus.Empty)
            {
                _mutex.WaitOne();
                
                _billProduct.Quantity = 0;
                _billProduct.Price = 0;

                _mutex.ReleaseMutex();
                return _billProduct;
            }

            if( status == QuantityStatus.NotEnoughPieces)
            {
                _mutex.WaitOne();

                _billProduct.Quantity = _bought.Quantity;
                _billProduct.Price = _bought.Quantity * _bought.Price;
                _bought.Quantity = 0;

                _mutex.ReleaseMutex();
                return _billProduct;
            }

            else
            {
                _mutex.WaitOne();

                _billProduct.Quantity = quant;
                _billProduct.Price = quant * _bought.Price;
                _bought.Quantity -= quant;

                _mutex.ReleaseMutex();
                return _billProduct;
            }  
        }
    }
}
