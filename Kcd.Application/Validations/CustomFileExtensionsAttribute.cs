using Kcd.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Kcd.Application.Validations;

public class CustomFileExtensionsAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var fileValidator = (IFileValidator)validationContext.GetService(typeof(IFileValidator));

        if (fileValidator == null)
        {
            throw new InvalidOperationException("FileValidator service is not registered.");
        }

        if (value != null && value is IFormFile file)
        {
            return fileValidator.Validate(file);
        }

        return ValidationResult.Success;
    }
}
