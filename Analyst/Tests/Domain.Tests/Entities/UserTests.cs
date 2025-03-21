using Domain.Entities;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void User_Should_Have_Valid_Name_And_Email()
    {
        var emailResult = Email.Create("johndoe@example.com");
        var firstNameResult = FirstName.Create("John");
        var lastNameResult = LastName.Create("Doe");
        var passwordResult = Password.Create("SecurePassword123!");
        
        // Arrange
        var user = User.Create(
            Guid.NewGuid(),
            emailResult.Value,
            firstNameResult.Value,
            lastNameResult.Value,
            passwordResult.Value);

        // Act & Assert
        user.FirstName.Value.Should().NotBeNullOrWhiteSpace();
        user.LastName.Value.Should().NotBeNullOrWhiteSpace();
        user.Email.Value.Should().MatchRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}