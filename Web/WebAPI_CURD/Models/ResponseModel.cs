namespace WebAPI_CURD.Models
{
    /// <summary>
    /// 统一的响应类
    /// </summary>
    public class ResponseModel<T>
    {
        public int status { get; set; } = 200;
        public string? message { get; set; }
        public T? Data { get; set; }
    }
}
