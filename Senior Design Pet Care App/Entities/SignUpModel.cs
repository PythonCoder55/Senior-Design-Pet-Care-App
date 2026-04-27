using System.ComponentModel.DataAnnotations;

namespace Senior_Design_Pet_Care_App.Entities
{
    public class SignUpModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression("^(?=.{10,}$)(?=.*[A-Z])(?=.*\\d)(?=.*[!@#$%^&*()_+\\-=[\\]{};:'\",.<>/?|\\\\]).+$", ErrorMessage = "Password must be at least 10 characters and contain an uppercase letter, a number, and a special character.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}