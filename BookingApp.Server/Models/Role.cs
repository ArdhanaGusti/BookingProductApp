using System.ComponentModel.DataAnnotations;

namespace BookingApp.Server.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
