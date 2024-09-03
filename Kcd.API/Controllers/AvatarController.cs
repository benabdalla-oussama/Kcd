using Kcd.Common.Enums;
using Kcd.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kcd.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AvatarController(IAvatarService avatarService) : BaseController
{
    private readonly IAvatarService _avatarService = avatarService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAvatarAsync(string id)
    {
        try
        {
            // Check the avatar id saved in the jwt claims if the role is user
            if (UserHasRole(Roles.User.ToString()) && GetUserAvatarId() != id)
            {
                return Forbid("You are not authorized to access this avatar.");
            }

            var avatarStream = await _avatarService.GetAvatarAsync(id);
            if (avatarStream.Stream == null)
            {
                return NotFound("Avatar not found.");
            }

            return File(avatarStream.Stream, avatarStream.ContentType);
        }
        catch (FileNotFoundException)
        {
            return NotFound("Avatar not found.");
        }
    }
}