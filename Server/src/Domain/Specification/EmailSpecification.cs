using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Specification;

public class EmailSpecification(Email email) : Specification<User>(u => u.Email == email);