using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Specification.Users;

public class EmailUniqueSpecification(Email email) : Specification<User>(u => u.Email == email);