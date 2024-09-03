using Kcd.Common.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Kcd.Identity.Helpers;

public static class Policies
{
    public const string IsAdmin = "IsAdmin";
    public const string IsUser = "IsUser";


    public static AuthorizationPolicy IsAdminPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(Roles.Admin.ToString())
            .Build();
    }

    public static AuthorizationPolicy IsUserPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireRole(Roles.User.ToString())
            .Build();
    }
}
