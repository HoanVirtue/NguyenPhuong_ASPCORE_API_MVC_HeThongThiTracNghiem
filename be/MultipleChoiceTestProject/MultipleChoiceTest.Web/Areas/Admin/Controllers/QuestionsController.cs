﻿using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultipleChoiceTest.Domain.Helpper;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;
using MultipleChoiceTest.Web.Constants;
using Newtonsoft.Json;

namespace MultipleChoiceTest.Web.Areas.Admin.Controllers
{
    public class QuestionsController : BaseController
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;

        public QuestionsController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment,
            INotyfService notyfService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<BaseController> logger,
            IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {
            _environment = hostingEnvironment;
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
        public async Task<IActionResult> Create([Bind("Id,QuestionText,Choices,CorrectAnswer,AnswerExplanation,SubjectId,LessonId,QuestionTypeId")] CUQuestion question, IFormFile? AudioFile)
        {
            if (ModelState.IsValid)
            {
                #region Validate
                string errors = ValidateData(question, AudioFile);
                if (!string.IsNullOrEmpty(errors))
                {
                    ModelState.AddModelError("error", errors);
                    return View(question);
                }
                #endregion

                #region import file
                question.AudioFilePath = await UploadAudioAsync(AudioFile);
                #endregion

                dynamic choices = Utilities.FormatOptions(question.Choices);
                if (!string.IsNullOrEmpty(choices.Data))
                    question.Choices = choices.Data;
                else
                {
                    ModelState.AddModelError("error", choices.Message);
                    return View(question);
                }

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
                await CreateViewBagAsync();
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
            //var lessons = await ApiClient.GetAsync<List<Lesson>>(Request, "Lessons");
            var types = await ApiClient.GetAsync<List<QuestionType>>(Request, "QuestionTypes");
            if (question != null)
            {
                ViewData["Subjects"] = new SelectList(subjects.Data, "Id", "SubjectName", question.SubjectId);
                //ViewData["Lessons"] = new SelectList(lessons.Data, "Id", "LessonName", question.LessonId);
                ViewData["QuestionTypes"] = new SelectList(types.Data, "Id", "TypeName", question.QuestionTypeId);
            }
            else
            {
                ViewData["Subjects"] = new SelectList(subjects.Data, "Id", "SubjectName");
                //ViewData["Lessons"] = new SelectList(lessons.Data, "Id", "LessonName");
                ViewData["QuestionTypes"] = new SelectList(types.Data, "Id", "TypeName");
            }
        }

        private string ValidateData(CUQuestion question, IFormFile? audioFile)
        {
            string errors = "";
            if (question.QuestionTypeId == (int)QuestionTypeConstant.Type.MultipleChoice)
            {
                if (string.IsNullOrEmpty(question.QuestionText))
                {
                    errors = string.IsNullOrEmpty(errors)
                    ? "Câu hỏi không được bỏ trống"
                        : $"{errors}, Câu hỏi không được bỏ trống";
                }
                if (string.IsNullOrEmpty(question.Choices))
                {
                    errors = string.IsNullOrEmpty(errors)
                    ? "Lựa chọn không được bỏ trống"
                        : $"{errors}, Lựa chọn không được bỏ trống";
                }
            }
            else if (question.QuestionTypeId == (int)QuestionTypeConstant.Type.Essay)
            {
                if (string.IsNullOrEmpty(question.QuestionText))
                {
                    errors = string.IsNullOrEmpty(errors)
                    ? "Câu hỏi không được bỏ trống"
                        : $"{errors}, Câu hỏi không được bỏ trống";
                }
                if (string.IsNullOrEmpty(question.AnswerExplanation))
                {
                    errors = string.IsNullOrEmpty(errors)
                    ? "Đáp án tự luận không được bỏ trống"
                        : $"{errors}, Đáp án tự luận không được bỏ trống";
                }
            }
            else
            {
                if (audioFile == null || audioFile.Length == 0)
                {
                    errors = string.IsNullOrEmpty(errors)
                    ? "File audio không được bỏ trống"
                        : $"{errors}, File audio không được bỏ trống";
                }
                if (string.IsNullOrEmpty(question.Choices))
                {
                    errors = string.IsNullOrEmpty(errors)
                    ? "Lựa chọn không được bỏ trống"
                        : $"{errors}, Lựa chọn không được bỏ trống";
                }
            }
            return errors;
        }

        private async Task<string> UploadAudioAsync(IFormFile? audioFile)
        {
            if (audioFile != null && audioFile.Length > 0)
            {
                string path = Path.Combine(this._environment.WebRootPath, "Audios");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(audioFile.FileName);
                string extension = Path.GetExtension(audioFile.FileName);
                string fullPath = Path.Combine(path, fileNameWithoutExtension + extension);
                if (System.IO.File.Exists(fullPath))
                {
                    int index = 1;
                    while (true)
                    {
                        fileNameWithoutExtension = Path.GetFileName(audioFile.FileName) + index;
                        fullPath = Path.Combine(path, fileNameWithoutExtension + extension);
                        if (System.IO.File.Exists(fullPath))
                        {
                            index++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    await audioFile.CopyToAsync(stream);
                }

                return fileNameWithoutExtension + extension;
            }
            return null;
        }
    }
}
