using AutoMapper;
using BenchmarkDotNet.Attributes;
using Kcd.Application.Interfaces;
using Kcd.Application.Models;
using Kcd.Application.Services;
using Kcd.Identity.Services;
using Kcd.Infrastructure.Services;
using Kcd.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kcd.Benchmarks;

[MemoryDiagnoser]
public class ApplicationServiceBenchmarks
{
    private readonly IApplicationService _applicationService;
    private readonly UserApplicationRequest _userApplicationRequest;
    public ApplicationServiceBenchmarks()
    {
        // Set up mocks for dependencies
        var mapperMock = new Mock<IMapper>();
        var repositoryMock = new Mock<IUserApplicationRepository>();
        var authServiceMock = new Mock<IAuthService>();
        var avatarServiceMock = new Mock<IAvatarService>();
        var emailSenderMock = new Mock<IEmailSender>();
        var loggerMock = new Mock<ILogger<ApplicationService>>();

        // Initialize the service
        _applicationService = new ApplicationService(
            mapperMock.Object,
            repositoryMock.Object,
            authServiceMock.Object,
            avatarServiceMock.Object,
            emailSenderMock.Object,
            loggerMock.Object
        );

        // Create a test request
        _userApplicationRequest = new UserApplicationRequest
        {
            Email = "testuser@example.com",
            Name = "Test User",
            Country = "Netherlands",
            Avatar = null // Assume no avatar for simplicity
        };

    }

    [Benchmark]
    public async Task FetchApplicationsBenchmark()
    {
        await _applicationService.GetApplicationsAsync(null); // Fetch all applications
    }

    [Benchmark]
    public async Task ApplyUserApplicationBenchmark()
    {
        await _applicationService.ApplyAsync(_userApplicationRequest);
    }
}