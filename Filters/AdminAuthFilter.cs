using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OBeefSoup.Services;

namespace OBeefSoup.Filters
{
    /// <summary>
    /// Bảo vệ toàn bộ Admin area - yêu cầu đăng nhập
    /// </summary>
    public class AdminAuthFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            // Bỏ qua kiểm tra cho Login, Logout, ResetSeed, AccessDenied
            if (controller == "Account" && (action == "Login" || action == "Logout" || action == "ResetSeed" || action == "AccessDenied"))
            {
                base.OnActionExecuting(context);
                return;
            }

            var userId = session.GetString(AuthService.SESSION_USER_ID);
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new RedirectToActionResult("Login", "CustomerAccount", new { area = "" });
                return;
            }

            base.OnActionExecuting(context);
        }
    }

    /// <summary>
    /// Chỉ cho phép tài khoản Admin (không áp dụng cho Manager)
    /// </summary>
    public class AdminOnlyFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var role = session.GetString(AuthService.SESSION_USER_ROLE);

            if (role != "Admin")
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Account", new { area = "Admin" });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
