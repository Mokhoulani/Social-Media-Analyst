using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;

namespace Application.CQRS.User.Commands;

[AllowAnonymous]
public sealed class SignUpCommand : ICommand<Result<TokenResponseViewModel>>
{
    /// <summary>
    ///     Initiates a new instance of the <see cref="SignUpCommand" /> class.
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="email"></param>
    /// <param name="lastName"></param>
    /// <param name="password"></param>
    public SignUpCommand(
        string firstName,
        string email,
        string lastName,
        string password)
    {
        FirstName = firstName;
        Email = email;
        LastName = lastName;
        Password = password;
    }

    /// <summary>
    ///     The username of the AppUser.
    /// </summary>
    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    /// <summary>
    ///     The email of the AppUser.
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    ///     The Password of the AppUser.
    /// </summary>
    public string Password { get; set; }
}

