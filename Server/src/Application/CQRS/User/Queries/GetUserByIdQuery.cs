using MediatR;
namespace Application.CQRS.User.Queries;

public class GetUserByIdQuery : IRequest<UserDto>
{
    public int Id { get; set; }

    public GetUserByIdQuery(int id)
    {
        Id = id;
    }
}

public class UserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string TimeZone { get; set; }
    public DateTime RegistrationDate { get; set; }
    public bool IsActive { get; set; }
}

