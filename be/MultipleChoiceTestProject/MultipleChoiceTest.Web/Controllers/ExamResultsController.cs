using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;
using MultipleChoiceTest.Web.Constants;
using MultipleChoiceTest.Web.Controllers.Guard;

namespace MultipleChoiceTest.Web.Controllers
{
    public class ExamResultsController : BaseController
    {
        public ExamResultsController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {
        }

        [HttpGet]
        [User]
        public async Task<IActionResult> Index()
        {
            var userId = ApiClient.GetCookie(Request, UserConstant.UserId);
            var history = await ApiClient.GetAsync<List<ExamResultItem>>(Request, $"ExamResults/GetByUsserID/{userId}");// "ExamResults/GetByUsserID/{userId}" hàm gọi api 

            return View(history.Data);
        }

        [HttpGet]
        public async Task<IActionResult> ExamResult(int id)
        {
            var examResult = await ApiClient.GetAsync<ExamResultItem>(Request, $"ExamResults/{id}");
            if (!examResult.Success)
                return NotFound();

            var questions = ApiClient.GetSession<List<QuestionItem>>(HttpContext.Session, SessionDataConstant.FormatKey(SessionDataConstant.ListQuestion, userCurrentId));
            var answers = ApiClient.GetSession<List<CandidateAnswer>>(HttpContext.Session, SessionDataConstant.FormatKey(SessionDataConstant.QuestionAnswer, userCurrentId));
            if ((questions == null || questions.Count == 0) || (answers == null || answers.Count == 0))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["ListQuestion"] = questions;
            ViewData["ListAnswer"] = answers;
            ViewData["Username"] = (await ApiClient.GetAsync<User>(Request, $"Users/{userCurrentId}")).Data.UserName;

            return View(examResult.Data);
        }
    }
}
