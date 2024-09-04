using AutoMapper;
using FluentAssertions;
using Kcd.Application.Models;
using Kcd.Application.Services;
using Kcd.Common.Enums;
using Kcd.Common.Exceptions;
using Kcd.Domain;
using Kcd.Identity.Models;
using Kcd.Identity.Services;
using Kcd.Infrastructure.Services;
using Kcd.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kcd.Core.Tests.Application;

[TestFixture]
public class ApplicationServiceTests
{
    private Mock<IMapper> _mapper;
    private Mock<IUserApplicationRepository> _repository;
    private Mock<IAuthService> _authService;
    private Mock<IAvatarService> _avatarService;
    private Mock<IEmailSender> _emailSender;
    private Mock<ILogger<ApplicationService>> _logger;
    private ApplicationService _service;

    [SetUp]
    public void Setup()
    {
        _mapper = new Mock<IMapper>();
        _repository = new Mock<IUserApplicationRepository>();
        _authService = new Mock<IAuthService>();
        _avatarService = new Mock<IAvatarService>();
        _emailSender = new Mock<IEmailSender>();
        _logger = new Mock<ILogger<ApplicationService>>();

        _service = new ApplicationService(
            _mapper.Object,
            _repository.Object,
            _authService.Object,
            _avatarService.Object,
            _emailSender.Object,
            _logger.Object
        );
    }

    [Test]
    public async Task ApplyAsync_ShouldThrowBadRequestException_WhenApplicationAlreadyExists()
    {
        // Arrange
        var request = new UserApplicationRequest { Email = "test@example.com" };
        _repository.Setup(r => r.GetUserApplicationByEmail(request.Email, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new UserApplication());

        // Act
        Func<Task> act = async () => await _service.ApplyAsync(request);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();
    }

    [Test]
    public async Task ApplyAsync_ShouldSaveAvatar_WhenAvatarIsProvided()
    {
        // Arrange
        var request = new UserApplicationRequest
        {
            Email = "test@example.com",
            Avatar = CreateMockFormFile("avatar", "avatar.png")
        };
        var userApplication = new UserApplication();
        _repository.Setup(r => r.GetUserApplicationByEmail(request.Email, It.IsAny<CancellationToken>()))
                   .ReturnsAsync((UserApplication)null);

        _avatarService.Setup(a => a.SaveAvatarAsync(It.IsAny<Stream>(), request.Avatar.FileName, It.IsAny<string>()))
                      .ReturnsAsync("avatar-id");

        _mapper.Setup(m => m.Map<UserApplication>(request))
               .Returns(userApplication);

        _mapper.Setup(m => m.Map<UserApplicationResponse>(userApplication))
           .Returns(new UserApplicationResponse
           {
               AvatarId = "avatar-id"
           });

        // Act
        var result = await _service.ApplyAsync(request);

        // Assert
        _repository.Verify(r => r.CreateAsync(userApplication, It.IsAny<CancellationToken>()), Times.Once);
        result.AvatarId.Should().Be("avatar-id");
    }

    [Test]
    public async Task ApproveApplicationAsync_ShouldThrowNotFoundException_WhenApplicationDoesNotExist()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        _repository.Setup(r => r.GetByIdAsync(applicationId, It.IsAny<CancellationToken>()))
                   .ReturnsAsync((UserApplication)null);

        // Act
        Func<Task> act = async () => await _service.ApproveApplicationAsync(applicationId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ApproveApplicationAsync_ShouldUpdateApplicationStatusToApproved()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        var application = new UserApplication { Email = "test@example.com", Status = ApplicationStatus.Pending };
        _repository.Setup(r => r.GetByIdAsync(applicationId, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(application);
        _authService.Setup(a => a.RegisterAsync(It.IsAny<RegistrationRequest>()))
                    .ReturnsAsync(new RegistrationResponse());
        _emailSender.Setup(e => e.SendEmailAsync(application.Email, It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(Task.CompletedTask);

        // Act
        await _service.ApproveApplicationAsync(applicationId);

        // Assert
        _repository.Verify(r => r.UpdateAsync(It.Is<UserApplication>(a => a.Status == ApplicationStatus.Approved), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task RejectApplicationAsync_ShouldThrowNotFoundException_WhenApplicationDoesNotExist()
    {
        // Arrange
        var applicationId = Guid.NewGuid();
        _repository.Setup(r => r.GetByIdAsync(applicationId, It.IsAny<CancellationToken>()))
                   .ReturnsAsync((UserApplication)null);

        // Act
        Func<Task> act = async () => await _service.RejectApplicationAsync(applicationId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task GetApplicationsAsync_ShouldReturnMappedApplications()
    {
        // Arrange
        var applications = new List<UserApplication>
            {
                new UserApplication { Id = Guid.NewGuid(), Email = "test1@example.com" },
                new UserApplication { Id = Guid.NewGuid(), Email = "test2@example.com" }
            };
        var applicationResponses = new List<UserApplicationResponse>
            {
                new UserApplicationResponse { Id = applications[0].Id, Email = applications[0].Email },
                new UserApplicationResponse { Id = applications[1].Id, Email = applications[1].Email }
            };
        _repository.Setup(r => r.GetApplicationsAsync(null, It.IsAny<CancellationToken>()))
                   .ReturnsAsync(applications);
        _mapper.Setup(m => m.Map<IEnumerable<UserApplicationResponse>>(applications))
               .Returns(applicationResponses);

        // Act
        var result = await _service.GetApplicationsAsync();

        // Assert
        result.Should().BeEquivalentTo(applicationResponses);
    }

    // Helper method to create a mock IFormFile
    private IFormFile CreateMockFormFile(string fileName, string contentType)
    {
        var stream = new MemoryStream(new byte[1]); // a stream with a single byte
        var file = new Mock<IFormFile>();
        file.Setup(f => f.OpenReadStream()).Returns(stream);
        file.Setup(f => f.FileName).Returns(fileName);
        file.Setup(f => f.ContentType).Returns(contentType);
        file.Setup(f => f.Length).Returns(stream.Length);
        return file.Object;
    }
}