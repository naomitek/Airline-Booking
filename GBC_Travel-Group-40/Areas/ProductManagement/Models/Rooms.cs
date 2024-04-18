using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBC_Travel_Group_40.Areas.ProductManagement.Models
{
    public class Rooms
    {
        [Key]
        public int RoomId { get; set; }

        [Required]
        public string HotelName { get; set; }

        public string Location { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Check-in Date")]
        public DateTime Checking { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Check-out Date")]
        public DateTime Checkout { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PricePerNight { get; set; }

        public int NumOfGuests { get; set; } = 0;

        public int MaxGuests { get; set; }

        [Required]
        public int NumberOfBeds { get; set; }

        public int Rating { get; set; }

        public bool PetFriendly { get; set; }

        public bool AirConditioning { get; set; }

        public bool Wifi { get; set; }
        public string Url { get; set; }
    


    }
}
