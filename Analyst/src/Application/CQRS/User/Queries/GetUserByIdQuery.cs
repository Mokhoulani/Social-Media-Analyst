using Application.Abstractions.Messaging;
using Application.Common.CustomAttributes;
using Application.Common.Mod.ViewModels;
using Domain.Shared;

namespace Application.CQRS.User.Queries;

[Cache("User-{0}", 300)]
public record GetUserByIdQuery(string Id) : IQuery<Result<AppUserViewModel>>;


