using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;

namespace Application.CQRS.User.Queries;

public class GetUserByIdQuery(string id) : ICommand<Result<AppUserViewModel>>
{
    public string Id { get; set; } = id;
}



