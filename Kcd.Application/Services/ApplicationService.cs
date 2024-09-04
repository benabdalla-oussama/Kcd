using AutoMapper;
using Kcd.Application.Interfaces;
using Kcd.Application.Models;
using Kcd.Common.Enums;
using Kcd.Common.Exceptions;
using Kcd.Domain;
using Kcd.Identity.Models;
using Kcd.Identity.Services;
using Kcd.Infrastructure.Services;
using Kcd.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using Stayr.Backend.Common.Observability;

namespace Kcd.Application.Services;

/// <summary>
/// Service responsible for handling user applications. 
/// Provides functionality to apply, approve, reject, and retrieve user applications.
/// </summary>
public class ApplicationService(IMapper mapper,
    IUserApplicationRepository repository,
    IAuthService authService,
    IAvatarService avatarService,
    IEmailSender emailSender,
    ILogger<ApplicationService> logger) : IApplicationService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserApplicationRepository _repository = repository;
    private readonly IAuthService _authService = authService;
    private readonly IAvatarService _avatarService = avatarService;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly ILogger<ApplicationService> _logger = logger;


    public async Task<UserApplicationResponse> ApplyAsync(UserApplicationRequest request)
    {
        _logger.LogTrace(LogEvents.TraceMessage, "Applying new user application for {Email}", request.Email);

        // Check if application exists
        var existingApplication = await _repository.GetUserApplicationByEmail(request.Email);
        if (existingApplication != null)
        {
            _logger.LogWarning(LogEvents.Application.ApplyWarning, "User application with ID: {Email} already exists.", request.Email);
            throw new BadRequestException($"User application with ID: {request.Email} already exists.");
        }

        var application = _mapper.Map<UserApplication>(request);
        application.Status = ApplicationStatus.Pending;

        // Save avatar
        if (request.Avatar != null)
        {
            using var avatarStream = request.Avatar.OpenReadStream();
            string avatarId = await _avatarService.SaveAvatarAsync(avatarStream, request.Avatar.FileName, request.Avatar.ContentType);
            application.AvatarId = avatarId; // Assign the avatar ID
        }

        // Save application
        await _repository.CreateAsync(application);
        _logger.LogTrace(LogEvents.TraceMessage, "User application for {Email} successfully added.", request.Email);

        return _mapper.Map<UserApplicationResponse>(application);
    }

    public async Task ApproveApplicationAsync(Guid applicationId)
    {
        _logger.LogTrace(LogEvents.TraceMessage, "Approving user application with ID: {ApplicationId}", applicationId);

        var application = await _repository.GetByIdAsync(applicationId);
        if (application == null)
        {
            _logger.LogWarning(LogEvents.Application.ApproveWarning, "User application with ID: {ApplicationId} not found", applicationId);
            throw new NotFoundException("Application", applicationId);
        }

        await _authService.RegisterAsync(_mapper.Map<RegistrationRequest>(application));

        // Send welcoming email
        await _emailSender.SendEmailAsync(application.Email, "Welcome to our service", string.Empty);

        application.Status = ApplicationStatus.Approved;
        await _repository.UpdateAsync(application);
        _logger.LogTrace(LogEvents.TraceMessage, "User application with ID: {ApplicationId} approved", applicationId);
    }

    public async Task RejectApplicationAsync(Guid applicationId)
    {
        _logger.LogTrace(LogEvents.TraceMessage, "Rejecting user application with ID: {ApplicationId}", applicationId);

        var application = await _repository.GetByIdAsync(applicationId);
        if (application == null)
        {
            _logger.LogWarning(LogEvents.Application.RejectWarning, "User application with ID: {ApplicationId} not found", applicationId);
            throw new NotFoundException("Application", applicationId);
        }

        application.Status = ApplicationStatus.Rejected;
        await _repository.UpdateAsync(application);
        _logger.LogTrace(LogEvents.TraceMessage, "User application with ID: {ApplicationId} rejected", applicationId);
    }

    public async Task<IEnumerable<UserApplicationResponse>> GetApplicationsAsync(ApplicationStatus? status = null)
    {
        _logger.LogTrace(LogEvents.TraceMessage, "Fetching user applications with status: {Status}", status);

        var applications = await _repository.GetApplicationsAsync(status);

        return _mapper.Map<IEnumerable<UserApplicationResponse>>(applications);
    }
}
