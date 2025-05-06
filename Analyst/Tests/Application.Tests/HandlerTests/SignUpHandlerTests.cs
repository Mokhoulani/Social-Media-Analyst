using Application.Common.Interfaces;
using Application.Common.Mod.ViewModels;
using Application.CQRS.User.Commands;
using Application.CQRS.User.Handlers;
using Domain.Entities;
using Domain.Errors;
using Domain.Interfaces;
using Domain.Shared;
using Domain.Shared.Extensions;
using Domain.ValueObjects;
using FluentAssertions;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.Tests.HandlerTests;

public class SignUpHandlerTests
{
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<SignUpHandler>> _loggerMock = new();
    private readonly Mock<IPermissionService> _permissionServiceMock = new();

    [Fact]
    public async Task Handle_ShouldReturnViewModel_WhenSignUpIsSuccessful()
    {
        // Arrange
        var command = new SignUpCommand("John", "test@example.com", "Doe", "StrongPassword1!");

        var result = Email.Create(command.Email)
            .Bind(email => Password.Create(command.Password)
                .Bind(password => FirstName.Create(command.FirstName)
                    .Bind(firstName => LastName.Create(command.LastName)
                        .Map(lastName => new
                        {
                            Email = email,
                            Password = password,
                            FirstName = firstName,
                            LastName = lastName
                        }))));

        Assert.True(result.IsSuccess, $"Invalid test data: {result.Error}");

        _permissionServiceMock.Setup(s => s.GetRoleByNameAsync("registered", CancellationToken.None))
            .ReturnsAsync(Result.Success(new Role(1, "registered")));

        var roleResult = await _permissionServiceMock.Object.GetRoleByNameAsync("registered", CancellationToken.None);

        var domainUser = User.Create(Guid.NewGuid(), result.Value.Email, result.Value.FirstName, result.Value.LastName,
            result.Value.Password);

        if (roleResult.IsSuccess)
            domainUser.Roles.Add(roleResult.Value);

        _userServiceMock.Setup(s => s.AddUserAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success(domainUser));

        var viewModel = new AppUserViewModel
        {
            Id = domainUser.Id.ToString(),
            Email = domainUser.Email.Value,
            FirstName = domainUser.FirstName.Value,
            LastName = domainUser.LastName.Value,
        };

        _mapperMock
            .Setup(x => x.Map<AppUserViewModel>(It.IsAny<User>()))
            .Returns(viewModel);

        var handler = new SignUpHandler(_userServiceMock.Object, _permissionServiceMock.Object, _mapperMock.Object, _loggerMock.Object);

        // Act
        var resultVm = await handler.Handle(command, CancellationToken.None);

        // Assert
        resultVm.IsSuccess.Should().BeTrue();
        resultVm.Value.Should().BeEquivalentTo(viewModel);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenEmailIsInvalid()
    {
        // Arrange
        var command = new SignUpCommand("John", "invalid-email", "Doe", "StrongPassword1!");
        var handler = new SignUpHandler(_userServiceMock.Object, _permissionServiceMock.Object, _mapperMock.Object, _loggerMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().NotBeNull();
        result.Error.Code.Should().Be(DomainErrors.Email.InvalidFormat.Code);
    }

}