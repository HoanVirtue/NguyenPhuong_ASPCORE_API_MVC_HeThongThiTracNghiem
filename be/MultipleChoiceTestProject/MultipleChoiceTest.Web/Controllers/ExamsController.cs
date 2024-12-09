using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MultipleChoiceTest.Domain.Constants.Api;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;
using MultipleChoiceTest.Web.Constants;
using MultipleChoiceTest.Web.Controllers.Guard;

namespace MultipleChoiceTest.Web.Controllers
{
    [User]
    public class ExamsController : BaseController
    {
        private string userCurrentId;
        public ExamsController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            userCurrentId = ApiClient.GetCookie(Request, UserConstant.UserId);
        }

        [HttpGet]
        public async Task<IActionResult> InfoExamByLesson(int lessonId, int? pageIndex = 0, int? pageSize = 10)
        {
            if (lessonId != null && lessonId > 0)
            {
                var examPage = await ApiClient.GetAsync<Pagination<ExamItem>>(Request, $"Exams/GetExamByLesson/{lessonId}?type={(int)TypeGetSelectConstant.TypeGet.GetGrid}&pageIndex={pageIndex}&pageSize={pageSize}");
                if (examPage.Data.Items == null || examPage.Data.Items.Count == 0)
                {
                    _notyfService.Warning("Không có dữ liệu");
                    return RedirectToAction("Index", "Home");
                }
                if (!examPage.Success)
                {
                    _notyfService.Warning(examPage.Message);
                }
                ViewData["PageIndex"] = pageIndex;
                return View(examPage.Data);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> InfoExamBySubject(int subjectId, int? pageIndex = 0, int? pageSize = 10)
        {
            if (subjectId != null && subjectId > 0)
            {
                var examPage = await ApiClient.GetAsync<Pagination<ExamItem>>(Request, $"Exams/GetExamBySubject/{subjectId}?type={(int)TypeGetSelectConstant.TypeGet.GetGrid}&pageIndex={pageIndex}&pageSize={pageSize}");
                if (examPage.Data.Items == null || examPage.Data.Items.Count == 0)
                {
                    _notyfService.Warning("Không có dữ liệu");
                    return RedirectToAction("Index", "Home");
                }
                if (!examPage.Success)
                {
                    _notyfService.Warning(examPage.Message);
                }
                ViewData["PageIndex"] = pageIndex;
                return View(examPage.Data);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> InfoExam(int id)
        {
            if (id != null && id > 0)
            {
                var infoExam = await ApiClient.GetAsync<ExamItem>(Request, $"Exams/{id}?type={(int)TypeGetSelectConstant.TypeGetDetail.GetDetail}");
                if (!infoExam.Success)
                {
                    return NotFound();
                }

                ViewData["Username"] = (await ApiClient.GetAsync<User>(Request, $"Users/{userCurrentId}")).Data.UserName;
                return View(infoExam.Data);
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> StartExam(int id)
        {
            if (id != null && id > 0)
            {
                var infoExam = await ApiClient.GetAsync<ExamItem>(Request, $"Exams/{id}?type={(int)TypeGetSelectConstant.TypeGetDetail.GetDetail}");
                if (!infoExam.Success)
                {
                    //return Json(new ApiResponse<string>
                    //{
                    //    Success = false,
                    //    Message = "Không tìm thấy bài thi"
                    //});
                    return NotFound();
                }

                var questions = await ApiClient.GetAsync<List<QuestionItem>>(Request, $"Exams/GetQuestionByExam/{id}");
                if (!questions.Success)
                {
                    //return Json(questions);
                    _notyfService.Warning(questions.Message);
                    return RedirectToAction("InfoExam", "Exams", new { id = id });
                }
                ApiClient.SetSession<List<QuestionItem>>(HttpContext.Session, SessionDataConstant.FormatKey(SessionDataConstant.ListQuestion, userCurrentId), questions.Data);
                ViewData["Questions"] = questions.Data;
                ViewData["Username"] = (await ApiClient.GetAsync<User>(Request, $"Users/{userCurrentId}")).Data.UserName;
                return View(infoExam.Data);
            }
            //return Json(new ApiResponse<string>
            //{
            //    Success = false,
            //    Message = "Không tìm thấy bài thi"
            //});
            return NotFound();
        }

        public PartialViewResult UserQuestionAnswer(int index)
        {
            //var cadAnswer = ApiClient.GetSession<List<CandidateAnswer>>(HttpContext.Session, SessionDataConstant.FormatKey(SessionDataConstant.QuestionAnswer, userCurrentId));
            var questions = ApiClient.GetSession<List<QuestionItem>>(HttpContext.Session, SessionDataConstant.FormatKey(SessionDataConstant.ListQuestion, userCurrentId));
            var question = questions.FirstOrDefault(x => x.Index == index);
            return PartialView("~/Views/Shared/QuestionView/_QuestionMultipleChoice.cshtml", question);
        }

        [HttpPost]
        public IActionResult SubmitExam([FromBody] List<CandidateAnswer> answers)
        {
            if (answers == null || !answers.Any())
            {
                return BadRequest("Không có câu trả lời nào");
            }

            var questionItems = ApiClient.GetSession<List<QuestionItem>>(HttpContext.Session, SessionDataConstant.FormatKey(SessionDataConstant.ListQuestion, userCurrentId));
            foreach (var answer in answers)
            {
                answer.QuestionId = questionItems.FirstOrDefault(x => x.Index == answer.QuestionIndex).Id;
            }
            return default;
        }
    }
}
