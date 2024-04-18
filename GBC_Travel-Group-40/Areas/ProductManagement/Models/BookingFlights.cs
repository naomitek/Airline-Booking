using System.ComponentModel.DataAnnotations;

namespace GBC_Travel_Group_40.Areas.ProductManagement.Models

{
    public class BookingFlights
    {
        [Key]
        public int BookingFlightId {  get; set; }
        public int FlightId { get; set; }
        public int PassengerId {  get; set; }
        // user id fk
        public string UserId { get; set; }
    }
}
