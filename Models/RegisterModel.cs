using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User name is required")]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email should be in a proper format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)[A-Za-z\d]{6,}$", ErrorMessage = "Password must be at least 6 charecters long, contain at least one letter,and at least one number")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
