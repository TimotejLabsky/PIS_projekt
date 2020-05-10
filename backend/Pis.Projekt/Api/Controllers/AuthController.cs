using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pis.Projekt.Api.Requests;
using Pis.Projekt.Api.Responses;
using Pis.Projekt.Business.Authorization;

namespace Pis.Projekt.Api.Controllers
{
    [Microsoft.AspNetCore.Components.Route("auth")]
    [Controller]
    public class AuthController : Controller
    {
        public AuthController(AuthorizationService auth)
        {
            _auth = auth;
        }

        [HttpPost]
        public async Task<ActionResult<AuthLoginResponse>> LoginAsync(
            [FromBody] AuthLoginRequest request)
        {
            if (await _auth.LoginAsync(request.User, request.Password).ConfigureAwait(false))
            {
                return Ok(new AuthLoginResponse{User = request.User});
            }

            return Unauthorized();
        }


        private readonly AuthorizationService _auth;
    }
}