using Application.Common.Mod.Abstraction;

namespace Application.Common.Mod.ViewModels;

public class PasswordResetViewModel(bool success) : BaseViewModel
{
    public bool Success { get; set; } = success;
}
