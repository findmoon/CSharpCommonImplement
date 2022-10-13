使用TextWriterTraceListener实现简易的异步日志

# TextWriterTraceListener

`TextWriterTraceListener`是`TraceListener`的一个实现，用于直接将跟踪和调试输出到 文本写入类(TextWriter) 或 流(如FileStream) 中。

借助`TextWriterTraceListener`，我们可以实现一个简单的日志系统或异步日志系统。日志几乎在任何程序中都需要用到，如果是小型项目，则没有必要使用`NLog`、`Log4Net`、`Serilog`等大的日志框架，仅需要文件记录即可。那我们就可以自己实现简单的日志写入。

借助`TextWriterTraceListener`实现的异步的，支持控制台输出、日志文件输出、数据库输出的简易日志帮助类。