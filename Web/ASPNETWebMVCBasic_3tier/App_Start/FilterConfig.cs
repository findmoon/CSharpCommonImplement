using System.Web;
using System.Web.Mvc;

namespace ASPNETWebInstall
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalFilterAttribute());
            filters.Add(new HandleErrorAttribute());
            // 添加全局 授权 过滤器
            filters.Add(new AuthorizeAttribute());
        }
    }
}
