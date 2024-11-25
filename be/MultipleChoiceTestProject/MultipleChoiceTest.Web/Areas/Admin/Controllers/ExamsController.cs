using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultipleChoiceTest.Domain.Helpper;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            var exams = await ApiClient.GetAsync<List<ExamItem>>(Request, "Exams");

            if (exams.Success)
            {
                var exam = exams.Data;
                if (!string.IsNullOrWhiteSpace(searchKey))
                {
                    searchKey = Utilities.RemoveDiacriticsAndToLower(searchKey);
                    exam = exam.Where(p => Utilities.IsSubstring(Utilities.RemoveDiacriticsAndToLower(p.ExamName), searchKey)).ToList();
                }
                ViewBag.SearchKey = searchKey;
                return View(exam);
            }
            this._notyfService.Error("Retrieving the list of type failed");
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await CreateViewBagAsync();
            return View();
        }
        // POST: Brand/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,ExamName,Duration,TotalQuestions,SubjectId,LessonId")] CUExam exam)
        {
            if (ModelState.IsValid)
            {
                var createRs = await ApiClient.PostAsync<Exam>(Request, "Exams", JsonConvert.SerializeObject(exam));
                if (createRs.Success)
                {
                    _notyfService.Success("Thêm dữ liệu thành công");
                    return RedirectToAction("Index", "Exams");
                }
                else
                {
                    _notyfService.Warning(createRs.Message);
                }
                _notyfService.Error("Thêm dữ liệu thất bại");
                await CreateViewBagAsync(exam);
                return View(exam);
            }
            _notyfService.Error("Vui lòng nhập đầy đủ dữ liệu");
            await CreateViewBagAsync();
            return View(exam);
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
        // POST: Brand/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExamName,Duration,TotalQuestions,SubjectId,LessonId")] CUExam exam)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updRs = await ApiClient.PutAsync<Exam>(Request, $"Exams", JsonConvert.SerializeObject(exam));
                    if (updRs != null && updRs.Success)
                    {
                        _notyfService.Success("Cập nhật dữ liệu thành công");
                        return RedirectToAction("Index", "Exams");
                    }
                    else
                    {
                        _notyfService.Warning(updRs.Message);
                    }
                    await CreateViewBagAsync(exam);
                    return View(exam);
                }
                catch (Exception ex)
                {
                    _notyfService.Error("Đã có lỗi xảy ra vui lòng thử lại sau!");
                }
            }
            await CreateViewBagAsync(exam);
            return View(exam);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var delRs = await ApiClient.DeleteAsync<Exam>(Request, $"Exams/{id}");
                if (delRs.Success)
                {
                    _notyfService.Success("Xóa dữ liệu thành công");
                }
                else
                {
                    _notyfService.Error(delRs.Message);
                }
                return RedirectToAction("Index", "Exams");

            }
            catch (Exception ex)
            {
                _notyfService.Error("Đã có lỗi xảy ra!");
                return RedirectToAction("Delete", "Exams");
            }
        }
        private async Task CreateViewBagAsync(CUExam? exam = null)
        {
            var subjects = await ApiClient.GetAsync<List<Subject>>(Request, "Subjects");
            if (exam != null)
            {
                var lessons = await ApiClient.GetAsync<List<Lesson>>(Request, $"Lessons/GetBySubjectId/{exam.SubjectId}");

                ViewData["Subjects"] = new SelectList(subjects.Data, "Id", "SubjectName", exam.SubjectId);
                ViewData["Lessons"] = new SelectList(lessons.Data, "Id", "LessonName", exam.LessonId);
            }
            else
            {
                ViewData["Subjects"] = new SelectList(subjects.Data, "Id", "SubjectName");
            }
        }
    }
}
