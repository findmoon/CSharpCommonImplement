**Windows下如何查看某个端口被谁占用**

> [Windows下如何查看某个端口被谁占用](https://www.runoob.com/w3cnote/windows-finds-port-usage.html)
>
> 查看被占用端口对应的PID：`netstat -aon|findstr "8081"`
>
> 查看指定 PID 的进程：`tasklist|findstr "9088"`
>
> 强制（/F参数）杀死 pid 为 9088 的所有进程包括子进程（/T参数）：`taskkill /T /F /PID 9088`