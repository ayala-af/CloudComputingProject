using CloudComputingProject.Constants;
using System.ComponentModel.DataAnnotations;

namespace CloudComputingProject.Models
{
    /// <summary>
    /// This model describes details of flavor 
    /// </summary>
    public class Flavor
    {
        public int Id { get; set; }
        public string FlavorName { get; set; }
        public Category Category { get; set; }
        public bool IsAvailable { get; set; }
        public string? FlavorUrl { get; set; }
    }
}

