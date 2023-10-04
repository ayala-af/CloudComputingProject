using System.ComponentModel.DataAnnotations.Schema;

namespace CloudComputingProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientFirstName { get; set; }
        public string ClientLastName { get; set; }
       public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public string Email { get; set; }
        public string City { get; set; }

        public string Street { get; set; }
        public int House { get; set; }
       public double FeelsLike { get; set; }
        public int Humidity { get; set; }
        public double Temperature { get; set; }
        public bool IsHoliday { get; set; }
        public DayOfWeek OrderDay { get; set; }
        public double TotalPrice { get; set; }
        [NotMapped]
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    }
}
