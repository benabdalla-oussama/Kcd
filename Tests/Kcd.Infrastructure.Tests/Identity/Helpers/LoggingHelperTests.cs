using FluentAssertions;
using Kcd.Identity.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Kcd.Infrastructure.Tests.Identity.Helpers;

[TestFixture]
public class LoggingHelperTests
{
    [Test]
    public void GetFullIdentityError_ShouldReturnFormattedString_WhenErrorsAreProvided()
    {
        // Arrange
        var errors = new List<IdentityError>
            {
                new IdentityError { Description = "Error 1" },
                new IdentityError { Description = "Error 2" },
                new IdentityError { Description = "Error 3" }
            };

        var expected = "•Error 1\n•Error 2\n•Error 3\n";

        // Act
        var result = LoggingHelper.GetFullIdentityError(errors);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public void GetFullIdentityError_ShouldReturnEmptyString_WhenNoErrorsAreProvided()
    {
        // Arrange
        var errors = new List<IdentityError>();

        var expected = string.Empty;

        // Act
        var result = LoggingHelper.GetFullIdentityError(errors);

        // Assert
        result.Should().Be(expected);
    }

    [Test]
    public void GetFullIdentityError_ShouldHandleSingleErrorCorrectly()
    {
        // Arrange
        var errors = new List<IdentityError>
            {
                new IdentityError { Description = "Single Error" }
            };

        var expected = "•Single Error\n";

        // Act
        var result = LoggingHelper.GetFullIdentityError(errors);

        // Assert
        result.Should().Be(expected);
    }
}