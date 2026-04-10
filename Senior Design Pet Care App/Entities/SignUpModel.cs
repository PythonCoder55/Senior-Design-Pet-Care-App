using System.ComponentModel.DataAnnotations;

namespace Senior_Design_Pet_Care_App.Entities
{
    public class SignUpModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(10, ErrorMessage = "Password must be at least 10 characters")]
        [RegularExpression(@"^(?=.*[^a-zA-Z0-9]).+$", ErrorMessage = "Password must contain at least one special character")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Passwords must match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}