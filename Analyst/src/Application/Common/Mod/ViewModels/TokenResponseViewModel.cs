using Application.Common.Mod.Abstraction;

namespace Application.Common.Mod.ViewModels;

public class TokenResponseViewModel(string accessToken, string refreshToken) : BaseViewModel
{
    public string AccessToken { get; } = accessToken;
    public string RefreshToken { get; } = refreshToken;
}
