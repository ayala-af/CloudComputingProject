namespace CloudComputingProject.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        public int Humidity { get; set; }

        public DateTime OrderDate { get; set; }

        public double Temperature { get; set; }

       
    }
}
