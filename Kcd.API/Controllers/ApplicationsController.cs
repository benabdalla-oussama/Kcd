using Kcd.Application.Interfaces;
using Kcd.Application.Models;
using Kcd.Common.Enums;
using Kcd.Identity.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kcd.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApplicationsController(IApplicationService applicationService, ILogger<ApplicationsController> logger) : BaseController
{
    private readonly IApplicationService _applicationService = applicationService;
    private readonly ILogger<ApplicationsController> _logger = logger;

    /// <summary>
    /// Retrieves a list of user applications.
    /// </summary>
    /// <param name="status">Optional filter by application status.</param>
    /// <returns>A list of user applications.</returns>
    [HttpGet]
    [Authorize(Policy = Policies.IsAdmin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetApplications([FromQuery] ApplicationStatus? status = null)
    {
        var applications = await _applicationService.GetApplicationsAsync(status);
        return Ok(applications);
    }

    /// <summary>
    /// Applies for a new user application.
    /// </summary>
    /// <param name="request">The user application data transfer object.</param>
    /// <returns>An IActionResult indicating success or failure.</returns>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Apply([FromForm] UserApplicationRequest request)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid application model received.");
            return BadRequest(ModelState);
        }

        var createdApplication = await _applicationService.ApplyAsync(request);
        return CreatedAtAction(nameof(Apply), new { id = createdApplication.Id }, createdApplication);
    }

    /// <summary>
    /// Approves a user application.
    /// </summary>
    /// <param name="id">The ID of the application to approve.</param>
    /// <returns>An IActionResult indicating success or failure.</returns>
    [HttpPut("approve/{id}")]
    [Authorize(Policy = Policies.IsAdmin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ApproveApplication(Guid id)
    {
        await _applicationService.ApproveApplicationAsync(id);
        return Ok();
    }

    /// <summary>
    /// Rejects a user application.
    /// </summary>
    /// <param name="id">The ID of the application to reject.</param>
    /// <returns>An IActionResult indicating success or failure.</returns>
    [HttpPut("reject/{id}")]
    [Authorize(Policy = Policies.IsAdmin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RejectApplication(Guid id)
    {
        await _applicationService.RejectApplicationAsync(id);
        return Ok();
    }
}

