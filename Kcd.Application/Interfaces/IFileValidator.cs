using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Kcd.Infrastructure.Services;

/// <summary>
/// Interface for validating files, such as user avatars.
/// </summary>
public interface IFileValidator
{
    /// <summary>
    /// Validates the provided file according to specified rules.
    /// </summary>
    /// <param name="file">The file to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating whether the file is valid or not.</returns>
    ValidationResult Validate(IFormFile file);
}
