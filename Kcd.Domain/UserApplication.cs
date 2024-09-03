using Kcd.Common.Enums;
using Kcd.Domain.Common;

namespace Kcd.Domain;

/// <summary>
/// Represents a user application.
/// </summary>
public class UserApplication : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string? Company { get; set; }
    public string? Referral { get; set; }
    public string? AvatarId { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
}