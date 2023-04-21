namespace WebAPI_CURD.Models
{
    /// <summary>
    /// 统一返回的Model类 【响应类】
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIModel<T>
    {
        public int status { get; set; } = 200;
        public string message { get; set; } = "OK";
        public T Data { get; set; }
    }

    /// <summary>
    /// 统一返回的Model类，默认 Data 为 string 类型的 APIModel
    /// </summary>
    public class APIModel : APIModel<string>
    {
    }
}
