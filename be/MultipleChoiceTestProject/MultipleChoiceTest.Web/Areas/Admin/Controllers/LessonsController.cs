using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultipleChoiceTest.Domain.Helpper;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;
using Newtonsoft.Json;

namespace MultipleChoiceTest.Web.Areas.Admin.Controllers
{
    public class LessonsController : BaseController
    {
        public LessonsController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchKey = "")
        {
            var lessonRs = await ApiClient.GetAsync<List<LessonItem>>(Request, "Lessons");

            if (lessonRs.Success)
            {
                var lessons = lessonRs.Data;
                if (!string.IsNullOrWhiteSpace(searchKey))
                {
                    searchKey = Utilities.RemoveDiacriticsAndToLower(searchKey);
                    lessons = lessons.Where(p => Utilities.IsSubstring(Utilities.RemoveDiacriticsAndToLower(p.LessonName), searchKey)).ToList();
                }
                ViewBag.SearchKey = searchKey;
                return View(lessons);
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
        public async Task<IActionResult> Create([Bind("Id,LessonName,SubjectId")] CULesson lesson)
        {
            if (ModelState.IsValid)
            {
                var createRs = await ApiClient.PostAsync<Lesson>(Request, "Lessons", JsonConvert.SerializeObject(lesson));
                if (createRs.Success)
                {
                    _notyfService.Success("Thêm dữ liệu thành công");
                    return RedirectToAction("Index", "Lessons");
                }
                else
                {
                    _notyfService.Warning(createRs.Message);
                }
                _notyfService.Error("Thêm dữ liệu thất bại");
                return View(lesson);
            }
            _notyfService.Error("Vui lòng nhập đầy đủ dữ liệu");
            await CreateViewBagAsync();
            return View(lesson);
        }

        // GET: Brand/Edit/Id
        public async Task<IActionResult> Edit(int id)
        {
            var detailRs = await ApiClient.GetAsync<Lesson>(Request, $"Lessons/{id}");
            var data = _mapper.Map<CULesson>(detailRs.Data);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,LessonName,SubjectId")] CULesson lesson)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updRs = await ApiClient.PutAsync<Lesson>(Request, $"Lessons", JsonConvert.SerializeObject(lesson));
                    if (updRs != null && updRs.Success)
                    {
                        _notyfService.Success("Cập nhật dữ liệu thành công");
                    }
                    else
                    {
                        _notyfService.Warning(updRs.Message);
                    }
                    return RedirectToAction("Index", "Lessons");
                }
                catch (Exception ex)
                {
                    _notyfService.Error("Đã có lỗi xảy ra vui lòng thử lại sau!");
                }
            }
            await CreateViewBagAsync(lesson);
            return View(lesson);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var delRs = await ApiClient.DeleteAsync<Lesson>(Request, $"Lessons/{id}");
                if (delRs.Success)
                {
                    _notyfService.Success("Xóa dữ liệu thành công");
                }
                else
                {
                    _notyfService.Error(delRs.Message);
                }
                return RedirectToAction("Index", "Lessons");

            }
            catch (Exception ex)
            {
                _notyfService.Error("Đã có lỗi xảy ra!");
                return RedirectToAction("Delete", "Lessons");
            }
        }


        private async Task CreateViewBagAsync(CULesson? lesson = null)
        {
            var subjects = await ApiClient.GetAsync<List<Subject>>(Request, "Subjects");
            if (lesson != null)
            {
                ViewData["Subjects"] = new SelectList(subjects.Data, "Id", "SubjectName", lesson.SubjectId);
            }
            else
            {
                ViewData["Subjects"] = new SelectList(subjects.Data, "Id", "SubjectName");
            }
        }
    }
}
