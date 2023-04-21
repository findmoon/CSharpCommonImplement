using Autofac.Integration.Mvc;
using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAPI_CURD.Services;
using WebAPI_CURD.Repositories;
using System.Reflection;
using Dapper;
using System.Diagnostics;

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

            #region 注册单例
            builder.RegisterType<DataContext>().SingleInstance();
            #endregion

            #region 注册service

            builder.RegisterType<UserService>().As<IUserService>();

            #endregion

            #region 注册Repository

            builder.RegisterType<UserRepository>().As<IUserRepository>();

            #endregion

            #region 使用 RegisterAssemblyTypes  一次注册多个 按名称约定的 类型
            //加载要注入的程序集
            //var iServices = Assembly.Load("AutoFac.Dapper.IService");
            //var services = Assembly.Load("AutoFac.Dapper.Service");
            //根据名称约定（服务层的接口和实现均以Service结尾），实现服务接口和服务实现的依赖
            //builder.RegisterAssemblyTypes(iServices, services)
            //  .Where(t => t.Name.EndsWith("Service"))
            //  .AsImplementedInterfaces().InstancePerRequest();

            #endregion

            #region 其他类
            builder.RegisterType<OracleDynamicParameters>();
            #endregion

            //注册控制控制器
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            //// ASPNETWebMVCBasic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
            //Debug.WriteLine(typeof(MvcApplication).Assembly, "Assembly");

            //创建一个Autofac的容器
            var container = builder.Build();
            //将注册的类型对象实例 交由autofac来创建
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));


            // 测试
            // DataContext.Test();

            // 只有在使用时，才会创建相应对象
            var dataCtx = container.Resolve<DataContext>();//通过IOC容器创建对象

            // 执行一些必要的初始化
            dataCtx.Init().Wait();
        }
        /* MVC和API不同的注入方式：
         * 
         var builder = new ContainerBuilder();
         builder.RegisterControllers(Assembly.GetExecutingAssembly()); //Register MVC Controllers
         builder.RegisterApiControllers(Assembly.GetExecutingAssembly()); //Register WebApi Controllers
         builder.RegisterType<Type>().As<IType>();
         
         var container = builder.Build();
         
         DependencyResolver.SetResolver(new AutofacDependencyResolver(container)); //Set the MVC DependencyResolver
         GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container); //Set the WebApi DependencyResolver
         */
    }
}