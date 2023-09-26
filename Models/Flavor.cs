using CloudComputingProject.Constants;
using System.ComponentModel.DataAnnotations;

namespace CloudComputingProject.Models
{

    public class Flavor
    {
        public int Id { get; set; }
        public string FlavorName { get; set; }
      
        public Categoty Categoty { get; set; }
        public bool IsAvailable { get; set; }
        public string? FlavorUrl { get; set; }
    }
}

