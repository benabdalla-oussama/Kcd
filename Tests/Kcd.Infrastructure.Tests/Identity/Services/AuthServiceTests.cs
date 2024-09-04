using FluentAssertions;
using Kcd.Common;
using Kcd.Common.Enums;
using Kcd.Common.Exceptions;
using Kcd.Identity.Entities;
using Kcd.Identity.Models;
using Kcd.Identity.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Kcd.Infrastructure.Tests.Identity.Services;

[TestFixture]
public class AuthServiceTests
{
    private Mock<UserManager<KcdUser>> _userManagerMock;
    private Mock<SignInManager<KcdUser>> _signInManagerMock;
    private Mock<RoleManager<KcdRole>> _roleManagerMock;
    private Mock<ISystemClock> _clockMock;
    private Mock<ILogger<AuthService>> _loggerMock;
    private Mock<IOptions<JwtSettings>> _jwtSettingsMock;
    private AuthService _authService;

    [SetUp]
    public void Setup()
    {
        _userManagerMock = new Mock<UserManager<KcdUser>>(Mock.Of<IUserStore<KcdUser>>(), null, null, null, null, null, null, null, null);
        _signInManagerMock = new Mock<SignInManager<KcdUser>>(_userManagerMock.Object, Mock.Of<IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<KcdUser>>(), null, null, null, null);
        _roleManagerMock = new Mock<RoleManager<KcdRole>>(Mock.Of<IRoleStore<KcdRole>>(), null, null, null, null);
        _clockMock = new Mock<ISystemClock>();
        _loggerMock = new Mock<ILogger<AuthService>>();
        _jwtSettingsMock = new Mock<IOptions<JwtSettings>>();

        _jwtSettingsMock.SetupGet(x => x.Value).Returns(new JwtSettings
        {
            Key = "testkey",
            Issuer = "testissuer",
            Audience = "testaudience",
            DurationInMinutes = 60
        });

        _authService = new AuthService(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _roleManagerMock.Object,
            _clockMock.Object,
            _loggerMock.Object,
            _jwtSettingsMock.Object);
    }

    [Test]
    public void LoginAsync_ShouldThrowNotFoundException_WhenUserNotFound()
    {
        // Arrange
        var request = new AuthRequest { Email = "test@example.com", Password = "Password123" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((KcdUser)null);

        // Act
        Func<Task> act = async () => await _authService.LoginAsync(request);

        // Assert
        act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"User with {request.Email} not found.");
    }

    [Test]
    public void LoginAsync_ShouldThrowBadRequestException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var request = new AuthRequest { Email = "test@example.com", Password = "Password123" };
        var user = new KcdUser { Email = "test@example.com", UserName = "testuser" };
        _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync(user);
        _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, request.Password, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        Func<Task> act = async () => await _authService.LoginAsync(request);

        // Assert
        act.Should().ThrowAsync<BadRequestException>()
            .WithMessage($"Credentials for '{request.Email} aren't valid'.");
    }

    [Test]
    public async Task RegisterAsync_ShouldReturnUserId_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var request = new RegistrationRequest { Email = "test@example.com", UserName = "testuser", Name = "Test User" };
        var user = new KcdUser { Email = "test@example.com", UserName = "testuser" };
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<KcdUser>(), Constants.DefaultPassword)).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(x => x.AddToRoleAsync(user, Roles.User.ToString())).ReturnsAsync(IdentityResult.Success);

        // Act
        var response = await _authService.RegisterAsync(request);

        // Assert
        response.Should().NotBeNull();
        response.UserId.Should().NotBeNull();
    }

    [Test]
    public void RegisterAsync_ShouldThrowBadRequestException_WhenRegistrationFails()
    {
        // Arrange
        var request = new RegistrationRequest { Email = "test@example.com", UserName = "testuser", Name = "Test User" };
        var errors = new[] { new IdentityError { Description = "Error" } };
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<KcdUser>(), Constants.DefaultPassword))
            .ReturnsAsync(IdentityResult.Failed(errors));

        // Act
        Func<Task> act = async () => await _authService.RegisterAsync(request);

        // Assert
        act.Should().ThrowAsync<BadRequestException>()
            .WithMessage("•Error\n");
    }
}
