using Application.Common.Behaviours;
using Domain.Shared;
using FluentValidation;
using MediatR;
using Moq;

namespace Application.Tests.ValidationBehavior;

public class ValidationBehaviorTests
{
    private record SampleRequest(string Name) : IRequest<Result>;

    private class SampleRequestValidator : AbstractValidator<SampleRequest>
    {
        public SampleRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }

    [Fact]
    public async Task Handle_NoValidators_ShouldProceedToNextHandler()
    {
        // Arrange
        var validators = new List<IValidator<SampleRequest>>();
        var behavior = new ValidationBehavior<SampleRequest, Result>(validators);
        var request = new SampleRequest("Valid Name");
        var next = new Mock<RequestHandlerDelegate<Result>>();
        next.Setup(n => n()).ReturnsAsync(Result.Success());

        // Act
        var result = await behavior.Handle(request, next.Object, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        next.Verify(n => n(), Times.Once); 
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldProceedToNextHandler()
    {
        // Arrange
        var validators = new List<IValidator<SampleRequest>> { new SampleRequestValidator() };
        var behavior = new ValidationBehavior<SampleRequest, Result>(validators);
        var request = new SampleRequest("Valid Name");
        var next = new Mock<RequestHandlerDelegate<Result>>();
        next.Setup(n => n()).ReturnsAsync(Result.Success());

        // Act
        var result = await behavior.Handle(request, next.Object, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        next.Verify(n => n(), Times.Once); 
    }

    [Fact]
    public async Task Handle_InvalidRequest_ShouldReturnValidationError()
    {
        // Arrange
        var validators = new List<IValidator<SampleRequest>> { new SampleRequestValidator() };
        var behavior = new ValidationBehavior<SampleRequest, Result>(validators);
        var request = new SampleRequest(""); 
        var next = new Mock<RequestHandlerDelegate<Result>>();

        // Act
        var result = await behavior.Handle(request, next.Object, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        next.Verify(n => n(), Times.Never); 
    }
}