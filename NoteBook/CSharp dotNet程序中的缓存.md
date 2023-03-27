**C# .Net程序中的缓存**

[toc]

# WPF使用缓存

> 原文 [WPF使用缓存](https://www.cnblogs.com/yetsen/p/13561818.html)

[System.Runtime.Caching](https://docs.microsoft.com/zh-cn/dotnet/api/system.runtime.caching) 命名空间是 .NET Framework 4 中的新命名空间。 此命名空间使缓存可供所有 .NET Framework 应用程序使用。

```C#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.Caching;
using System.IO;

namespace WPFCaching
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            ObjectCache cache = MemoryCache.Default;
            string fileContents = cache["filecontents"] as string;

            if (fileContents == null)
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration =
                    DateTimeOffset.Now.AddSeconds(10.0);

                List<string> filePaths = new List<string>();
                filePaths.Add("c:\\cache\\cacheText.txt");

                policy.ChangeMonitors.Add(new
                    HostFileChangeMonitor(filePaths));

                // Fetch the file contents.
                fileContents = File.ReadAllText("c:\\cache\\cacheText.txt") + "\n" + DateTime.Now.ToString();

                cache.Set("filecontents", fileContents, policy);
            }
            MessageBox.Show(fileContents);
        }
    }
}
```

如果未提供任何逐出或过期信息，则默认值为 [InfiniteAbsoluteExpiration](https://docs.microsoft.com/zh-cn/dotnet/api/system.runtime.caching.objectcache.infiniteabsoluteexpiration)，这意味着缓存条目永远不会仅基于绝对时间过期。 相反，缓存项仅在存在内存压力时过期。 最佳做法是，应始终显式提供绝对或可调过期。

[HostFileChangeMonitor](https://docs.microsoft.com/zh-cn/dotnet/api/system.runtime.caching.hostfilechangemonitor) 对象监视文本文件的路径，并在发生更改时通知缓存。 在此示例中，如果文件的内容发生更改，缓存项将过期。
