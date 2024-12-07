using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.Constants.Api;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;
using MultipleChoiceTest.Web.Constants;
using MultipleChoiceTest.Web.Controllers.Guard;

namespace MultipleChoiceTest.Web.Controllers
{
    public class ExamsController : BaseController
    {
        public ExamsController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {

        }

        [User]
        [HttpGet]
        public async Task<IActionResult> InfoExam(int id)
        {
            if (id != null && id > 0)
            {
                var infoExam = await ApiClient.GetAsync<ExamItem>(Request, $"Exams/{id}?type={(int)TypeGetSelectConstant.TypeGetDetail.GetDetail}");
                if (!infoExam.Success)
                {
                    return NotFound();
                }
                var userId = ApiClient.GetCookie(Request, UserConstant.UserId);
                ViewData["Username"] = (await ApiClient.GetAsync<User>(Request, $"Users/{userId}")).Data.UserName;
                return View(infoExam.Data);
            }
            return BadRequest();
        }


        public IActionResult StartExam()
        {
            return View();
        }
        
    }
}
