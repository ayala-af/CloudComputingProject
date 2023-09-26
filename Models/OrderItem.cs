namespace CloudComputingProject.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
       public int ProductId { get; set; }
       public string Flavors { get; set; }  
       //One order has many OrderItems
       public int OrderId { get; set; }

    }

}
