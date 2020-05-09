using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Pis.Projekt.Business.Authorization
{
    // ReSharper disable once ClassNeverInstantiated.Global USed by DI
    public class AuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleware(RequestDelegate next, AuthorizationService service)
        {
            _next = next;
            _service = service;
        }

        public async Task Invoke(HttpContext context)
        {
            var headers = context.Request.Headers;
            var isSuccess = await _service.LoginAsync(headers["user_id"], headers["password"])
                .ConfigureAwait(false);
            if (isSuccess)
            {
                await _next.Invoke(context);
            }
            else
            {
                context.Response.Clear();
                context.Response.ContentType = "text/plain";
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
            }
        }

        private readonly AuthorizationService _service;
    }
}