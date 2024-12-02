using Microsoft.AspNetCore.Mvc;

namespace MultipleChoiceTest.Web.Components
{
    public class RightPageViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string? type)
        {
            if (type == "ranking")
            {
                return View("Ranking");
            }
            return View("Access");
        }
    }
}
