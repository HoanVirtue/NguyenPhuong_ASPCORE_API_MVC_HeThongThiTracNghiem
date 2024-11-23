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
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            //await CreateViewBagAsync();
            return View();
        }

        // POST: Brand/Create
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
    }
}
