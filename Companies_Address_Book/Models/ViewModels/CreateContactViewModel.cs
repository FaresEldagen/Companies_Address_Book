using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Companies_Address_Book.Models.ViewModels
{
    public class CreateContactViewModel
    {
        [Required(ErrorMessage = "Please select a company.")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Contact name is required.")]
        [StringLength(100, ErrorMessage = "Contact name cannot exceed 100 characters.")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateOnly DateOfBirth { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddYears(-25));

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        [RegularExpression(@"^(\+?[0-9]{1,4}[\s-]?)?([0-9]{6,14})$", ErrorMessage = "Please enter a valid phone number format (e.g., +1234567890, +201012345678, or 01012345678).")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        public IEnumerable<SelectListItem>? CompaniesList { get; set; }
    }
}
