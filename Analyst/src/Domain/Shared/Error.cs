﻿namespace Domain.Shared;

public sealed class Error(string code, string message) : IEquatable<Error>
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

    public string Code { get; } = code;

    public string Message { get; } = message;


    public static implicit operator string(Error error) => error.Code;

    public static bool operator ==(Error? a, Error? b) =>
        ReferenceEquals(a, b) || (a is not null && b is not null && a.Equals(b));

    public static bool operator !=(Error? a, Error? b) => !(a == b);

    public bool Equals(Error? other) => other is not null && Code == other.Code && Message == other.Message;

    public override bool Equals(object? obj) => obj is Error error && Equals(error);

    public override int GetHashCode() => HashCode.Combine(Code, Message);

    public override string ToString() => $"{Code}: {Message}";
}
