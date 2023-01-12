using System.ComponentModel.DataAnnotations;

namespace Identity.Models.DTO
{
    public class RegistrationModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [RegularExpression("^(?=.*[0 - 9])(?=.*[a - z])(?=.*[A - Z])(?=.*[@#$%^&-+=()])(?=\\S+$).{8, 20}$", ErrorMessage = "Password must contain 8 to 20 characters. It must contain 1 uppercase, 1 lowercase, 1 special character and 1 digit")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
        public string? Role { get; set; }
    }
}
