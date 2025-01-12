using Application.Common.Modoles.ViewModels;
using MediatR;
namespace Application.CQRS.User.Queries;

public class GetUserByIdQuery : IRequest<AppUserViewModel>
{
    public string Id { get; set; }

    public GetUserByIdQuery(string id)
    {
        Id = id;
    }
}



