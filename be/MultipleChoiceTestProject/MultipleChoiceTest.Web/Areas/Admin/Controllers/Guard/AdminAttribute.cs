using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MultipleChoiceTest.Web.Constants;
namespace MultipleChoiceTest.Web.Controllers.Guard
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AdminAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;

            // Kiểm tra sự tồn tại của token trong cookie
            if (!httpContext.Request.Cookies.TryGetValue(UserConstant.AccessToken, out var token) || string.IsNullOrEmpty(token))
            {
                // Chuyển hướng đến trang đăng nhập nếu không có token
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "area", "Admin" },
                    { "controller", "Auth" },
                    { "action", "Login" }
                });
                return;
            }
            if (!httpContext.Request.Cookies.TryGetValue(UserConstant.Role, out var role) || string.IsNullOrEmpty(role))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    { "area", "Admin" },
                    { "controller", "Auth" },
                    { "action", "Login" }
                });
                return;
            }
            else
            {
                if (role != ((int)TypeUserConstant.Role.ADMIN).ToString())
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        { "area", "Admin" },
                        { "controller", "Auth" },
                        { "action", "Login" }
                    });
                    return;
                }
            }
        }
    }
}
