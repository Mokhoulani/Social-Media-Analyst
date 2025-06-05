using Application.Common.Mod.Abstraction;

namespace Application.Common.Mod.ViewModels;

public class UserSocialMediaUsageViewModel : BaseViewModel
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int PlatformId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}