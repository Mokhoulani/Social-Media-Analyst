using System.Security.Claims;
using Application.Common.Modoles.ViewModels;
using Domain.Entities;
using Mapster;

namespace Application.Common.Mappings;

public class MappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, AppUserViewModel>()
        .Map(dest => dest.Id, src => src.Id.ToString()) // Convert Guid to string
        .Map(dest => dest.FirstName, src => src.FirstName.Value) // Extract FirstName's string value
        .Map(dest => dest.LastName, src => src.LastName.Value) // Extract LastName's string value
        .Map(dest => dest.Email, src => src.Email.Value); // Extract Email's string
    }
}