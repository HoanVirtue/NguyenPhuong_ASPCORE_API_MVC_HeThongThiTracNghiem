using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Web.Controllers.Guard;

namespace MultipleChoiceTest.Web.Areas.Admin.Controllers
{
    [Admin]
    public class DashboardController : BaseController
    {
        public DashboardController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger) : base(notyfService, httpContextAccessor, logger)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
