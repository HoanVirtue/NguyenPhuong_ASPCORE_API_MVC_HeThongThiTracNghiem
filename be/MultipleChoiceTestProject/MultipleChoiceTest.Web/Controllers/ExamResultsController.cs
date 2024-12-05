using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;
using MultipleChoiceTest.Web.Constants;
using MultipleChoiceTest.Web.Controllers.Guard;

namespace MultipleChoiceTest.Web.Controllers
{
    public class ExamResultsController : Controller
    {
        [HttpGet]
        [User]
        public async Task<IActionResult> Index()
        {
            var userId = ApiClient.GetCookie(Request, UserConstant.UserId);
            var history = await ApiClient.GetAsync<List<ExamResultItem>>(Request, $"ExamResults/GetByUsserID/{userId}");

            return View(history.Data);
        }
       
    }
}
