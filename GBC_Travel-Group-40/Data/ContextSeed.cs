using GBC_Travel_Group_40.Areas.ProductManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace GBC_Travel_Group_40.Data
{
    public class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enum.Roles.Customer.ToString()));
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default super User
            var superUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "adminsupport@gmail.com",
                FirstName = "Super",
                LastName = "Admin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
            };

            // check if the super user does not already exixt in database
            if (userManager.Users.All(u => u.Id != superUser.Id))
            {
                // attempt to find the super user
                var user = await userManager.FindByEmailAsync(superUser.Email);
                // if the super user does not exist, proceed creation
                if (user == null)
                {
                    // create the super user account with specified password 
                    await userManager.CreateAsync(superUser, "Pa$$word123");
                    // assign the super user with all the folloeing roles
                    await userManager.AddToRoleAsync(superUser, Enum.Roles.Customer.ToString());
                    await userManager.AddToRoleAsync(superUser, Enum.Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(superUser, Enum.Roles.SuperAdmin.ToString());
                }
            }
        }
    }
}
