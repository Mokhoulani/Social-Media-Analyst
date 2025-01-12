using Domain.ValueObjects;

namespace Application.Common.Modoles.ViewModels;

public class AppUserViewModel
{
   public string Id { get; set; }
   public string FirstName { get; set; }
   public string LastName { get; set; }
   public string Email { get; set; }
}