using Kcd.Common.Enums;

namespace Kcd.Application.Models;

/// <summary>
/// Represents a user application response.
/// </summary>
public class UserApplicationResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Referral { get; set; } = string.Empty;
    public string AvatarId { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public ApplicationStatus Status { get; set; }
}
