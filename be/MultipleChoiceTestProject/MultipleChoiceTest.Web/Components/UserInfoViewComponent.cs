using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Web.Api;
using MultipleChoiceTest.Web.Constants;

namespace MultipleChoiceTest.Web.Components
{
    public class UserInfoViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = ApiClient.GetCookie(Request, UserConstant.UserId);
            var user = (await ApiClient.GetAsync<User>(Request, $"Users/{userId}"));
            return View(user.Data);
        }
    }
}
