using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Primitives;

namespace Tasks_Handler.Services.TokenService
{
    public class TokenManager : ITokenManager
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenManager(IDistributedCache cache, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;

        }
        public async Task DeactivateAsync(string token)
             => await _cache.SetStringAsync(GetKey(token),
                 " ", new DistributedCacheEntryOptions
                 {
                     AbsoluteExpirationRelativeToNow =
                         TimeSpan.FromMinutes(1440)
                 });
        public async Task<bool> IsCurrentActiveToken()
            => await IsActiveAsync(GetCurrentAsync());

        public async Task DeactivateCurrentAsync()
            => await DeactivateAsync(GetCurrentAsync());
        public async Task<bool> IsActiveAsync(string token)
            => await _cache.GetStringAsync(GetKey(token)) == null;
        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
        }
        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";
    }
}