using Domain.Entities;
using Domain.Interfaces;
using Domain.Shared;
using Domain.Specification.Roles;
using FluentAssertions;
using Infrastructure.Authentication;
using Moq;

namespace Infrastructure.Tests.Authentication;

public class PermissionServiceTest
{
    private readonly Mock<IRepository<Role, int>> _roleRepositoryMock;
    private readonly IPermissionService _permissionService;

    public PermissionServiceTest()
    {
        Mock<IUnitOfWork> unitOfWorkMock = new();
        _roleRepositoryMock = new Mock<IRepository<Role, int>>();
        unitOfWorkMock.Setup(u => u.Repository<Role, int>()).Returns(_roleRepositoryMock.Object);
        _permissionService = new PermissionService(unitOfWorkMock.Object);
    }


    [Fact]
    public async Task GetRoleByNameAsync_Should_ReturnRole_WhenNameExists()
    {
        // Arrange
        var roleName = "registered";
        var expectedRole = new Role(1, roleName);

        _roleRepositoryMock
            .Setup(r => r.FindOneAsync(It.IsAny<RoleByNameSpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(expectedRole));

        // Act
        var result = await _permissionService.GetRoleByNameAsync(roleName);

        // Assert
        result.Should().NotBeNull();

        var nonNullResult = result!;
        nonNullResult.Value.Name.Should().Be("registered");
        nonNullResult.Value.Id.Should().Be(1);
    }


    [Fact]
    public async Task GetRoleByNameAsync_Should_ReturnNull_WhenNameDoesNotExist()
    {
        // Arrange
        var roleName = "nonexistent";
        var failureResult = Result.Failure<Role>(new Error("RoleNotFound", "Role not found"));

        _roleRepositoryMock
            .Setup(r => r.FindOneAsync(It.IsAny<RoleByNameSpecification>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(failureResult);

        // Act
        var result = await _permissionService.GetRoleByNameAsync(roleName);

        // Assert
        result.Error.Message.Should().Be("Role not found");
        result.Error.Code.Should().Be("RoleNotFound");
    }
}