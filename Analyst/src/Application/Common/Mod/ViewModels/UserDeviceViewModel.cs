using Application.Common.Mod.Abstraction;

namespace Application.Common.Mod.ViewModels;

public class UserDeviceViewModel : BaseViewModel
{
    public string Id { get; set; }
    public string DeviceToken { get; set; }
    public string DeviceId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}