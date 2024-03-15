using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApp.Server.Models
{
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Sender")]
        public required int SenderId { get; set; }
        public virtual User? Sender { get; set; }
        [ForeignKey("Receiver")]
        public required int ReceiverId { get; set; }
        public virtual User? Receiver { get; set; }
        public required string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }
}
