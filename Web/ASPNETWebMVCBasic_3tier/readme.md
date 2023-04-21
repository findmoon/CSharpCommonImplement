Asp.NET MVC 5 基础的Web功能实现：

1. MVC 与 三层架构 的 结合实现（表现层（UI）、业务逻辑层（BLL）、数据访问层（DAL） 与 实体类(Model)）

2. 自定义实现用户认证和授权（登陆、注册），借助内置的FormsAuthentication和[Authorize](AccountController.cs 内)，以及全局的授权过滤器（filters.Add(new AuthorizeAttribute())，FilterConfig.cs）

3. 自定义全局过滤器 GlobalFilterAttribute （，FilterConfig.cs）

4. Autofac 依赖注入和控制反转 的使用

5. Dapper 的使用，连接Oracle [由于参数变量问题，借助 OracleDynamicParameters.cs 实现]

6. .Net Framework 管理用户机密

7. MVC中[HttpGet]没有路由参数的构造函数可以使用。[Route("{id}")]、[Route("[controller]/{id}")] 均无法匹配到Action的id参数(public async Task<ActionResult> GetById(int id))

添加 Web API 包 Microsoft.AspNet.WebApi 【暂未配置】

8. mailSettings 邮件发送配置 和 实现 【未实现】