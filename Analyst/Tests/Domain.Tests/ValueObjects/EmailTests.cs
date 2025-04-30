using Domain.ValueObjects;
using Domain.Errors;
using FluentAssertions;

namespace Domain.Tests.ValueObjects;
public class EmailTests
{
    [Fact]
    public void Create_Should_Return_Success_When_Email_Is_Valid()
    {
        // Arrange
        var validEmail = "test@example.com";

        // Act
        var result = Email.Create(validEmail);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Value.Should().Be(validEmail);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Create_Should_Return_Failure_When_Email_Is_Empty(string input)
    {
        // Act
        var result = Email.Create(input);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(DomainErrors.Email.Empty.Code);
    }

    [Fact]
    public void Create_Should_Return_Failure_When_Email_Too_Long()
    {
        // Arrange
        var longEmail = new string('a', 256) + "@example.com";

        // Act
        var result = Email.Create(longEmail);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(DomainErrors.Email.TooLong.Code);
    }

    [Theory]
    [InlineData("plainaddress")]
    [InlineData("missingatsign.com")]
    [InlineData("missingdomain@.com")]
    public void Create_Should_Return_Failure_When_Email_Is_Invalid_Format(string invalidEmail)
    {
        // Act
        var result = Email.Create(invalidEmail);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Code.Should().Be(DomainErrors.Email.InvalidFormat.Code);
    }
}
