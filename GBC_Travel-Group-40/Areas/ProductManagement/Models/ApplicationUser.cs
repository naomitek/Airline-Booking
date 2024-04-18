using Microsoft.AspNetCore.Identity;

namespace GBC_Travel_Group_40.Areas.ProductManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public int UsernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }
    }
}