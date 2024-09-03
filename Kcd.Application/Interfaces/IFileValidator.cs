using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Kcd.Infrastructure.Services;

public interface IFileValidator
{
    ValidationResult Validate(IFormFile file);
}