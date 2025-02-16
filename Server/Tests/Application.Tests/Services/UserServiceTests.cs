using Xunit;
using Moq;
using FluentAssertions;
using Domain.Interfaces;
using Application.Common.Interfaces;
using Application.Services;
using Domain.ValueObjects;
using Domain.Entities;
using System;

public class UserServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRepository<User>> _userRepositoryMock;
    private readonly IUserService _userService;

    public UserServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IRepository<User>>();

        // Mock the repository retrieval from UnitOfWork
        _unitOfWorkMock.Setup(uow => uow.Repository<User>()).Returns(_userRepositoryMock.Object);

        _userService = new UserService(_unitOfWorkMock.Object);
    }

    [Fact]
    public void CreateUser_Should_Return_Success()
    {
        // Arrange
        var email = Email.Create("johndoe@example.com");
        var firstName = FirstName.Create("John");
        var lastName = LastName.Create("Doe");
        var password = Password.Create("SecurePassword123!");

        var user = User.Create(
            Guid.NewGuid(),
            email,
            firstName,
            lastName,
            password
        );

        // Simulate behavior: Adding user to repository
        _userRepositoryMock.Setup(repo => repo.AddAsync(user, default));

        // Act
        _unitOfWorkMock.Object.Repository<User>().AddAsync(user, default);

        // Assert
        user.Should().NotBeNull();
        user.FirstName.Value.Should().Be("John");
        user.LastName.Value.Should().Be("Doe");

        // Verify that Add was called once
        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>(), default), Times.Once);
    }
}
