using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MultipleChoiceTest.Domain;
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
        [HttpPost]
        public async Task<IActionResult> ImportFromExcel(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                List<ImportQuestionMessage> questionRes = await ParseExcelFileAsync(file.OpenReadStream());
                if (questionRes != null && questionRes.Count > 0)
                {
                    TempData["error"] = string.Join("", questionRes.Select(x => x.Message));
                }
                else
                {
                    _notyfService.Success("Upload câu hỏi thành công");
                }
                return RedirectToAction("Index", "Questions");
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> Index(string searchKey = "")
        {
            var questionRs = await ApiClient.GetAsync<IEnumerable<QuestionItem>>(Request, "Questions/GetGridQuestions");

            string message = TempData["error"] as string;
            if (!string.IsNullOrEmpty(message))
            {
                message = message.Replace("\n", "<br/>");
                ViewData["error"] = message;
            }
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
                    await CreateViewBagAsync(question);
                    return View(question);
                }
                #endregion

                #region import file
                if (question.QuestionTypeId == (int)QuestionTypeConstant.Type.Audio)
                {
                    question.AudioFilePath = await UploadAudioAsync(AudioFile);
                }
                #endregion

                if (question.QuestionTypeId == (int)QuestionTypeConstant.Type.MultipleChoice || question.QuestionTypeId == (int)QuestionTypeConstant.Type.Audio)
                {
                    var choices = Utilities.FormatOptions(question.Choices);
                    if (choices.Success)
                        question.Choices = choices.Data;
                    else
                    {
                        ModelState.AddModelError("error", choices.Message);
                        await CreateViewBagAsync(question);
                        return View(question);
                    }
                }

                var createRs = await ApiClient.PostAsync<Domain.Models.Question>(Request, "Questions", JsonConvert.SerializeObject(question));
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
                await CreateViewBagAsync(question);
                return View(question);
            }
            _notyfService.Error("Vui lòng nhập đầy đủ dữ liệu");
            await CreateViewBagAsync();
            return View(question);
        }

        // GET: Brand/Edit/Id
        public async Task<IActionResult> Edit(int id)
        {
            var detailRs = await ApiClient.GetAsync<Domain.Models.Question>(Request, $"Questions/GetDetail/{id}");
            var data = _mapper.Map<CUQuestion>(detailRs.Data);
            if (detailRs.Success)
            {
                await CreateViewBagAsync(data);
                if (data.QuestionTypeId != (int)QuestionTypeConstant.Type.Essay)
                {
                    data.Choices = Utilities.ConvertToTextArea(data.Choices);
                }
                return View(data);
            }

            _notyfService.Error("Không tìm thấy câu hỏi");
            await CreateViewBagAsync(data);
            return View(data);
        }

        // POST: Brand/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,QuestionText,Choices,CorrectAnswer,AnswerExplanation,SubjectId,LessonId,QuestionTypeId")] CUQuestion question, IFormFile? AudioFile)
        {
            if (ModelState.IsValid)
            {
                #region Validate
                string errors = ValidateData(question, AudioFile);
                if (!string.IsNullOrEmpty(errors))
                {
                    ModelState.AddModelError("error", errors);
                    await CreateViewBagAsync(question);
                    return View(question);
                }
                #endregion

                #region import file
                if (question.QuestionTypeId == (int)QuestionTypeConstant.Type.Audio && (AudioFile != null && AudioFile.Length > 0))
                {
                    question.AudioFilePath = await UploadAudioAsync(AudioFile);
                }
                #endregion

                if (question.QuestionTypeId == (int)QuestionTypeConstant.Type.MultipleChoice || question.QuestionTypeId == (int)QuestionTypeConstant.Type.Audio)
                {
                    var choices = Utilities.FormatOptions(question.Choices);
                    if (choices.Success)
                        question.Choices = choices.Data;
                    else
                    {
                        ModelState.AddModelError("error", choices.Message);
                        await CreateViewBagAsync(question);
                        return View(question);
                    }
                }

                try
                {
                    var updRs = await ApiClient.PutAsync<Domain.Models.Question>(Request, $"Questions", JsonConvert.SerializeObject(question));
                    if (updRs != null && updRs.Success)
                    {
                        _notyfService.Success("Cập nhật dữ liệu thành công");
                        return RedirectToAction("Index", "Questions");
                    }
                    else
                    {
                        _notyfService.Warning(updRs.Message);
                        await CreateViewBagAsync(question);
                        return View(question);
                    }
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
                var delRs = await ApiClient.DeleteAsync<Domain.Models.Question>(Request, $"Questions/{id}");
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

        [HttpGet("Details")]
        public async Task<IActionResult> Details(int id)
        {
            var question = await ApiClient.GetAsync<QuestionItem>(Request, $"Questions/GetDetail/{id}");
            if (question.Success)
            {
                return View(question.Data);
            }
            return BadRequest(question.Message);
        }


        private async Task CreateViewBagAsync(CUQuestion? question = null)
        {
            var subjects = await ApiClient.GetAsync<List<Subject>>(Request, "Subjects");
            var types = await ApiClient.GetAsync<List<QuestionType>>(Request, "QuestionTypes");
            if (question != null)
            {
                var lessons = await ApiClient.GetAsync<List<Lesson>>(Request, $"Lessons/GetBySubjectId/{question.SubjectId}");

                ViewData["Subjects"] = new SelectList(subjects.Data, "Id", "SubjectName", question.SubjectId);
                ViewData["Lessons"] = new SelectList(lessons.Data, "Id", "LessonName", question.LessonId);
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
                if (string.IsNullOrEmpty(question.CorrectAnswer))
                {
                    errors = string.IsNullOrEmpty(errors)
                    ? "Đáp án trắc nghiệm không được bỏ trống"
                        : $"{errors}, Đáp án trắc nghiệm không được bỏ trống";
                }
                question.AnswerExplanation = null;
                question.AudioFilePath = null;
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

                question.Choices = null;
                question.CorrectAnswer = null;
                question.AudioFilePath = null;
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
                if (string.IsNullOrEmpty(question.CorrectAnswer))
                {
                    errors = string.IsNullOrEmpty(errors)
                    ? "Đáp án trắc nghiệm không được bỏ trống"
                        : $"{errors}, Đáp án trắc nghiệm không được bỏ trống";
                }
                question.QuestionText = null;
                question.AnswerExplanation = null;
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
                        fileNameWithoutExtension = $"{Path.GetFileNameWithoutExtension(audioFile.FileName)}_{index}";
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

        public async Task<List<ImportQuestionMessage>> ParseExcelFileAsync(Stream stream)
        {
            List<ImportQuestionMessage> errorList = new List<ImportQuestionMessage>();
            using (var workbook = new XLWorkbook(stream))
            {

                var worksheet = workbook.Worksheet(1);
                // bỏ qua dòng tiêu đề lấy từ dòng 2

                var rows = worksheet.RangeUsed().RowsUsed().Skip(1);
                string errors = "";
                bool isCreate = true;
                foreach (var row in rows)
                {
                    errors = "";
                    isCreate = true;
                    string questionText = row.Cell(2).GetValue<string>().ResolveExcelValue();
                    string choices = row.Cell(3).GetValue<string>().ResolveExcelValue();
                    string correctAnswer = row.Cell(4).GetValue<string>().ResolveExcelValue();
                    string answerexplanation = row.Cell(5).GetValue<string>().ResolveExcelValue();
                    string questionType = row.Cell(6).GetValue<string>().ResolveExcelValue();
                    string lessonCode = row.Cell(7).GetValue<string>().ResolveExcelValue();
                    //string subjectCode= row.Cell(8).GetValue<string>().ResolveExcelValue();

                    #region validate
                    if (!QuestionTypeExists(questionType))
                    {
                        errors = string.IsNullOrEmpty(errors)
                            ? "Lựa chọn không được bỏ trống"
                                : $"{errors}, Lựa chọn không được bỏ trống";
                        isCreate = false;
                    }

                    if (questionType == "trac-nghiem" && isCreate)
                    {
                        if (string.IsNullOrEmpty(questionText))
                        {
                            errors = string.IsNullOrEmpty(errors)
                            ? "Câu hỏi không được bỏ trống"
                                : $"{errors}, Câu hỏi không được bỏ trống";
                            isCreate = false;
                        }
                        if (string.IsNullOrEmpty(choices))
                        {
                            errors = string.IsNullOrEmpty(errors)
                            ? "Lựa chọn không được bỏ trống"
                                : $"{errors}, Lựa chọn không được bỏ trống";
                            isCreate = false;
                        }
                        if (string.IsNullOrEmpty(correctAnswer))
                        {
                            errors = string.IsNullOrEmpty(errors)
                            ? "Đáp án trắc nghiệm không được bỏ trống"
                                : $"{errors}, Đáp án trắc nghiệm không được bỏ trống";
                            isCreate = false;
                        }
                        answerexplanation = null;
                    }
                    else if (questionType == "tu-luan" && isCreate)
                    {
                        if (string.IsNullOrEmpty(questionText))
                        {
                            errors = string.IsNullOrEmpty(errors)
                            ? "Câu hỏi không được bỏ trống"
                                : $"{errors}, Câu hỏi không được bỏ trống";
                            isCreate = false;
                        }
                        if (string.IsNullOrEmpty(answerexplanation))
                        {
                            errors = string.IsNullOrEmpty(errors)
                            ? "Đáp án tự luận không được bỏ trống"
                                : $"{errors}, Đáp án tự luận không được bỏ trống";
                            isCreate = false;
                        }
                        choices = null;
                    }
                    //if(string.IsNullOrEmpty(subjectCode))
                    //{
                    //    errors = string.IsNullOrEmpty(errors)
                    //        ? "Mã môn không dược để trống"
                    //            : $"{errors}, Mã môn không dược để trống";
                    //    isCreate = false;
                    //}
                    if (string.IsNullOrEmpty(lessonCode))
                    {
                        errors = string.IsNullOrEmpty(errors)
                            ? "Mã bài học không dược để trống"
                                : $"{errors}, Mã bài học không dược để trống";
                        isCreate = false;
                    }
                    //var subjectRes = await ApiClient.GetAsync<Subject>(Request, $"Subjects/GetByCode/{subjectCode}");
                    //if (subjectRes.Data == null)
                    //{
                    //    errors = string.IsNullOrEmpty(errors)
                    //        ? "Mã môn học không tồn tại"
                    //            : $"{errors}, Mã môn học không tồn tại";
                    //    isCreate = false;
                    //}
                    var lessonRes = await ApiClient.GetAsync<Lesson>(Request, $"Lessons/GetByCode/{lessonCode}");
                    if (lessonRes.Data == null)
                    {
                        errors = string.IsNullOrEmpty(errors)
                            ? "Mã bài học không tồn tại"
                                : $"{errors}, Mã bài học không tồn tại";
                        isCreate = false;
                    }
                    #endregion


                    if (isCreate)
                    {
                        var questions = new CUQuestion
                        {
                            QuestionText = questionText,
                            Choices = choices,
                            CorrectAnswer = correctAnswer,
                            AnswerExplanation = answerexplanation,
                            SubjectId = lessonRes.Data.SubjectId ?? 0,
                            LessonId = lessonRes.Data.Id,
                            QuestionTypeId = questionType == "trac-nghiem" ? 1 : 2,
                        };
                        var createRs = await ApiClient.PostAsync<Domain.Models.Question>(Request, $"Questions", JsonConvert.SerializeObject(questions));
                        if (!createRs.Success)
                        {
                            errors = string.IsNullOrEmpty(errors)
                            ? createRs.Message
                                : $"{errors}, {createRs.Message}";
                            errorList.Add(new ImportQuestionMessage(row.RowNumber(), errors + "\n"));
                        }
                    }
                    else
                    {
                        errorList.Add(new ImportQuestionMessage(row.RowNumber(), errors + "\n"));
                    }
                }
            }
            return errorList;
        }
        public bool QuestionTypeExists(string questionTypeID)
        {
            return questionTypeID == "trac-nghiem" || questionTypeID == "tu-luan";
        }
    }
}
