using Kcd.Application.Validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Kcd.Application.Models;

/// <summary>
/// Represents a user application request submitted by a prospective user.
/// </summary>
public class UserApplicationRequest
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    [StringLength(150, ErrorMessage = "Email address cannot be longer than 150 characters.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    [StringLength(100, ErrorMessage = "Country cannot be longer than 100 characters.")]
    public string Country { get; set; }

    [StringLength(100, ErrorMessage = "Company name cannot be longer than 100 characters.")]
    public string? Company { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Referral information cannot be longer than 100 characters.")]
    public string? Referral { get; set; } = string.Empty;

    /// <summary>
    /// Avatar picture of the applicant.
    /// </summary>
    [CustomFileExtensions]
    public IFormFile? Avatar { get; set; }
}
