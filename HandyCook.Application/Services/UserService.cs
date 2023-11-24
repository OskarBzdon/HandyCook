using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace HandyCook.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public UserService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<string> GetCurrentUserIdAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;
            return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
