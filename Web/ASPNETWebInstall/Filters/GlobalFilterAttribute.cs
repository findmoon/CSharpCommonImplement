using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc
{
    public class GlobalFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
      
         
                if (DateTime.Now>DateTime.Parse("2023-12-31"))
                {
                filterContext.Result = new ContentResult() {
                    Content = "<h1 style=\"text-align:center;margin-top:30px;\">已过期，若想继续使用，请联系管理员<h1>",
                         ContentType= "text/html", // application/json
                        
                    }; //跳转到登录页面并把当前所在页面的URL当做参数传到登录页面
                }
            
        }
    }
}