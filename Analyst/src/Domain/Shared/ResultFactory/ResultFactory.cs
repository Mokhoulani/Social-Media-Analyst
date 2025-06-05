using System.Collections.Concurrent;
using System.Linq.Expressions;
using Domain.Shared.ResultTypes.AuthenticationResult;
using Domain.Shared.ResultTypes.AuthorizationResult;
using Domain.Shared.ResultTypes.ValidationResult;
using AuthenticationResult = Domain.Shared.ResultTypes.AuthenticationResult.AuthenticationResult;
using AuthorizationResult = Domain.Shared.ResultTypes.AuthorizationResult.AuthorizationResult;

namespace Domain.Shared.ResultFactory;

public static class ResultFactory
{
    private static readonly ConcurrentDictionary<(Type resultType, string operation), Func<string, string, Result>>
        _authFactories = new();

    private static readonly ConcurrentDictionary<Type, Func<Error[], Result>> _validationFactories = new();

    public static TResult CreateAuthenticationResult<TResult>(string code,
        string message)
        where TResult : Result
    {
        return CreateAuthResult<TResult>(code,
            message,
            "authentication");
    }

    public static TResult CreateAuthorizationResult<TResult>(string code,
        string message)
        where TResult : Result
    {
        return CreateAuthResult<TResult>(code,
            message,
            "authorization");
    }

    public static TResult CreateValidationResult<TResult>(params Error[] errors)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
            return (TResult)(object)ValidationResult.WithErrors(errors);

        if (!typeof(TResult).IsGenericType)
            throw new ArgumentException($"Type {typeof(TResult).Name} must be a generic Result type");

        var valueType = typeof(TResult).GetGenericArguments()[0];
        var factory = _validationFactories.GetOrAdd(valueType,
            CreateValidationFactory);
        return (TResult)factory(errors);
    }

    private static TResult CreateAuthResult<TResult>(string code,
        string message,
        string operation)
        where TResult : Result
    {
        if (typeof(TResult) == typeof(Result))
        {
            return operation switch
            {
                "authentication" => (TResult)(object)AuthenticationResult.Unauthenticated(code,
                    message),
                "authorization" => (TResult)(object)AuthorizationResult.Unauthorized(code,
                    message), // Fixed: should be Unauthorized
                _ => throw new ArgumentException($"Unknown operation: {operation}")
            };
        }

        if (!typeof(TResult).IsGenericType)
            throw new ArgumentException($"Type {typeof(TResult).Name} must be a generic Result type");

        var valueType = typeof(TResult).GetGenericArguments()[0];
        var cacheKey = (valueType, operation);
        var factory = _authFactories.GetOrAdd(cacheKey,
            key => CreateAuthFactoryForType(key.resultType,
                key.operation));
        return (TResult)factory(code,
            message);
    }

    private static Func<string, string, Result> CreateAuthFactoryForType(Type valueType,
        string operation)
    {
        var codeParam = Expression.Parameter(typeof(string),
            "code");
        var messageParam = Expression.Parameter(typeof(string),
            "message");

        var (genericType, methodName) = operation switch
        {
            "authentication" => (typeof(AuthenticationResult<>), nameof(AuthenticationResult<object>.Unauthenticated)),
            "authorization" => (typeof(AuthorizationResult<>),
                nameof(AuthorizationResult<object>.Unauthorized)), // Fixed
            _ => throw new ArgumentException($"Unknown operation: {operation}")
        };

        var method = genericType
                         .MakeGenericType(valueType)
                         .GetMethod(methodName,
                             new[]
                             {
                                 typeof(string),
                                 typeof(string)
                             }) ??
                     throw new InvalidOperationException($"Method {methodName} not found");

        var call = Expression.Call(method,
            codeParam,
            messageParam);
        var lambda = Expression.Lambda<Func<string, string, Result>>(call,
            codeParam,
            messageParam);
        return lambda.Compile();
    }

    private static Func<Error[], Result> CreateValidationFactory(Type valueType)
    {
        var methodInfo = typeof(ValidationResult<>)
                             .MakeGenericType(valueType)
                             .GetMethod(nameof(ValidationResult<object>.WithErrors),
                                 new[]
                                 {
                                     typeof(Error[])
                                 }) ??
                         throw new InvalidOperationException($"Method WithErrors not found for type {valueType}");

        var errorsParam = Expression.Parameter(typeof(Error[]),
            "errors");
        var callExpr = Expression.Call(methodInfo,
            errorsParam);
        var lambda = Expression.Lambda<Func<Error[], Result>>(callExpr,
            errorsParam);
        return lambda.Compile();
    }
}