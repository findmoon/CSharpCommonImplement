**ASP.NET实现永久运行或一直运行的定时任务**



使用ASP.NET实现Windows Service定时执行任务
：https://developer.aliyun.com/article/531170

利用缓存失效实现Asp.net真正的永久定时运行任务，非常巧妙【里面提到最小时间2分钟，似乎工作进程最小检查时间有关。该文章至少在2011年之前就存在，所以现在的IIS也许能提供更小时间的缓存检查】。
更多去查看 集锦：“Net6 windows服务 ASPNet定时永久运行” 

Making ASP.NET Application Always Running：https://docs.hangfire.io/en/latest/deployment-to-production/making-aspnet-app-always-running.html


以上两种方法，一个利用缓存实现定时运行；一个实现永久运行（然后自己在永久运行中实现定时）


https://blog.csdn.net/maaici/article/details/108746085   这个应该可以不用修改 就能做到 定时一直有效（系列专栏都很不错）
