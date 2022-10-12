namespace System.Diagnostics
{
    public class ExecResult
    {
        public string Output { get; set; }
        /// <summary>
        /// 程序正常执行后的错误输出，需要根据实际内容判断是否成功。如果Output为空但Error不为空，则基本可以说明发生了问题或错误，但是可以正常执行结束
        /// </summary>
        public string Error { get; set; }
        /// <summary>
        /// 执行发生的异常，表示程序没有正常执行并结束
        /// </summary>
        public Exception ExceptError { get; set; }
    }
}
