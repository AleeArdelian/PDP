namespace Non_cooperative_threads.Model
{
    public enum QuantityStatus
    {
        Empty,
        NotEnoughPieces,
        Not_Empty
    }
    public class Product
    {
        private int _price;
        private int _quantity;
        private string _name;

        public string Name
        {
            get => _name;
            set { _name = value;}
        }
        public int Price
        {
            get => _price;
            set { _price = value; }
        }
        public int Quantity
        {
            get => _quantity;
           set { _quantity = value; }
        }
        public override string ToString()
        {
            return Name + " Quantity left: " + Quantity + " Price: " + Price + "\n";
        }

        public QuantityStatus CheckQuantity( int pieces)
        {
            if (Quantity == 0)
                return QuantityStatus.Empty;
            if (Quantity < pieces)
                return QuantityStatus.NotEnoughPieces;

            return QuantityStatus.Not_Empty;

        }
    }
}
