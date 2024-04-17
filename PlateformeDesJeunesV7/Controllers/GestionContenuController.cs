using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class GestionContenuController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
