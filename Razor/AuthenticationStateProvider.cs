using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public CustomAuthenticationStateProvider(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }


    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await _userManager.GetUserAsync(_signInManager.Context.User);
        
        var identity = user != null
            ? new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }, "Bearer")
            : new ClaimsIdentity();

        var userPrincipal = new ClaimsPrincipal(identity);
        return new AuthenticationState(userPrincipal);
    }

    public void MarkUserAsAuthenticated(IdentityUser user)
    {
        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
        }, "Bearer");

        var userPrincipal = new ClaimsPrincipal(identity);
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userPrincipal)));
    }

    public void MarkUserAsLoggedOut()
    {
        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(userPrincipal)));
    }
}
