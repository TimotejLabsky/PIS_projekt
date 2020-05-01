using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Pis.Projekt.Business.Authorization
{
    public class AuthorizationMiddleWare
    {
        private readonly RequestDelegate _next;

        public AuthorizationMiddleWare(RequestDelegate next, AuthorizationService service)
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