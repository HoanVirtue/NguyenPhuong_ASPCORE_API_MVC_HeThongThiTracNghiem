using Microsoft.AspNetCore.Mvc;

namespace MultipleChoiceTest.Web.Controllers
{
    public class ExamResultsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
