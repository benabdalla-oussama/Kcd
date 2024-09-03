using Kcd.Identity.Models;
using Kcd.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kcd.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : BaseController
{
    private readonly IAuthService _authenticationService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authenticationService, ILogger<AuthController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login(AuthRequest request)
    {
        var response = await _authenticationService.LoginAsync(request);
        return Ok(response);
    }
}