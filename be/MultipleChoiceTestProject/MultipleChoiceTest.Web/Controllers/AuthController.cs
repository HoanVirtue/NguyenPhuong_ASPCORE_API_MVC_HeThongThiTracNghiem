using Microsoft.AspNetCore.Mvc;

namespace MultipleChoiceTest.Web.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
