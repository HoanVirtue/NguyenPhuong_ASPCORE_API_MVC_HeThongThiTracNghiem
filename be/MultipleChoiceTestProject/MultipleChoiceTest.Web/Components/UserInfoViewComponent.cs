using Microsoft.AspNetCore.Mvc;

namespace MultipleChoiceTest.Web.Components
{
    public class UserInfoViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
