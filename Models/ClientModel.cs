using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class ClientModel
    {
        [Key]
        [Required(ErrorMessage = "Personal ID is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Personal ID should only contains number")]
        [Length(11, 11, ErrorMessage = "Personal ID should be 11 charachter")]
        public required string PersonalID { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(60)]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(60)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone number should only contains number")]
        [DataType(DataType.PhoneNumber)]
        public required string PhoneNumber { get; set; }


        [Required(ErrorMessage = "Gender is required")]
        [RegularExpression("^[fFmM]$", ErrorMessage = "The gender should be represented by one letter (f, m)")]
        public required string Gender { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email should be in a proper format")]
        public required string Email { get; set; }


        public string? Country { get; set; } = null;
        public string? City { get; set; } = null;
        public string? Street { get; set; } = null;
        public string? ZipCode { get; set; } = null;

    }
}
