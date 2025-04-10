using Domain.Entities;

namespace Domain.Specification;

public class ValidResetTokenSpecification(string token)
    : Specification<PasswordResetToken,Guid>(t => t.Token == token && t.ExpiresAt > DateTime.UtcNow && !t.Used);


