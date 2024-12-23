public class Order
    {
        public int? id { get; set; }
        public string nameCustomer { get; set; }
        public float? count { get; set; }
        public DateTime date { get; set; }

        public Order() 
        { 
            this.nameCustomer = "";
            this.count = 0.0f;
            this.date = DateTime.Today;
        }
    }