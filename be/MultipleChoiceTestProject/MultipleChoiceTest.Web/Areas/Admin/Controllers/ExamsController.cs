using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;

namespace MultipleChoiceTest.Web.Areas.Admin.Controllers
{
    public class ExamsController : BaseController
    {
        public ExamsController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchKey = "")
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await CreateViewBagAsync();
            return View();
        }

        // GET: Brand/Edit/Id
        public async Task<IActionResult> Edit(int id)
        {
            var detailRs = await ApiClient.GetAsync<Exam>(Request, $"Exams/{id}");
            var data = _mapper.Map<CUExam>(detailRs.Data);
            if (detailRs.Success)
            {
                await CreateViewBagAsync(data);
                return View(data);
            }

            _notyfService.Error("Không tìm thấy môn học");
            await CreateViewBagAsync(data);
            return View(data);
        }

        private async Task CreateViewBagAsync(CUExam? exam = null)
        {
            var subjects = await ApiClient.GetAsync<List<Subject>>(Request, "Subjects");
            if (exam != null)
            {
                ViewData["Subjects"] = new SelectList(subjects.Data, "Id", "SubjectName", exam.SubjectId);
            }
            else
            {
                ViewData["Subjects"] = new SelectList(subjects.Data, "Id", "SubjectName");
            }
        }
    }
}
