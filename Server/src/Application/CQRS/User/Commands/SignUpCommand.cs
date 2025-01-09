using Application.Common.Modoles;
using Application.Common.Modoles.ViewModels;
using Domain.ValueObjects;
using MediatR;

namespace Application.CQRS.User.Commands;


public sealed class SignUpCommand : IRequest<AppUserViewModel>
{
    /// <summary>
    ///     Initiates a new instance of the <see cref="SignUpCommand" /> class.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    public SignUpCommand(string username, string email)
    {
        Username = username;
        Email = email;
    }

    /// <summary>
    ///     The username of the AppUser.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    ///     The email of the AppUser.
    /// </summary>
    public string Email { get; set; }
}

// public sealed record SignInResponse(AppUserViewModel User, IEnumerable<Permissions> Claims, string Token);