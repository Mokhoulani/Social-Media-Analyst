using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Api.Tests.Controllers;

public class UserControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetUsers_Should_Return_OK()
    {
        // Arrange
        var userId = "6D8116CD-96C2-4DCB-B67B-A6122E1FA230";

        // Act
        var response = await _client.GetAsync($"/api/user/{userId}");

        // Log the response content for debugging
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }
}