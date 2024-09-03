using AutoMapper;
using Kcd.Application.Interfaces;
using Kcd.Application.Models;
using Kcd.Common.Enums;
using Kcd.Common.Exceptions;
using Kcd.Domain;
using Kcd.Identity.Models;
using Kcd.Identity.Services;
using Kcd.Persistence.Repositories;
using Microsoft.Extensions.Logging;

namespace Kcd.Application.Services;

public class UserApplicationService(IMapper mapper,
        IUserApplicationRepository repository,
        IAuthService authService,
        ILogger<UserApplicationService> logger) : IUserApplicationService
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserApplicationRepository _repository = repository;
    private readonly IAuthService _authService = authService;
    private readonly ILogger<UserApplicationService> _logger = logger;

    public async Task<UserApplicationResponse> ApplyAsync(UserApplicationRequest request)
    {
        _logger.LogInformation("Applying new user application for {Email}", request.Email);

        var existingAppLication = await _repository.GetUserApplicationByEmail(request.Email);
        if (existingAppLication != null)
        {
            _logger.LogWarning("User application with ID: {Email} already exists.", request.Email);
            throw new BadRequestException($"User application with ID: {request.Email} already exists.");
        }

        var application = _mapper.Map<UserApplication>(request);
        application.Status = ApplicationStatus.Pending;

        await _repository.CreateAsync(application);
        _logger.LogInformation("User application for {Email} successfully added.", request.Email);

        return _mapper.Map<UserApplicationResponse>(application);
    }

    public async Task ApproveApplicationAsync(Guid applicationId)
    {
        _logger.LogInformation("Approving user application with ID: {ApplicationId}", applicationId);

        var application = await _repository.GetByIdAsync(applicationId);
        if (application == null)
        {
            _logger.LogWarning("User application with ID: {ApplicationId} not found", applicationId);
            throw new NotFoundException("Application", applicationId);
        }

        application.Status = ApplicationStatus.Approved;
        await _repository.UpdateAsync(application);

        await _authService.RegisterAsync(_mapper.Map<RegistrationRequest>(application));
        _logger.LogInformation("User application with ID: {ApplicationId} approved", applicationId);
    }

    public async Task RejectApplicationAsync(Guid applicationId)
    {
        _logger.LogInformation("Rejecting user application with ID: {ApplicationId}", applicationId);

        var application = await _repository.GetByIdAsync(applicationId);
        if (application == null)
        {
            _logger.LogWarning("User application with ID: {ApplicationId} not found", applicationId);
            throw new NotFoundException("Application", applicationId);
        }

        application.Status = ApplicationStatus.Rejected;
        await _repository.UpdateAsync(application);
        _logger.LogInformation("User application with ID: {ApplicationId} rejected", applicationId);
    }

    public async Task<IEnumerable<UserApplicationResponse>> GetApplicationsAsync(ApplicationStatus? status = null)
    {
        _logger.LogInformation("Fetching pending user applications");

        var pendingApplications = await _repository.GetApplicationsAsync(status);
        _logger.LogInformation("Retrieved {Count} pending user applications", pendingApplications.Count());

        return _mapper.Map<IEnumerable<UserApplicationResponse>>(pendingApplications);
    }
}
