using Microsoft.AspNetCore.Mvc;

namespace MultipleChoiceTest.Web.Controllers
{
    public class ExamsController : Controller
    {
        public IActionResult InfoExam()
        {
            return View();
        }
        public IActionResult StartExam() {
            return View();
        }
    }
}
