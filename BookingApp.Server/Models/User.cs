using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

namespace BookingApp.Server.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string UserName { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public DateTime? EmailVerifiedAt { get; set; }
        public required string Password { get; set; }
        public virtual List<Product>? Products { get; set; }
        [ForeignKey("Role")]
        public required int RoleId { get; set; } = 1;
        public virtual Role? Role { get; set; }
    }
}
