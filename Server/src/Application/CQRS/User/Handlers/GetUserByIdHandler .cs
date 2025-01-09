using Application.Common.Modoles.ViewModels;
using Domain.Interfaces;
using MediatR;
using Application.CQRS.User.Queries;
using Application.Interfaces;
using MapsterMapper;


namespace Application.CQRS.User.Handlers
{
    public class GetUserByIdQueryHandler(
        IRepository<Domain.Entities.User> repository,
        IMapper mapper)
        : IRequestHandler<GetUserByIdQuery, AppUserViewModel>
    {
        public async Task<AppUserViewModel> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await repository.GetByIdAsync(query.Id, cancellationToken);

            if (user == null)
                throw new Exception("User not found");

            return mapper.Map<AppUserViewModel>(user);
        }
    }
}
