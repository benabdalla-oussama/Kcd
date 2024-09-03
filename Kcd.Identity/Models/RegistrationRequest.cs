using Kcd.Common;
using System.ComponentModel.DataAnnotations;

namespace Kcd.Identity.Models;

public class RegistrationRequest
{
    [Required]
    [StringLength(128, ErrorMessage = "Name is required")]
    [Display(Name = "Name")]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    [Display(Name = "UserName")]
    public string UserName { get; set; }

    [Required]
    [MinLength(6)]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = Constants.DefaultPassword;

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = Constants.DefaultPassword;

    [Required]
    [Display(Name = "Country")]
    public string Country { get; set; }

    [Display(Name = "Company")]
    public string Company { get; set; }

    [Display(Name = "Referral")]
    public string Referral { get; set; }

    [Display(Name = "AvatarId")]
    public string AvatarId { get; set; }
}