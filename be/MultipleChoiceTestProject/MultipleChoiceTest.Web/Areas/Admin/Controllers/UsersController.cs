
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
    public class UsersController : BaseController
    {
        public UsersController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchKey = "")
        {
            var user = await ApiClient.GetAsync<List<UserItem>>(Request, "Users");

            if (user.Success)
            {
                var users = user.Data;
                if (!string.IsNullOrWhiteSpace(searchKey))
                {
                    searchKey = Utilities.RemoveDiacriticsAndToLower(searchKey);
                    users = users.Where(p => Utilities.IsSubstring(Utilities.RemoveDiacriticsAndToLower(p.AccountName), searchKey)).ToList();
                }
                ViewBag.SearchKey = searchKey;
                return View(users);
            }
            this._notyfService.Error("Retrieving the list of type failed");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // POST: Brand/Create
        // em để ý thằng create này xem, khi nó lỗi thì nó lại trả về trang create ban đầu và truền thêm dl, khi lỗi nó phải báo lỗi và ở yên trang đag sửa chứ s lại về trang ban đầu là seo
        // đrio thế a mới bảo e xem lại create á, sau cái kia a sửa sau
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,UserName,Email,Gender,DateOfBirth,Phone,AccountName,PasswordHash,IsAdmin")] CUUser user)
        {
            if (ModelState.IsValid)
            {
                var createRs = await ApiClient.PostAsync<User>(Request, "Users", JsonConvert.SerializeObject(user));
                if (createRs.Success)
                {
                    _notyfService.Success("Thêm dữ liệu thành công");
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    _notyfService.Warning(createRs.Message);
                }
                _notyfService.Error("Thêm dữ liệu thất bại");
                return View(user);
            }
            _notyfService.Error("Vui lòng nhập đầy đủ dữ liệu");
            return View(user);
        }
        // GET: Brand/Edit/Id
        public async Task<IActionResult> Edit(int id)
        {
            var detailRs = await ApiClient.GetAsync<User>(Request, $"Users/{id}");
            var data = _mapper.Map<CUUser>(detailRs.Data);
            if (detailRs.Success)
            {
                return View(data);
            }

            _notyfService.Error("Không tìm thấy người dùng");
            return View(data);
        }

        // POST: Brand/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserName,Email,Gender,DateOfBirth,Phone,AccountName,PasswordHash,IsAdmin")] CUUser user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var updRs = await ApiClient.PutAsync<User>(Request, $"Users", JsonConvert.SerializeObject(user));
                    if (updRs != null && updRs.Success)
                    {
                        _notyfService.Success("Cập nhật dữ liệu thành công");
                        // thành công trả về danh sách
                        return RedirectToAction("Index", "Users");
                    }
                    else
                    {
                        _notyfService.Warning(updRs.Message);
                    }
                    return View(user);
                }
                catch (Exception ex)
                {
                    _notyfService.Error("Đã có lỗi xảy ra vui lòng thử lại sau!");
                }
            }
            return View(user);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var delRs = await ApiClient.DeleteAsync<User>(Request, $"Users/{id}");
                if (delRs.Success)
                {
                    _notyfService.Success("Xóa dữ liệu thành công");
                }
                else
                {
                    _notyfService.Error(delRs.Message);
                }
                return RedirectToAction("Index", "Users");

            }
            catch (Exception ex)
            {
                _notyfService.Error("Đã có lỗi xảy ra!");
                return RedirectToAction("Delete", "Users");
            }
        }
    }
}
