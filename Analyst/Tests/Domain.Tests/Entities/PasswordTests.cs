using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.Entities;

public class PasswordTests
{
    [Fact]
    public void Password_Should_Have_Valid_Values()
    {
        // Arrange
        string plainTextPassword = "SecurePassword123!";

        // Act
        var passwordResult = Password.Create(plainTextPassword);

        // Assert
        passwordResult.IsSuccess.Should().BeTrue();
        passwordResult.Value.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]                     // Empty password
    [InlineData("short")]                // Too short
    [InlineData("alllowercase1!")]       // Missing uppercase
    [InlineData("ALLUPPERCASE1!")]       // Missing lowercase
    [InlineData("NoDigits!")]            // Missing digit
    [InlineData("NoSpecialChar1")]       // Missing special character
    public void Password_Should_Return_Error_When_Invalid(string invalidPassword)
    {
        // Act
        var passwordResult = Password.Create(invalidPassword);

        // Assert
        passwordResult.IsFailure.Should().BeTrue();
        passwordResult.Error.Should().NotBeNull();
    }
}