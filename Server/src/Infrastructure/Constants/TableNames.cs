using Domain.Entities;
using Infrastructure.Persistence.Outbox;

namespace Infrastructure.Constants;

internal static class TableNames
{
    internal const string Users = nameof(User);
    internal const string OutboxMessages = nameof(OutboxMessage);
}
