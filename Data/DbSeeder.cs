using CloudComputingProject.Constants;
using Microsoft.AspNetCore.Identity;

namespace CloudComputingProject.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            //Seed Roles
            var userManager = service.GetService<UserManager<ApplicationUser>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();
            await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));


            //creating admin
            var user = new ApplicationUser
            {
                UserName="admin@gmail.com",
                Email="admin@gmail.com",
                Name="Admin",
                EmailConfirmed=true,
                PhoneNumberConfirmed=true,
    
            };
            var userInDb = await userManager.FindByEmailAsync(user.Email);
        if(userInDb==null)
            {
                await userManager.CreateAsync(user,"CHY.64Z,U4@Fv,C");//5tBF5IUpT78$ is the password of the admin
             //   await userManager.PasswordValidators.Insert(user, "5tBF5IUpT78$");
                await userManager.AddToRoleAsync(user,Roles.Admin.ToString()); 
            }
        }

    }
}
