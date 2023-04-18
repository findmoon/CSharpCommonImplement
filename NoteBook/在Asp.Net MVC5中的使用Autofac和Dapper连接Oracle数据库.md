**在Asp.Net MVC5中的使用Autofac和Dapper连接Oracle数据库**

[toc]

> ASPNETWebMVCBasic 项目下。

# Autofac的使用

## 安装nuget包 Autofac 和 Autofac.Mvc5

![](img/20230418103705.png)

> 项目引用 - 程序集 System.Web.Mvc.dll 右键属性，可以查看 mvc 的版本号。
> 
> 项目文件 .csproj 中也可以查看 MVC 版本号。

## mvc项目的App_Start文件夹下添加AutofacConfig类

```cs
namespace ASPNETWebMVCBasic.App_Start
{
    public class AutofacConfig
    {
        /// <summary>
        /// 负责调用autofac框架实现 业务逻辑层 和 数据仓储层程序集中的类型对象的创建
        /// 负责创建MVC控制器类的对象(调用控制器中的有参构造函数),接管DefaultControllerFactory的工作
        /// </summary>
        public static void Register()
        {
            //实例化一个autofac的创建容器
            var builder = new ContainerBuilder();

            #region 注入service

            builder.RegisterType<UserService>().As<IUserService>();

            #endregion

            #region 注入Repository

            builder.RegisterType<UserRepository>().As<IUserRepository>();

            #endregion

            //注入控制控制器
            builder.RegisterControllers(typeof(MvcApplication).Assembly);


            //创建一个Autofac的容器
            var container = builder.Build();
            //将MVC的控制器对象实例 交由autofac来创建
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}
```

**MVC和API不同的注入方式：**

```C#
var builder = new ContainerBuilder();
builder.RegisterControllers(Assembly.GetExecutingAssembly()); //Register MVC Controllers
builder.RegisterApiControllers(Assembly.GetExecutingAssembly()); //Register WebApi Controllers
builder.RegisterType<Type>().As<IType>();

var container = builder.Build();

DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); //Set the MVC DependencyResolver
GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container); //Set the WebApi DependencyResolver
```

## Global.asax 中初始化Autofac的配置

Global.asax 文件中：

```C#
namespace ASPNETWebMVCBasic
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Autofac 初始化注册
            AutofacConfig.Register();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
```

## Controller、Service 构造函数中 注入 


```C#
namespace WebAPI_CURD.Controllers
{
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
......
......
```


```C#
namespace WebAPI_CURD.Services
{

    /// <summary>
    /// core business logic and validation related to user CRUD operations.连接 controllers 和 repositories
    /// </summary>
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;


        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

......
......
```

# Dapper连接Oracle

安装Nuget包 Dapper 和 Oracle.ManagedDataAccess 19.18.0（此版本没有其他依赖，之后的版本都有 System.Text.Json 依赖）

# 参考

[【Autofac】在Asp.Net MVC5中的使用](https://www.cnblogs.com/chuankang/articles/9717873.html)