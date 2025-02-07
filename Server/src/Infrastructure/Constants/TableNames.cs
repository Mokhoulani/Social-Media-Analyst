using Domain.Entities;
using Infrastructure.Outbox;

namespace Infrastructure.Constants;

internal static class TableNames
{
    internal const string Users = nameof(User);
    internal const string OutboxMessages = nameof(OutboxMessage);
    internal const string RefreshTokens = nameof(RefreshToken);
}
