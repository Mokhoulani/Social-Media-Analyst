using Application.Abstractions.Messaging;
using Application.Common.Mod.ViewModels;
using Domain.Shared;

namespace Application.CQRS.Goal.Queries;

public record GetUserUsageGoalsByUserIdQuery(string UserId)
    : IQuery<Result<List<UserUsageGoalViewModel>>>;