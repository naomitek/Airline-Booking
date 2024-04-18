 using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_40.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserEmail {  get; set; }
        public string? UserPhone { get; set; }
        public string? UserCountry {  get; set; }
        public bool IsMember { get; set;}
        public string? Password { get; set; }
        public string? UserIcon {  get; set; }
    }
}
