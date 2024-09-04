using Kcd.Infrastructure.Models;
using Kcd.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace Kcd.Core.Tests.Application;

[TestFixture]
public class FileValidatorTests
{
    private IFileValidator _fileValidator;
    private Mock<IOptions<AvatarSettings>> _mockOptions;
    private Mock<ILogger<FileValidator>> _mockLogger;

    [SetUp]
    public void SetUp()
    {
        _mockOptions = new Mock<IOptions<AvatarSettings>>();
        _mockLogger = new Mock<ILogger<FileValidator>>();

        // Setting up AvatarSettings with allowed extensions and max file size
        _mockOptions.Setup(o => o.Value).Returns(new AvatarSettings
        {
            AllowedExtensions = new[] { "jpg", "png", "gif" },
            MaxFileSizeInBytes = 2 * 1024 * 1024 // 2 MB
        });

        _fileValidator = new FileValidator(_mockOptions.Object, _mockLogger.Object);
    }

    [Test]
    public void Validate_FileIsNull_ReturnsSuccess()
    {
        // Arrange
        IFormFile file = null;

        // Act
        var result = _fileValidator.Validate(file);

        // Assert
        Assert.AreEqual(ValidationResult.Success, result);
    }

    [Test]
    public void Validate_InvalidFileType_ReturnsError()
    {
        // Arrange
        var file = new Mock<IFormFile>();
        file.Setup(f => f.FileName).Returns("test.docx");
        file.Setup(f => f.Length).Returns(1024);

        // Act
        var result = _fileValidator.Validate(file.Object);

        // Assert
        Assert.NotNull(result);
        Assert.AreNotEqual(ValidationResult.Success, result);
        Assert.AreEqual("Invalid file type. Allowed types are: jpg, png, gif.", result.ErrorMessage);
    }

    [Test]
    public void Validate_FileSizeExceedsLimit_ReturnsError()
    {
        // Arrange
        var file = new Mock<IFormFile>();
        file.Setup(f => f.FileName).Returns("test.jpg");
        file.Setup(f => f.Length).Returns(3 * 1024 * 1024); // 3 MB

        // Act
        var result = _fileValidator.Validate(file.Object);

        // Assert
        Assert.NotNull(result);
        Assert.AreNotEqual(ValidationResult.Success, result);
        Assert.AreEqual("File size exceeds the maximum allowed size of 2MB.", result.ErrorMessage);
    }

    [Test]
    public void Validate_ValidFile_ReturnsSuccess()
    {
        // Arrange
        var file = new Mock<IFormFile>();
        file.Setup(f => f.FileName).Returns("test.jpg");
        file.Setup(f => f.Length).Returns(1 * 1024 * 1024); // 1 MB

        // Act
        var result = _fileValidator.Validate(file.Object);

        // Assert
        Assert.AreEqual(ValidationResult.Success, result);
    }
}