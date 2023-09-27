using CloudComputingProject.Constants;


namespace CloudComputingProject.Models
{
    public enum Categoty
    {
        IceCream,
        Frozen,
        Other
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Categoty  Categoty { get; set; }
        public string? Url { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }

    }
}
