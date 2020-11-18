using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Tasks_Handler.Services.TokenService;

namespace Tasks_Handler.Services
{
    public class TokenManagerMiddleware : IMiddleware
    {
       private readonly ITokenManager _tokenManager;

        public TokenManagerMiddleware(ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
        }
        
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await _tokenManager.IsCurrentActiveToken())
            {
                await next(context);
                
                return;
            }else if(context.Request.Path.Value.Contains("/auth/login") || context.Request.Path.Value.Contains("/auth/register"))
            {
                await next.Invoke(context);
                return;
            }
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
        }
    }
}