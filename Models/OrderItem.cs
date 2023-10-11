using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CloudComputingProject.Models
{
    /// <summary>
    /// This model used when the client want to add item for his order
    /// </summary>
    public class OrderItem
    {
		
		public int Id { get; set; }
        /// <summary>
        /// What product is it 
        /// example: 1kg frozen yogurt
        /// </summary>
       public int ProductId { get; set; }
        /// <summary>
        /// The flavors selected for the product
        /// this string consist  "FlavorId,FlavorId,FlavorId..." see the comment ***
        /// example: 123,456,999
        /// </summary>
        public string Flavors { get; set; }  
       //One order has many OrderItems
       public int OrderId { get; set; }
        public string UserId { get; set; } = "000";
        public double Price { get; set; }
    }
    /*
    ***Due to many problems working with a list type property with the
    sql server db, It was decided to model a list by a string of keys when 
    we use the fuction split and join when we serialize and deserialize this object
     */

}
