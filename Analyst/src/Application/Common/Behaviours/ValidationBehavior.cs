using Domain.Shared;
using Domain.Shared.Extensions;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviours;

public class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var validationResults = await Task.WhenAll(validators
            .Select(v => v.ValidateAsync(request,
                cancellationToken)));

        Error[] errors = validationResults.SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .Select(failure => new Error(failure.PropertyName,
                failure.ErrorMessage))
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return errors.AsValidationFailure<TResponse>();
        }

        return await next();
    }
}