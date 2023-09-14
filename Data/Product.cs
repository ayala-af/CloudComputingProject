using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudComputingProject.Data
{
    [Table("Product")]
    public class Product
    {
        public  int Id { get; set; }
        [Required]
        public  string  Name { get; set; }
    }
}
