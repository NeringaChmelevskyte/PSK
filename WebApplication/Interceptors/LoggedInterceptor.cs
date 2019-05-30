using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.IServices;

namespace WebApplication.Interceptors
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class LoggedInterceptor : ActionFilterAttribute, IExceptionFilter
    {
        ILogService logService;
        IUserService userService;
        public LoggedInterceptor(ILogService logService, IUserService userService)
        {
            this.logService = logService;
            this.userService = userService;
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var user = userService.GetUserFromRequest(filterContext.HttpContext.Request);
            string message = String.Format("{0} {1} | {2}", user.Name, user.Surname, filterContext.ActionDescriptor.DisplayName);
            logService.AddLogMessage(message);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        public void OnException(ExceptionContext filterContext)
        {
        }
    }
}
