namespace Kcd.UI.Models;

public class UserProfile
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string Company { get; set; } = string.Empty;
    public string Referral { get; set; } = string.Empty;
    public string AvatarId { get; set; } = string.Empty;
}
