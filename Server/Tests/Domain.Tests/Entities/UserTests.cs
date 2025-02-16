using FluentAssertions;
using Domain.Entities;
using Domain.ValueObjects;

public class UserTests
{
    [Fact]
    public void User_Should_Have_Valid_Name_And_Email()
    {
        var email = Email.Create("johndoe@example.com");
        var firstName = FirstName.Create("John");
        var lastName = LastName.Create("Doe");
        var password = Password.Create("SecurePassword123!");
        // Arrange
        var user = User.Create(
            Guid.NewGuid(),
            email,
            firstName,
            lastName,
            password
        );

        // Act & Assert
        user.FirstName.Value.Should().NotBeNullOrWhiteSpace();
        user.LastName.Value.Should().NotBeNullOrWhiteSpace();
        user.Email.Value.Should().MatchRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
    }
}
