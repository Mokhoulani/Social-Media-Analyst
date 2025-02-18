using System.Text;
using FluentAssertions;
using Api.Tests.Common;

namespace Api.Tests.Controllers;

public class UserControllerTests(WebapiWebApplicationFactory factory) :
    IClassFixture<WebapiWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();
     // run at terminal
     // dotnet user-secrets init
     // dotnet user-secrets set "ConnectionStrings:testDb" "DataSource=file::memory:?cache=shared"
    [Fact]
    public async Task GetUserById_Should_Return_OK()
    {
        // Arrange
        var userId = "70ebadd4-e923-4584-b82f-52175d8c80db";

        // Act
        var response = await _client.GetAsync($"/api/user/{userId}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
    
    
    [Fact]
    public async Task LoginUser_Should_Return_OK()
    {
        // Arrange
        var user = new
        {
            Email = "johndoe@example.com",
            Password = "SecurePassword123!"
        };

        var content = new StringContent(
            System.Text.Json.JsonSerializer.Serialize(user),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await _client.PostAsync("/api/user/login", content); 

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

}