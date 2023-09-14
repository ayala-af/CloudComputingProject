using Microsoft.AspNetCore.Identity;

namespace CloudComputingProject.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string? Name  { get; set; }
    }
}
