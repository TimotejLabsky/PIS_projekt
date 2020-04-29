using Microsoft.AspNetCore.Mvc;

namespace Pis.Projekt.Api.Controllers
{
    public class TaskController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return Ok();
        }
    }
}