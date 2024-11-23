﻿using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace MultipleChoiceTest.Web.Areas.Admin.Controllers
{
    public class ExamResultsController : BaseController
    {
        public ExamResultsController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}