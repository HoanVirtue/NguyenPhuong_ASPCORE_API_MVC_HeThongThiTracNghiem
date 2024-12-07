using Microsoft.AspNetCore.Mvc;

namespace MultipleChoiceTest.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UserBasic()
        {
            return View();
        }

    }
}
