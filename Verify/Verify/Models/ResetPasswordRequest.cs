using System.ComponentModel.DataAnnotations;

namespace Verify.Models
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required, MinLength(6), MaxLength(20, ErrorMessage = "Please enter a password with a length of 8 to 20 characters !")]
        public string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
