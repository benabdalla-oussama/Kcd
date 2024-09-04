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
    [Display(Name = "Country")]
    public string Country { get; set; }

    [Display(Name = "Company")]
    public string Company { get; set; }

    [Display(Name = "Referral")]
    public string Referral { get; set; }

    [Display(Name = "AvatarId")]
    public string AvatarId { get; set; }
}