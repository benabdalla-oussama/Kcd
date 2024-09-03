using Kcd.Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Kcd.Infrastructure.Services;

public class FileValidator : IFileValidator
{
    private readonly AvatarSettings _settings;

    public FileValidator(IOptions<AvatarSettings> settings)
    {
        _settings = settings.Value;
    }

    public ValidationResult Validate(IFormFile file)
    {
        if (file == null)
        {
            return ValidationResult.Success;
        }

        // Validate file extension
        var extension = Path.GetExtension(file.FileName).TrimStart('.').ToLowerInvariant();
        if (!_settings.AllowedExtensions.Contains(extension))
        {
            return new ValidationResult($"Invalid file type. Allowed types are: {string.Join(", ", _settings.AllowedExtensions)}.");
        }

        // Validate file size
        if (file.Length > _settings.MaxFileSizeInBytes)
        {
            return new ValidationResult($"File size exceeds the maximum allowed size of {_settings.MaxFileSizeInBytes / (1024 * 1024)}MB.");
        }

        return ValidationResult.Success;
    }
}
