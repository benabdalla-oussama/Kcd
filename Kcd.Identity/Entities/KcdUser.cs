using Microsoft.AspNetCore.Identity;

namespace Kcd.Identity.Entities;

public class KcdUser : IdentityUser
{
    public string Name { get; set; }
    public string Country { get; set; }
    public string? Company { get; set; } = string.Empty;
    public string? Referral { get; set; } = string.Empty;
    public string? AvatarId { get; set; } = string.Empty;
}
