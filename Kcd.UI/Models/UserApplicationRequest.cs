using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kcd.UI.Models;

public class UserApplicationRequest
{
    /// <summary>
    /// Full name of the applicant.
    /// </summary>
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    [JsonPropertyName("Name")]
    public string Name { get; set; }

    /// <summary>
    /// Email address of the applicant.
    /// </summary>
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    [StringLength(150, ErrorMessage = "Email address cannot be longer than 150 characters.")]
    [JsonPropertyName("Email")]
    public string Email { get; set; }

    /// <summary>
    /// Country of the applicant.
    /// </summary>
    [Required(ErrorMessage = "Country is required.")]
    [StringLength(100, ErrorMessage = "Country cannot be longer than 100 characters.")]
    [JsonPropertyName("Country")]
    public string Country { get; set; }

    /// <summary>
    /// Company name of the applicant.
    /// </summary>
    [StringLength(100, ErrorMessage = "Company name cannot be longer than 100 characters.")]
    [JsonPropertyName("Company")]
    public string Company { get; set; } = string.Empty;

    /// <summary>
    /// How the applicant heard about KCD.
    /// </summary>
    [StringLength(100, ErrorMessage = "Referral information cannot be longer than 100 characters.")]
    [JsonPropertyName("Referral")]
    public string Referral { get; set; } = string.Empty;

    /// <summary>
    /// Avatar picture of the applicant.
    /// </summary>
    [JsonPropertyName("Avatar")]
    public IBrowserFile? Avatar { get; set; }
}