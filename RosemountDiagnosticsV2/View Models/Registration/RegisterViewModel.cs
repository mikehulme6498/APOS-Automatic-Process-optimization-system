using Microsoft.AspNetCore.Mvc;
using RosemountDiagnosticsV2.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RosemountDiagnosticsV2.View_Models.Registration
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        //[ValidEmailDomain(allowedDomain: "Unilever.com", ErrorMessage = "Email domain must be unilever.com")]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
