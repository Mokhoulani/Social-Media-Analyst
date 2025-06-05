using Application.Common.Mod.Abstraction;

namespace Application.Common.Mod.ViewModels;

public class SocialMediaPlatfomViewModel : BaseViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string IconUrl { get; set; }
}