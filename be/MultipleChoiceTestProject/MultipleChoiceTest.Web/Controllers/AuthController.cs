﻿using AspNetCoreHero.ToastNotification.Abstractions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Web.Api;
using MultipleChoiceTest.Web.Constants;
using MultipleChoiceTest.Web.Controllers.Guard;
using Newtonsoft.Json;

namespace MultipleChoiceTest.Web.Controllers
{
    public class AuthController : BaseController
    {
        public AuthController(INotyfService notyfService, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger, IMapper mapper) : base(notyfService, httpContextAccessor, logger, mapper)
        {
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (Request.Cookies.ContainsKey(UserConstant.AccessToken))
            {
                string role = Request.Cookies[UserConstant.Role];
                if (!string.IsNullOrEmpty(role) && role == ((int)TypeUserConstant.Role.CUSTOMER).ToString())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                var result = await ApiClient.PostAsync<LoginResponse>(Request, "Auth/Login", JsonConvert.SerializeObject(model));
                if (result.Success)
                {
                    if (result.Data.User.IsAdmin != true)
                    {
                        LoginSuccess(result.Data);
                        this._notyfService.Success("Đăng nhập thành công!");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        this._notyfService.Error("Không tồn tại tài khoản");
                    }
                }
                else
                {
                    this._notyfService.Error("Sai tên đăng nhập hoặc mật khẩu");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    this._notyfService.Error(error.ErrorMessage);
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            if (Request.Cookies.ContainsKey(UserConstant.AccessToken))
            {
                string role = Request.Cookies[UserConstant.Role];
                if (!string.IsNullOrEmpty(role) && role == ((int)TypeUserConstant.Role.CUSTOMER).ToString())
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var result = await ApiClient.PostAsync<User>(Request, "Auth/RegisterForUser", JsonConvert.SerializeObject(model));
                if (result.Success)
                {
                    this._notyfService.Success("Đăng ký thành công!");
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    this._notyfService.Error(result.Message);
                }
            }

            return View(model);
        }

        [User]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(UserConstant.AccessToken);
            Response.Cookies.Delete(UserConstant.Role);
            Response.Cookies.Delete(UserConstant.UserId);
            Response.Cookies.Delete(UserConstant.AccountName);

            return RedirectToAction("Login", "Auth");
        }

        public void LoginSuccess(LoginResponse loginRes)
        {
            ApiClient.SetCookie(Response, UserConstant.AccessToken, loginRes.AccessToken);
            ApiClient.SetCookie(Response, UserConstant.Role, loginRes.User?.IsAdmin == true ? "1" : "0");
            ApiClient.SetCookie(Response, UserConstant.UserId, loginRes.User.Id.ToString() ?? "");
            ApiClient.SetCookie(Response, UserConstant.AccountName, loginRes.User.Email);
        }


    }
}
