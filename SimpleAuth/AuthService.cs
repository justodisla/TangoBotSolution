using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TangoBotApi.Services.Auth;

namespace TangoBot.Infrastructure.AuthImpl
{
    /// <summary>
    /// Implements the <see cref="IAuthService"/> interface to provide authentication and authorization functionalities.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly Dictionary<string, string> _users = new()
        {
            { "user1", "password1" },
            { "user2", "password2" }
        };

        private readonly Dictionary<string, string> _tokens = new();

        public async Task<bool> AuthenticateAsync(string username, string password)
        {
            await Task.Delay(100); // Simulate async operation
            return _users.TryGetValue(username, out var storedPassword) && storedPassword == password;
        }

        public async Task<string> GenerateTokenAsync(string username)
        {
            await Task.Delay(100); // Simulate async operation
            var token = Guid.NewGuid().ToString();
            _tokens[username] = token;
            return token;
        }

        public async Task<bool> AuthorizeAsync(string token, string requiredRole)
        {
            await Task.Delay(100); // Simulate async operation
            return _tokens.ContainsValue(token);
        }

        public string[] Requires()
        {
            throw new NotImplementedException();
        }

        public void Setup(Dictionary<string, object> configuration)
        {
            throw new NotImplementedException();
        }
    }
}

