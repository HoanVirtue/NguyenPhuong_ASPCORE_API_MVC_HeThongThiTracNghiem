using Microsoft.AspNetCore.Mvc;

namespace MultipleChoiceTest.Web.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Contact()
        {
            return View();
        }
    }
}
