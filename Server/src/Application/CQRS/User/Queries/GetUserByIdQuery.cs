using Application.Common.Modoles.ViewModels;
using MediatR;
namespace Application.CQRS.User.Queries;

public class GetUserByIdQuery : IRequest<AppUserViewModel>
{
    public int Id { get; set; }

    public GetUserByIdQuery(int id)
    {
        Id = id;
    }
}



