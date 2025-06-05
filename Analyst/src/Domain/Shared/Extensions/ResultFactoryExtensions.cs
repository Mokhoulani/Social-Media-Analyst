namespace Domain.Shared.Extensions;

public static class ResultFactoryExtensions
{
    public static TResult AsAuthenticationFailure<TResult>(this (string code, string message) auth)
        where TResult : Result =>
        ResultFactory.ResultFactory.CreateAuthenticationResult<TResult>(auth.code,
            auth.message);

    public static TResult AsAuthorizationFailure<TResult>(this (string code, string message) auth)
        where TResult : Result =>
        ResultFactory.ResultFactory.CreateAuthorizationResult<TResult>(auth.code,
            auth.message);

    public static TResult AsValidationFailure<TResult>(this Error[] errors)
        where TResult : Result =>
        ResultFactory.ResultFactory.CreateValidationResult<TResult>(errors);

    // Extensions for Error objects
    public static TResult AsAuthenticationFailure<TResult>(this Error error)
        where TResult : Result =>
        ResultFactory.ResultFactory.CreateAuthenticationResult<TResult>(error.Code,
            error.Message);

    public static TResult AsAuthorizationFailure<TResult>(this Error error)
        where TResult : Result =>
        ResultFactory.ResultFactory.CreateAuthorizationResult<TResult>(error.Code,
            error.Message);

    public static TResult AsValidationFailure<TResult>(this Error error)
        where TResult : Result =>
        ResultFactory.ResultFactory.CreateValidationResult<TResult>(error);

    public static TResult AsValidationFailure<TResult>(this string message)
        where TResult : Result
    {
        var error = new Error("ValidationError",
            message);
        return ResultFactory.ResultFactory.CreateValidationResult<TResult>(error);
    }

    public static TResult AsValidationFailure<TResult>(this string code,
        string message)
        where TResult : Result
    {
        var error = new Error(code,
            message);
        return ResultFactory.ResultFactory.CreateValidationResult<TResult>(error);
    }

    public static TResult AsValidationFailure<TResult>(this (string code, string message) validation)
        where TResult : Result =>
        ResultFactory.ResultFactory.CreateValidationResult<TResult>(new Error(validation.code,
            validation.message));

    public static TResult AsValidationFailure<TResult>(this (string code, string message) validation,
        params Error[] additionalErrors)
        where TResult : Result
    {
        var errors = new Error[additionalErrors.Length + 1];
        errors[0] = new Error(validation.code,
            validation.message);
        for (int i = 0;
             i < additionalErrors.Length;
             i++)
        {
            errors[i + 1] = additionalErrors[i];
        }

        return ResultFactory.ResultFactory.CreateValidationResult<TResult>(errors);
    }
}