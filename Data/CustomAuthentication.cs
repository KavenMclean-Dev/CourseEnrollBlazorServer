using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using CourseEnrollBlazorServer.Data;
using System.Threading.Tasks;

public class CustomAuthentication : AuthenticationStateProvider
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;

    private IdentityUser? _currentUser;

    public CustomAuthentication(ApplicationDbContext db, UserManager<IdentityUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_currentUser == null)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        // Use ClaimTypes.Name so user.Identity.Name works
        var identity = new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.Name, _currentUser.Email ?? string.Empty) },
            "CustomAuth");

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        if (!await _userManager.CheckPasswordAsync(user, password)) return false;

        // Verify that a linked Student exists
        var student = await _db.Students.FirstOrDefaultAsync(s => s.IdentityUserId == user.Id);
        if (student == null) return false;

        _currentUser = user;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return true;
    }

    public void Logout()
    {
        _currentUser = null;
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
