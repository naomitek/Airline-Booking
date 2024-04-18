using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBC_Travel_Group_40.Areas.ProductManagement.Models
{
    public class Flights
    {
        [Key]
        public int FlightId { get; set; }
        [Required]
        public string Airlines { get; set; }
        [Required]
        public string Origen { get; set; }
        [Required]
        public string Destination { get; set; }
        [Required]
        public int MaxSeat { get; set; }
        public int NumOfPassengers { get; set; } = 0;
        [DataType(DataType.Time)]
        public DateTime DepartureTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime ArrivalTime { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    

    }
}
