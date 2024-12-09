using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;
using MultipleChoiceTest.Web.Constants;
using MultipleChoiceTest.Web.Controllers.Guard;

namespace MultipleChoiceTest.Web.Controllers
{
    public class UserController : Controller
    {
        [HttpGet]
        [User]
        public async Task<IActionResult> Index()
        {
            var userID = ApiClient.GetCookie(Request, UserConstant.UserId);
            var account = await ApiClient.GetAsync<UserItem>(Request, $"Users/{userID}");
            return View(account.Data);
        }
        public IActionResult UserBasic()
        {
            return View();
        }

    }
}
