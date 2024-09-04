using AutoMapper;
using FluentAssertions;
using Kcd.Application.Services;
using Kcd.Common.Enums;
using Kcd.Domain;
using Kcd.Infrastructure.Models;
using Kcd.Infrastructure.Storage;
using Kcd.Persistence.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Kcd.Core.Tests.Application;

[TestFixture]
public class AvatarServiceTests
{
    private Mock<IEnumerable<IAvatarStorageStrategy>> _mockStorageStrategies;
    private Mock<IAvatarRepository> _mockRepository;
    private Mock<IMapper> _mockMapper;
    private Mock<IOptions<AvatarSettings>> _mockOptions;
    private Mock<ILogger<AvatarService>> _mockLogger;
    private AvatarService _avatarService;

    [SetUp]
    public void SetUp()
    {
        _mockStorageStrategies = new Mock<IEnumerable<IAvatarStorageStrategy>>();
        _mockRepository = new Mock<IAvatarRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockOptions = new Mock<IOptions<AvatarSettings>>();
        _mockLogger = new Mock<ILogger<AvatarService>>();

        var avatarSettings = new AvatarSettings { StorageStrategy = "Database" };
        _mockOptions.Setup(o => o.Value).Returns(avatarSettings);

        _avatarService = new AvatarService(
            _mockStorageStrategies.Object,
            _mockRepository.Object,
            _mockMapper.Object,
            _mockOptions.Object,
            _mockLogger.Object
        );
    }

    [Test]
    public async Task SaveAvatarAsync_ShouldReturnAvatarId_WhenAvatarIsSaved()
    {
        // Arrange
        var avatarStream = new MemoryStream();
        var fileName = "test-avatar.png";
        var contentType = "image/png";
        var strategy = AvatarStorageStrategy.Database;
        var guid = Guid.NewGuid();
        var avatarModel = new AvatarModel { Id = guid, FileName = fileName, ContentType = contentType, StorageStrategy = strategy };
        var avatar = new Avatar { Id = guid, FileName = fileName, ContentType = contentType, StorageStrategy = strategy };

        var mockStrategy = new Mock<IAvatarStorageStrategy>();
        mockStrategy.Setup(x => x.StorageStrategy).Returns(strategy);

        mockStrategy.Setup(s => s.SaveAvatarAsync(It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(avatarModel);
        _mockStorageStrategies.Setup(s => s.GetEnumerator()).Returns(new List<IAvatarStorageStrategy> { mockStrategy.Object }.GetEnumerator());

        _mockMapper.Setup(m => m.Map<Avatar>(It.IsAny<AvatarModel>()))
            .Returns(avatar);
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Avatar>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _avatarService.SaveAvatarAsync(avatarStream, fileName, contentType);

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().Be(guid.ToString());
    }

    [Test]
    public async Task GetAvatarAsync_ShouldReturnAvatarStream_WhenAvatarExists()
    {
        // Arrange
        var avatarId = Guid.NewGuid().ToString();
        var avatar = new Avatar { Id = Guid.Parse(avatarId), FileName = "test-avatar.png", ContentType = "image/png" };
        var avatarModel = new AvatarModel { FileName = "test-avatar.png", ContentType = "image/png" };
        var avatarStream = new MemoryStream();

        var mockStrategy = new Mock<IAvatarStorageStrategy>();
        mockStrategy.Setup(s => s.GetAvatarAsync(It.IsAny<AvatarModel>()))
            .ReturnsAsync(avatarStream);
        _mockStorageStrategies.Setup(s => s.GetEnumerator()).Returns(new List<IAvatarStorageStrategy> { mockStrategy.Object }.GetEnumerator());

        _mockMapper.Setup(m => m.Map<AvatarModel>(It.IsAny<Avatar>()))
            .Returns(avatarModel);
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(avatar);

        // Act
        var (stream, contentType, fileName) = await _avatarService.GetAvatarAsync(avatarId);

        // Assert
        stream.Should().NotBeNull();
        contentType.Should().Be("image/png");
        fileName.Should().Be("test-avatar.png");
    }

    [Test]
    public void GetAvatarAsync_ShouldThrowFileNotFoundException_WhenAvatarDoesNotExist()
    {
        // Arrange
        var avatarId = Guid.NewGuid().ToString();
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Avatar)null);

        // Act
        Func<Task> act = async () => await _avatarService.GetAvatarAsync(avatarId);

        // Assert
        act.Should().ThrowAsync<FileNotFoundException>()
            .WithMessage("Avatar not found.");
    }

    [Test]
    public void GetStrategy_ShouldThrowInvalidOperationException_WhenNoStrategyFound()
    {
        // Arrange
        var avatarId = Guid.NewGuid().ToString();
        var avatarSettings = new AvatarSettings { StorageStrategy = "NonExistentStrategy" };
        _mockOptions.Setup(o => o.Value).Returns(avatarSettings);
        var avatar = new Avatar { Id = Guid.Parse(avatarId), FileName = "test-avatar.png", ContentType = "image/png" };
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(avatar);

        // Act
        Func<Task> act = async () => await _avatarService.GetAvatarAsync(avatarId);

        // Assert
        act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("No suitable storage strategy found.");
    }
}
