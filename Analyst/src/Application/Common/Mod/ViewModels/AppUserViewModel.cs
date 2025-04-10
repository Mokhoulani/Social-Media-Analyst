using Application.Common.Mod.Abstraction;

namespace Application.Common.Mod.ViewModels;

public class AppUserViewModel : BaseViewModel
{
   public string Id { get; set; }
   public string FirstName { get; set; }
   public string LastName { get; set; }
   public string Email { get; set; }
}