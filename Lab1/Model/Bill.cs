using System.Collections.Generic;
using System.Threading;

namespace Non_cooperative_threads.Model
{
    public class Bill
    {
        private List<Product> _soldProducts;
        private int _price;

        public Bill()
        {
            _soldProducts = new List<Product>();
            _price = 0;
        }
        public List<Product> SoldProducts
        {
            get => _soldProducts;
            set { _soldProducts = value; }
        }

        public int TotalPrice
        {
            get => _price;
            set { _price = value;}
        }

        public override string ToString()
        {
           string product = "";
           foreach (var b in _soldProducts)
           {
              product += "Product: " + b.Name + ", quantity " + b.Quantity + ", total price " + b.Price + "\n";
           }
           product += "Total bill price: " + _price;
           return product; 
        }

        public void AddProduct(Product p)
        {
            _soldProducts.Add(p);
        }
    }
}
