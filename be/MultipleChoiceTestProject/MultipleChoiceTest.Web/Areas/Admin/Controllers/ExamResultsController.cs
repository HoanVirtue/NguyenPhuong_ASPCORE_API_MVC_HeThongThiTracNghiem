using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.Helpper;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;

namespace MultipleChoiceTest.Web.Areas.Admin.Controllers
{
    public class ExamResultsController : BaseController
    {
        public ExamResultsController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {
        }
        // chưa có api em ơi 
        // lỗi 404 not found: là không tìm thấy địa chỉ
        [HttpGet]
        public async Task<IActionResult> Index(string searchKey = "")
        {
            var rf = await ApiClient.GetAsync<List<ResultItem>>(Request, "ExamResults");

            if (rf.Success)
            {
                var result = rf.Data;
                if (!string.IsNullOrWhiteSpace(searchKey))
                {
                    searchKey = Utilities.RemoveDiacriticsAndToLower(searchKey);
                    result = result.Where(p => Utilities.IsSubstring(Utilities.RemoveDiacriticsAndToLower(p.ExamName), searchKey)).ToList();
                }
                ViewBag.SearchKey = searchKey;
                return View(result);
            }
            this._notyfService.Error("Retrieving the list of type failed");
            return View();
        }

        // http post thì không gọi được như thế e nha, chỉ có httpget thì mới gọi được kiểu thế
        // đang ởângmre
        // quên cách a dạy rồi, để mà k bị nhầm lẫn e dùng ctrl + H cho anh cái í mới cop từ chỗ khác, user e cx nhầm của a mấy chỗ đó
        // nhuwgx e chạy có chết đâu, e chưa test kĩ, này mà 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var delRs = await ApiClient.DeleteAsync<ExamResult>(Request, $"ExamResults/{id}");
                if (delRs.Success)
                {
                    _notyfService.Success("Xóa dữ liệu thành công");
                }
                else
                {
                    _notyfService.Error(delRs.Message);
                }
                return RedirectToAction("Index", "ExamResults");

            }
            catch (Exception ex)
            {
                _notyfService.Error("Đã có lỗi xảy ra!");
                return RedirectToAction("Delete", "ExamResults");
            }
        }

    }
}
