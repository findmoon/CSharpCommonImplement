Asp.NET MVC 5 ������Web����ʵ�֣�

1. MVC �� ����ܹ� �� ���ʵ�֣����ֲ㣨UI����ҵ���߼��㣨BLL�������ݷ��ʲ㣨DAL�� �� ʵ����(Model)��

2. �Զ���ʵ���û���֤����Ȩ����½��ע�ᣩ���������õ�FormsAuthentication��[Authorize](AccountController.cs ��)���Լ�ȫ�ֵ���Ȩ��������filters.Add(new AuthorizeAttribute())��FilterConfig.cs��

3. �Զ���ȫ�ֹ����� GlobalFilterAttribute ����FilterConfig.cs��

4. Autofac ����ע��Ϳ��Ʒ�ת ��ʹ��

5. Dapper ��ʹ�ã�����Oracle [���ڲ����������⣬���� OracleDynamicParameters.cs ʵ��]

6. .Net Framework �����û�����

7. MVC��[HttpGet]û��·�ɲ����Ĺ��캯������ʹ�á�[Route("{id}")]��[Route("[controller]/{id}")] ���޷�ƥ�䵽Action��id����(public async Task<ActionResult> GetById(int id))

��� Web API �� Microsoft.AspNet.WebApi ����δ���á�

8. mailSettings �ʼ��������� �� ʵ�� ��δʵ�֡�