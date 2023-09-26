namespace CloudComputingProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public double Price { get; set; }
        
        public DateTime OrderDate { get; set; }
        public string City { get; set; }

        public string Street { get; set; }

        public string House { get; set; }

    }
}
