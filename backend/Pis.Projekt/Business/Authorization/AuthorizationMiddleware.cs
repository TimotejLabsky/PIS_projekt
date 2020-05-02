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

        public Task Invoke(HttpContext httpContext)
        {
            var headers = httpContext.Request.Headers;
            _service.Login(headers["user_id"], headers["password"]);
            return _next(httpContext);
        }
        private readonly AuthorizationService _service;
    }
}