using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Web.Controllers.Guard;

namespace MultipleChoiceTest.Web.Areas.Admin.Controllers
{
    [Admin]
    public class DashboardController : BaseController
    {
        public DashboardController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
