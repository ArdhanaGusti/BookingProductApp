using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BookingApp.Server.Models
{
    [Index(nameof(Role.Name), IsUnique = true)]
    public class Role
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
