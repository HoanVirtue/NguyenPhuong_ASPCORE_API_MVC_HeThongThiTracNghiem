using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.Constants.Api;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;
using System.Diagnostics;
using ErrorViewModel = MultipleChoiceTest.Domain.ModelViews.ErrorViewModel;

namespace MultipleChoiceTest.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? pageIndex = 0)
        {
            var page = await ApiClient.GetAsync<Pagination<ExamItem>>(Request, $"Exams?type={(int)TypeGetSelectConstant.TypeGet.GetGrid}&pageIndex={pageIndex}");
            if (!page.Success)
            {
                _notyfService.Warning(page.Message);
            }
            ViewData["PageIndex"] = pageIndex;
            return View(page.Data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
