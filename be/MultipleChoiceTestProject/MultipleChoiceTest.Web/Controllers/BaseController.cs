﻿using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MultipleChoiceTest.Web.Api;
using MultipleChoiceTest.Web.Constants;

namespace MultipleChoiceTest.Web.Controllers
{
    public class BaseController : Controller
    {
        protected readonly INotyfService _notyfService;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly ILogger<BaseController> _logger;
        protected readonly IMapper _mapper;
        protected string userCurrentId;
        public BaseController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper)
        {
            _notyfService = notyfService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _mapper = mapper;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            userCurrentId = ApiClient.GetCookie(Request, UserConstant.UserId);
        }
    }
}
