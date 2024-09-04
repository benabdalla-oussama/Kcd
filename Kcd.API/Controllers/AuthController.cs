using Kcd.Identity.Models;
using Kcd.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kcd.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Route("v1/")]
public class AuthController(IAuthService authenticationService) : BaseController
{
    private readonly IAuthService _authenticationService = authenticationService;

    /// <summary>
    /// Authenticates a user and generates a JWT token.
    /// </summary>
    /// <param name="request">The user authentication request object.</param>
    /// <returns>An AuthResponse containing the JWT token and user details.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
    {
        var response = await _authenticationService.LoginAsync(request);
        return Ok(response);
    }
}