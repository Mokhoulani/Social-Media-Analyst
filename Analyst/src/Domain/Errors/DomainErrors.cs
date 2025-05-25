using Domain.Shared;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static readonly Error EmailAlreadyExists = new Error(
            "User.EmailAlreadyExists",
            "The email address is already in use.");

        public static readonly Error NotFound = new Error(
            "User.NotFound",
            $"User not found.");

        public static readonly Error InvalidCredentials = new(
            "User.InvalidCredentials",
            "Invalid credentials.");
    }

    public static class Email
    {
        public static readonly Error Empty = new(
            "Email.Empty",
            "Email cannot be empty.");

        public static readonly Error TooLong = new(
            "Email.TooLong",
            "Email is too long.");

        public static readonly Error InvalidFormat = new(
            "Email.InvalidFormat",
            "The email format is invalid.");

        public static readonly Error Invalid = new(
            "Email.Invalid",
            "The email is invalid.");
    }

    public static class FirstName
    {
        public static readonly Error Empty = new(
            "FirstName.Empty",
            "First name cannot be empty.");

        public static readonly Error TooLong = new(
            "FirstName.TooLong",
            "First name is too long.");
        public static readonly Error Invalid = new(
            "FirstName.Invalid",
            "First name is invalid.");
    }

    public static class LastName
    {
        public static readonly Error Empty = new(
            "LastName.Empty",
            "Last name cannot be empty.");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "Last name is too long.");
        public static readonly Error Invalid = new(
            "LastName.Invalid",
            "Last name is invalid.");
    }

    public static class Password
    {
        public static readonly Error Empty = new(
            "Password.Empty",
            "Password cannot be empty.");

        public static readonly Error TooShort = new(
            "Password.TooShort",
            "Password must be at least 8 characters long.");

        public static readonly Error TooLong = new(
            "Password.TooLong",
            "Password must be at most 50 characters long.");

        public static readonly Error MissingUpperCase = new(
            "Password.MissingUpperCase",
            "Password must contain at least one uppercase letter.");

        public static readonly Error MissingLowerCase = new(
            "Password.MissingLowerCase",
            "Password must contain at least one lowercase letter.");

        public static readonly Error MissingDigit = new(
            "Password.MissingDigit",
            "Password must contain at least one digit.");

        public static readonly Error MissingSpecialChar = new(
            "Password.MissingSpecialChar",
            "Password must contain at least one special character.");

        public static readonly Error NotValid = new(
            "Password.NotValid",
            "Password is invalid.");
    }

    public static Error NotFound<T>() => new(
        $"{typeof(T).Name}.NotFound",
        $"{typeof(T).Name} not found.");

    public static class UserUsageGoal
    {
        public static readonly Error NotFound = new(
            "UserUsageGoal.NotFound",
            "The usage goal for the user was not found.");
    }
}