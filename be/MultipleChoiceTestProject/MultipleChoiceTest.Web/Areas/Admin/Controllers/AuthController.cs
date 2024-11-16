using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;
using MultipleChoiceTest.Web.Constants;
using Newtonsoft.Json;

namespace MultipleChoiceTest.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
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
                string role = Request.Cookies[UserConstant.Role];
                if (!string.IsNullOrEmpty(role) && role == TypeUserConstant.Role.ADMIN.ToString())
                {
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(Domain.ModelViews.Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await ApiClient.PostAsync<LoginResponse>(Request, "Auth/Login", JsonConvert.SerializeObject(model));
                if (result.Success)
                {
                    if (result.Data.User.IsAdmin == true)
                    {
                        LoginSuccess(result.Data);
                        this._notyfService.Success("Đăng nhập thành công!");
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                else
                {
                    this._notyfService.Error("Sai tên đăng nhập hoặc mật khẩu");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    this._notyfService.Error(error.ErrorMessage);
                }
            }
            return View(model);
        }

        [Admin]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(UserConstant.AccessToken);
            Response.Cookies.Delete(UserConstant.Role);
            Response.Cookies.Delete(UserConstant.UserId);
            Response.Cookies.Delete(UserConstant.AccountName);

            return RedirectToAction("Login", "Auth");
        }
        public void LoginSuccess(LoginResponse loginRes)
        {
            ApiClient.SetCookie(Response, UserConstant.AccessToken, loginRes.AccessToken);
            ApiClient.SetCookie(Response, UserConstant.Role, loginRes.User?.IsAdmin == true ? "1" : "0");
            ApiClient.SetCookie(Response, UserConstant.UserId, loginRes.User.Id.ToString() ?? "");
            ApiClient.SetCookie(Response, UserConstant.AccountName, loginRes.User.Email);
        }
    }
}
