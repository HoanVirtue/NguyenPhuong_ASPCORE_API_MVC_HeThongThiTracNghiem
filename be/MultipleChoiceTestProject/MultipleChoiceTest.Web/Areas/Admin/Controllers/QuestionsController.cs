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
    public class QuestionsController : BaseController
    {
        public QuestionsController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchKey = "")
        {
            var questionRs = await ApiClient.GetAsync<IEnumerable<QuestionItem>>(Request, "Questions/GetGridQuestions");

            if (questionRs.Success)
            {
                var questions = questionRs.Data;
                if (!string.IsNullOrWhiteSpace(searchKey))
                {
                    searchKey = Utilities.RemoveDiacriticsAndToLower(searchKey);
                    questions = questions.Where(p => Utilities.IsSubstring(Utilities.RemoveDiacriticsAndToLower(p.QuestionText), searchKey)).ToList();
                }
                ViewBag.SearchKey = searchKey;
                return View(questions);
            }
            this._notyfService.Error("Không có dữ liệu");
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
        public async Task<IActionResult> Create([Bind("Id,QuestionText,Choices,CorrectAnswer,AnswerExplanation,SubjectId,LessonId,QuestionTypeId,AudioFile")] CUQuestion question)
        {
            if (ModelState.IsValid)
            {
                var createRs = await ApiClient.PostAsync<Question>(Request, "Questions", JsonConvert.SerializeObject(question));
                if (createRs.Success)
                {
                    _notyfService.Success("Thêm dữ liệu thành công");
                    return RedirectToAction("Index", "Questions");
                }
                else
                {
                    _notyfService.Warning(createRs.Message);
                }
                _notyfService.Error("Thêm dữ liệu thất bại");
                return View(question);
            }
            _notyfService.Error("Vui lòng nhập đầy đủ dữ liệu");
            await CreateViewBagAsync();
            return View(question);
        }

        // GET: Brand/Edit/Id
        public async Task<IActionResult> Edit(int id)
        {
            var detailRs = await ApiClient.GetAsync<Question>(Request, $"Questions/{id}");
            var data = _mapper.Map<CUQuestion>(detailRs.Data);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,QuestionName,SubjectId")] CUQuestion question)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updRs = await ApiClient.PutAsync<Question>(Request, $"Questions", JsonConvert.SerializeObject(question));
                    if (updRs != null && updRs.Success)
                    {
                        _notyfService.Success("Cập nhật dữ liệu thành công");
                    }
                    else
                    {
                        _notyfService.Warning(updRs.Message);
                    }
                    return RedirectToAction("Index", "Questions");
                }
                catch (Exception ex)
                {
                    _notyfService.Error("Đã có lỗi xảy ra vui lòng thử lại sau!");
                }
            }
            await CreateViewBagAsync(question);
            return View(question);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var delRs = await ApiClient.DeleteAsync<Question>(Request, $"Questions/{id}");
                if (delRs.Success)
                {
                    _notyfService.Success("Xóa dữ liệu thành công");
                }
                else
                {
                    _notyfService.Error(delRs.Message);
                }
                return RedirectToAction("Index", "Questions");

            }
            catch (Exception ex)
            {
                _notyfService.Error("Đã có lỗi xảy ra!");
                return RedirectToAction("Delete", "Questions");
            }
        }


        private async Task CreateViewBagAsync(CUQuestion? question = null)
        {
            var subjects = await ApiClient.GetAsync<List<Subject>>(Request, "Subjects");
            var lessons = await ApiClient.GetAsync<List<Lesson>>(Request, "Lessons/GetAll");
            var types = await ApiClient.GetAsync<List<QuestionType>>(Request, "QuestionTypes");
            if (question != null)
            {
                ViewData["Subjects"] = new SelectList(subjects.Data, "Id", "SubjectName", question.SubjectId);
                ViewData["Lessons"] = new SelectList(lessons.Data, "Id", "LessonName", question.LessonId);
                ViewData["QuestionTypes"] = new SelectList(types.Data, "Id", "TypeName", question.QuestionTypeId);
            }
            else
            {
                ViewData["Subjects"] = new SelectList(subjects.Data, "Id", "SubjectName");
                ViewData["Lessons"] = new SelectList(lessons.Data, "Id", "LessonName");
                ViewData["QuestionTypes"] = new SelectList(types.Data, "Id", "TypeName");
            }
        }
    }
}
