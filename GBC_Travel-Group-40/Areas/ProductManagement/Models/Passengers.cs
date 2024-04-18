using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBC_Travel_Group_40.Areas.ProductManagement.Models
{
    public class Passengers
    {
        [Key]
        public int PassengerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address {  get; set; }
        [DataType(DataType.Date)]
        public DateTime Birth { get; set; }
        [Required]
        public string Citizen {  get; set; }
        [Required]
        public string Passport {  get; set; }
        public string Gender { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
    }
}
