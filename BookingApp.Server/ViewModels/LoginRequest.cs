using System.ComponentModel.DataAnnotations;

namespace BookingApp.Server.ViewModels
{
    public class LoginRequest
    {
        public required string Email { get; set; }

        public required string Password { get; set; }
    }
}
