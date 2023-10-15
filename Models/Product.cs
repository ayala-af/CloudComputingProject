using CloudComputingProject.Constants;


namespace CloudComputingProject.Models
{

    public enum Category
    {
        IceCream,
        Frozen,
        Other
    }
   
    /// <summary>
    /// This model is for define general product
    ///without actual flavors 
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int MaxFlavorsNumber { get; set; }
        public Category  Category { get; set; }
        public string? Url { get; set; }
        public double Price { get; set; }
        public bool IsAvailable { get; set; }

    }
}
