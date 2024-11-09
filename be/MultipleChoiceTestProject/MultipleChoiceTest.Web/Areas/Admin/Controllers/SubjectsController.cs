using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.Helpper;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;
using MultipleChoiceTest.Web.Controllers.Guard;
using Newtonsoft.Json;

namespace MultipleChoiceTest.Web.Areas.Admin.Controllers
{
    [Admin]
    public class SubjectsController : BaseController
    {
        public SubjectsController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchKey = "")
        {
            var subjectRs = await ApiClient.GetAsync<List<Subject>>(Request, "Subjects");

            if (subjectRs.Success)
            {
                var subjects = subjectRs.Data;
                if (!string.IsNullOrWhiteSpace(searchKey))
                {
                    searchKey = Utilities.RemoveDiacriticsAndToLower(searchKey);
                    subjects = subjects.Where(p => Utilities.IsSubstring(Utilities.RemoveDiacriticsAndToLower(p.SubjectName), searchKey)).ToList();
                }
                ViewBag.SearchKey = searchKey;
                return View(subjects);
            }
            this._notyfService.Error("Retrieving the list of type failed");
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brand/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,SubjectName")] CUSubject subject)
        {
            if (ModelState.IsValid)
            {
                var createRs = await ApiClient.PostAsync<Subject>(Request, "Subjects", JsonConvert.SerializeObject(subject));
                if (createRs.Success)
                {
                    _notyfService.Success("Thêm dữ liệu thành công");
                    return RedirectToAction("Index", "Subjects");
                }
                _notyfService.Error("Thêm dữ liệu thất bại");
                return View(subject);
            }
            _notyfService.Error("Vui lòng nhập đầy đủ dữ liệu");
            return View(subject);
        }

        // GET: Brand/Edit/Id
        public async Task<IActionResult> Edit(int id)
        {
            var detailRs = await ApiClient.GetAsync<Subject>(Request, $"Subjects/{id}");
            var data = _mapper.Map<CUSubject>(detailRs.Data);
            if (detailRs.Success)
            {
                return View(data);
            }

            _notyfService.Error("Subject not found");
            return View(data);
        }

        // POST: Brand/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubjectName")] CUSubject subject)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updRs = await ApiClient.PutAsync<Subject>(Request, $"Subjects", JsonConvert.SerializeObject(_mapper.Map<Subject>(subject)));
                    if (updRs != null && updRs.Success)
                    {
                        _notyfService.Success("Cập nhật dữ liệu thành công");
                    }
                    else
                    {
                        _notyfService.Warning(updRs.Message);
                    }
                    return RedirectToAction("Index", "Subjects");
                }
                catch (Exception ex)
                {
                    _notyfService.Error("An error occurred");
                    return View(subject);
                }
            }
            return View(subject);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var delRs = await ApiClient.DeleteAsync<Subject>(Request, $"Subjects/{id}");
                if (delRs.Success)
                {
                    _notyfService.Success("Xóa dữ liệu thành công");
                }
                else
                {
                    _notyfService.Error(delRs.Message);
                }
                return RedirectToAction("Index", "Subjects");

            }
            catch (Exception ex)
            {
                _notyfService.Error("Đã có lỗi xảy ra!");
                return RedirectToAction("Delete", "Subjects");
            }
        }
    }
}
