using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.Constants.Api;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Domain.ModelViews.Web;
using MultipleChoiceTest.Web.Api;
using MultipleChoiceTest.Web.Constants;

namespace MultipleChoiceTest.Web.Components
{
    public class NavbarViewComponent : ViewComponent
    {
        public NavbarViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            NavbarModelView navbar = new NavbarModelView()
            {
                Subjects = (await ApiClient.GetAsync<List<SubjectItem>>(Request, $"Subjects?type={(int)TypeGetSelectConstant.TypeGet.GetList}")).Data,
                Lessons = (await ApiClient.GetAsync<List<LessonItem>>(Request, "Lessons")).Data
            };
            var userId = ApiClient.GetCookie(Request, UserConstant.UserId);
            if (userId != null)
            {
                var user = (await ApiClient.GetAsync<UserItem>(Request, $"Users/{userId}?type={(int)TypeGetSelectConstant.TypeGetDetail.GetDetail}"));
                if (user.Success)
                {
                    ViewData["Username"] = user.Data.UserName;
                    ViewData["UserId"] = userId;
                }
            }
            return View(navbar);
        }
    }
}
