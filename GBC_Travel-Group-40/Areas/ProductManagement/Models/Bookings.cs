using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBC_Travel_Group_40.Areas.ProductManagement.Models
{
    public class Bookings
    {
        [Key]
        public int BookingId { get; set; }
        public int? UserId{  get; set; }
        public BookingFlights? BookingFlight { get; set; }
        //public BookingHotels?[] HotelBookings { get; set; }
        //public CarRentals?[] CarRentals { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
        [Column(TypeName ="datetime2")]
        public DateTime BookingDate { get; set; }
    }
}
