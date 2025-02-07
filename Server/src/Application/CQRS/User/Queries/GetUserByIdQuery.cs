using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;

namespace Application.CQRS.User.Queries;

public class GetUserByIdQuery :ICommand<AppUserViewModel>
{
    public string Id { get; set; }

    public GetUserByIdQuery(string id)
    {
        Id = id;
    }
}



