using Microsoft.AspNetCore.Mvc;

namespace MultipleChoiceTest.Web.Components
{
    public class RightPageViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
