using Kcd.Application.Interfaces;
using Kcd.Application.Models;
using Kcd.Common.Enums;
using Kcd.Identity.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kcd.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController : ControllerBase
{
    private readonly IUserApplicationService _userApplicationService;
    private readonly ILogger<ApplicationsController> _logger;

    public ApplicationsController(IUserApplicationService userApplicationService, ILogger<ApplicationsController> logger)
    {
        _userApplicationService = userApplicationService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves a list of user applications.
    /// </summary>
    /// <returns>A list of user applications.</returns>
    [HttpGet]
    [Authorize(Policy = Policies.IsAdmin)]
    public async Task<IActionResult> GetApplications([FromQuery] ApplicationStatus? status = null)
    {
        try
        {
            var applications = await _userApplicationService.GetApplicationsAsync(status);
            return Ok(applications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving applications.");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Applies for a new user application.
    /// </summary>
    /// <param name="UserApplicationRequest">The user application data transfer object.</param>
    /// <returns>An IActionResult indicating success or failure.</returns>
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Apply([FromForm] UserApplicationRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                // Log validation errors
                _logger.LogWarning("Invalid application model received.");
                return BadRequest(ModelState);
            }

            var createdApplication = await _userApplicationService.ApplyAsync(request);
            return CreatedAtAction(nameof(Apply), new { id = createdApplication.Id }, createdApplication);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while applying for user.");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Approves a user application.
    /// </summary>
    /// <param name="id">The ID of the application to approve.</param>
    /// <returns>An IActionResult indicating success or failure.</returns>
    [HttpPut("approve/{id}")]
    [Authorize(Policy = Policies.IsAdmin)]
    public async Task<IActionResult> ApproveApplication(Guid id)
    {
        try
        {
            await _userApplicationService.ApproveApplicationAsync(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while approving the application.");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Rejects a user application.
    /// </summary>
    /// <param name="id">The ID of the application to reject.</param>
    /// <returns>An IActionResult indicating success or failure.</returns>
    [HttpPut("reject/{id}")]
    [Authorize(Policy = Policies.IsAdmin)]
    public async Task<IActionResult> RejectApplication(Guid id)
    {
        try
        {
            await _userApplicationService.RejectApplicationAsync(id);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while rejecting the application.");
            return StatusCode(500, "Internal server error");
        }
    }
}

