using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Server.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public required string Text { get; set; }
        [ForeignKey("User")]
        public required int UserId { get; set; }
        public virtual User? User { get; set; }
        [ForeignKey("Product")]
        public required int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }
}
