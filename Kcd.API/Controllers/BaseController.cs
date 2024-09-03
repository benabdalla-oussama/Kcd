using Kcd.Common;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Kcd.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected bool UserHasRole(string role)
    {
        return HttpContext.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == role);
    }

    protected string? GetUserAvatarId()
    {
        return HttpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.Avatar_Id_Claim_Type)?.Value;
    }

    protected string? GetUserId()
    {
        return HttpContext.User.Claims.FirstOrDefault(c => c.Type == Constants.Uid_Claim_Type)?.Value;
    }
}
