using Application.Common.Modoles.ViewModels;
using Application.Abstractions.Messaging;

namespace Application.CQRS.User.Commands;


public sealed class SignUpCommand : ICommand<AppUserViewModel>
{
    /// <summary>
    ///     Initiates a new instance of the <see cref="SignUpCommand" /> class.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    public SignUpCommand(string firstName, string email,string lastName, string password)
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

// public sealed record SignInResponse(AppUserViewModel User, IEnumerable<Permissions> Claims, string Token);