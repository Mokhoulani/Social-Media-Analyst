using Domain.Base;
using Domain.Exceptions;
using Domain.Events;
using Domain.ValueObjects;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using TimeZone = Domain.ValueObjects.TimeZone;

namespace Domain.Entities;

public class User : IdentityUser<int>, IAggregateRoot
{
    public DateTime RegistrationDate { get; set; }
    public bool IsActive { get; set; } = true;
    
}
