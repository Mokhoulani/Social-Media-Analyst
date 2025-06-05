using Application.Common.Mod.Abstraction;

namespace Application.Common.Mod.ViewModels;

public class UserUsageGoalViewModel : BaseViewModel
{
  public int Id { get; set; }
  public string UserId { get; set; }
  public int PlatformId { get; set; }
  public string DailyLimit { get; set; }
}