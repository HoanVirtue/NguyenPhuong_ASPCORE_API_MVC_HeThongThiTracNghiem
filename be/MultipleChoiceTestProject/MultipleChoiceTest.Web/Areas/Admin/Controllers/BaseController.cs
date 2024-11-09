using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
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
        protected readonly IMapper _mapper;
        public BaseController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper)
        {
            _notyfService = notyfService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _mapper = mapper;
        }
    }
}
