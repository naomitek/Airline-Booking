using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_40.Areas.ProductManagement.Models
{
    public class RoomBookings
    {
        [Key]
        public int RoomBookingId { get; set; }
        public int RoomId { get; set; }
        [Required]
        public string NameOfHolder { get; set; }
        [Required]
        public string PhoneOfHolder { get; set; }
        // user id fk
        public string UserId { get; set; }
    }
}
