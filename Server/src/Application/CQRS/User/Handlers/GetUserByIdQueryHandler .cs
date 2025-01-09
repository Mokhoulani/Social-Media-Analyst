using Domain.Interfaces;
using MediatR;
using Application.CQRS.User.Queries;


namespace Application.CQRS.User.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IRepository<Domain.Entities.User> _repository;

        public GetUserByIdQueryHandler(IRepository<Domain.Entities.User> repository)
        {
            _repository = repository;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(query.Id, cancellationToken);

            if (user == null)
                throw new Exception("User not found");

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email.Value,
                Name = user.Name,
                RegistrationDate = user.RegistrationDate,
                IsActive = user.IsActive
            };
        }
    }
}
