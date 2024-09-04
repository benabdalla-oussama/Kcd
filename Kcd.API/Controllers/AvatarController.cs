using Kcd.Common.Enums;
using Kcd.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stayr.Backend.Common.Observability;

namespace Kcd.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Route("v1/")]
[Authorize]
public class AvatarController(IAvatarService avatarService, ILogger<AvatarController> logger) : BaseController
{
    private readonly IAvatarService _avatarService = avatarService;
    private readonly ILogger<AvatarController> _logger = logger;

    /// <summary>
    /// Retrieves the avatar image for a given ID.
    /// </summary>
    /// <param name="id">The unique identifier of the avatar.</param>
    /// <returns>The avatar image file.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAvatarAsync(string id)
    {
        // Check the avatar id saved in the jwt claims if the role is user
        if (UserHasRole(Roles.User.ToString()) && GetUserAvatarId() != id)
        {
            _logger.LogWarning(LogEvents.Api.UnAuthorizedError, "User unauthorized to access avatar with ID: {AvatarId}", id);
            return Forbid("You are not authorized to access this avatar.");
        }

        var avatarStream = await _avatarService.GetAvatarAsync(id);
        if (avatarStream.Stream == null)
        {
            _logger.LogWarning(LogEvents.Api.AvatarNotFoundError, "Avatar not found with ID: {AvatarId}", id);
            return NotFound("Avatar not found.");
        }

        return File(avatarStream.Stream, avatarStream.ContentType, avatarStream.FileName);
    }
}
