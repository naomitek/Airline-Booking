using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GBC_Travel_Group_40.Areas.ProductManagement.Models
{
    public class Cars
    {
        [Key]
        public int CarId { get; set; }
        [Required]
        public string CarCompany { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string OrigenLocation { get; set; }
        [Required]
        public string DestinationLocation { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Pricing { get; set; }

        [DataType(DataType.Time)]
        public DateTime DepartureTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime ArrivalTime { get; set; }
        public int LimitPassengers { get; set; }
        public bool Available { get; set; }
        public string Url { get; set; }



    }
}
