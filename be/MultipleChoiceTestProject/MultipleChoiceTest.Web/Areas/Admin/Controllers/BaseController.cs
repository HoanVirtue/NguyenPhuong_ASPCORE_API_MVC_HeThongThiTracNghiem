using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Web.Controllers.Guard;

namespace MultipleChoiceTest.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Admin]
    public class BaseController : Controller
    {
        protected readonly INotyfService _notyfService;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILogger<BaseController> _logger;
        public BaseController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
        {
            _notyfService = notyfService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
    }
}
