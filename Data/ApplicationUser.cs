using Microsoft.AspNetCore.Identity;

namespace CloudComputingProject.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string? FirstName  { get; set; }
        public string? LastName { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public int? HouseNumber { get; set; } 
    }
}
