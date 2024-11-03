using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MultipleChoiceTest.Web.Constants;

namespace MultipleChoiceTest.Web.Areas.Admin.Controllers
{
	public class AuthController : Controller
	{
		protected readonly INotyfService _notyfService;
		protected readonly IHttpContextAccessor _httpContextAccessor;
		protected readonly ILogger<BaseController> _logger;
		public AuthController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
		{
			_notyfService = notyfService;
			_httpContextAccessor = httpContextAccessor;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult Login()
		{
			if (Request.Cookies.ContainsKey(UserConstant.AccessToken))
			{
				// Lấy giá trị của cookie "role"
				string role = Request.Cookies[UserConstant.Role];

				// Kiểm tra xem giá trị của cookie "role" có phải là "admin" không
				if (role == TypeUserConstant.TYPEUSER_ADMIN)
				{
					// Chuyển hướng đến trang Dashboard
					return RedirectToAction("Index", "Dashboard");
				}
			}
			return View();
		}


		[HttpPost]
		public IActionResult Login(Login model)
		{
			return default;
		}
	}
}
