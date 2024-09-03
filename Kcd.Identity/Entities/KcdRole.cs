using Microsoft.AspNetCore.Identity;

namespace Kcd.Identity.Entities;

public class KcdRole : IdentityRole
{
    public KcdRole(string name) : base(name)
    {
    }
    public string Description { get; set; }
}