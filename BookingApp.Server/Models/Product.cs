using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Server.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public required string Cover { get; set; }
        public required string Description { get; set; }
        [ForeignKey("User")]
        public required int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
