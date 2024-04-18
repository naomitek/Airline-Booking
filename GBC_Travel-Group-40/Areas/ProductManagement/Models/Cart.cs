using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace GBC_Travel_Group_40.Areas.ProductManagement.Models
{
    public class Cart

    {
        [Key]
        public int ProductId { get; set; }
        public string ProductType { get; set; }
        public int? FlightId { get; set; }
        public int? CarId { get; set; }
        public int? RoomId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        // foreign key for user
        public string UserId { get; set; }
    }
}
