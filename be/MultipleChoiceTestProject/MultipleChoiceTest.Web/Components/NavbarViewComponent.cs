using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.Constants.Api;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Domain.ModelViews.Web;
using MultipleChoiceTest.Web.Api;

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
            return View(navbar);
        }
    }
}
