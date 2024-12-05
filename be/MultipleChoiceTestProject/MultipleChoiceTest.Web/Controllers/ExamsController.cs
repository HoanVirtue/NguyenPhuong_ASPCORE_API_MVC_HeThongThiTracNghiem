using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.Constants.Api;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;
using MultipleChoiceTest.Web.Constants;
using MultipleChoiceTest.Web.Controllers.Guard;

namespace MultipleChoiceTest.Web.Controllers
{
    public class ExamsController : BaseController
    {
        public ExamsController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {

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

        [User]
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
                var userId = ApiClient.GetCookie(Request, UserConstant.UserId);
                ViewData["Username"] = (await ApiClient.GetAsync<User>(Request, $"Users/{userId}")).Data.UserName;
                return View(infoExam.Data);
            }
            return BadRequest();
        }


        public IActionResult StartExam()
        {
            return View();
        }
    }
}
