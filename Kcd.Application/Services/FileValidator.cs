using Kcd.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stayr.Backend.Common.Observability;
using System.ComponentModel.DataAnnotations;

namespace Kcd.Infrastructure.Services;

/// <summary>
/// Service responsible for validating file uploads, such as avatar images.
/// </summary>
public class FileValidator(IOptions<AvatarSettings> settings, ILogger<FileValidator> logger) : IFileValidator
{
    private readonly AvatarSettings _settings = settings.Value;
    private readonly ILogger<FileValidator> _logger = logger;

    public ValidationResult Validate(IFormFile file)
    {
        if (file == null)
        {
            return ValidationResult.Success;
        }

        var extension = Path.GetExtension(file.FileName).TrimStart('.').ToLowerInvariant();
        if (!_settings.AllowedExtensions.Contains(extension))
        {
            _logger.LogWarning(LogEvents.Application.InvalidFileType, "Invalid file type: {Extension}. Allowed types are: {AllowedExtensions}.", extension, string.Join(", ", _settings.AllowedExtensions));
            return new ValidationResult($"Invalid file type. Allowed types are: {string.Join(", ", _settings.AllowedExtensions)}.");
        }

        if (file.Length > _settings.MaxFileSizeInBytes)
        {
            _logger.LogWarning(LogEvents.Application.ExceededMaxFileSize, "File size {FileSize} exceeds maximum allowed size of {MaxSize} bytes.", file.Length, _settings.MaxFileSizeInBytes);
            return new ValidationResult($"File size exceeds the maximum allowed size of {_settings.MaxFileSizeInBytes / (1024 * 1024)}MB.");
        }

        _logger.LogTrace(LogEvents.TraceMessage, "File validated successfully: {FileName}.", file.FileName);
        return ValidationResult.Success;
    }
}
