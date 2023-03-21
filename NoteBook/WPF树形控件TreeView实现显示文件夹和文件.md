**WPF树形控件TreeView实现显示文件夹和文件**

[toc]



# 附：DataGrid 根据选择的元素内容的数据自动生成列和数据绑定

[WPF-TreeView获取文件夹目录、DataGrid获取目录下文件信息](https://www.cnblogs.com/amourjun/p/6534161.html) 中给出的示例是自动生成 DataGrid 列，感觉很不错，在此记录下来。

- WAML 结构

```xml
 <TreeView x:Name="directoryTreeView">
     <TreeView.Resources>
        <HierarchicalDataTemplate DataType="{x:Type local:DirectoryRecord}"
                        ItemsSource="{Binding Directories}" >
          <StackPanel Orientation="Horizontal">
                  <TextBlock Text="{Binding Info.Name}"/>
            </StackPanel>
          </HierarchicalDataTemplate>
      </TreeView.Resources>
 </TreeView>


<DataGrid x:Name="fileInfoDataGrid" ItemsSource="{Binding SelectedItem.Files, ElementName=directoryTreeView}">
```

- `TreeView`数据绑定的`DirectoryRecord`类型结构

```C#
/// <summary>
/// 文件夹信息记录类
/// </summary>
public class DirectoryRecord
{
    public DirectoryInfo Info { get; }
    public string folderPath { get; }


    public DirectoryRecord(string folderPath)
    {
        this.folderPath = folderPath;
        if (Directory.Exists(folderPath))
        {
            Info = new DirectoryInfo(folderPath);
        }
    }
    public DirectoryRecord(DirectoryInfo Info)
    {
        this.Info = Info;
        folderPath = Info.FullName;
    }

    public IEnumerable<FileInfo> Files
    {
        get
        {
            return Info?.GetFiles();
        }
    }

    public IEnumerable<DirectoryRecord> Directories
    {
        get
        {
            return from di in Info?.GetDirectories("*", SearchOption.TopDirectoryOnly)
                   select new DirectoryRecord( di);
        }
    }
}
```

- `DataGrid.AutoGeneratingColumn` 自动生成列事件

```js
//DataGrid事件，设置列标
fileInfoDataGrid.AutoGeneratingColumn += fileInfoDataGridColumn_Load;

private void fileInfoDataGridColumn_Load(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            List<string> requiredProperties = new List<string>
            {
                "Name", "Length", "FullName", "LastWriteTime"
            };

            if (!requiredProperties.Contains(e.PropertyName))
            {
                e.Cancel = true;
            }
            else
            {
                e.Column.Header = e.Column.Header.ToString();
            }
        }
```

- 树形控件数据的绑定赋值

```C#
/// <summary>
/// 设置 directoryTreeView.ItemsSource
/// </summary>
/// <param name="folderPath">文件夹路径</param>
private void SetDirTreeViewDatas(string folderPath)
{
    var directory = new ObservableCollection<DirectoryRecord>
    {
        new DirectoryRecord(folderPath)
    };
    directoryTreeView.ItemsSource = directory;
}
```

- 最后就是指定要加载的文件夹路径`folderPath`，调用`SetDirTreeViewDatas`：

比如窗体加载事件中显示 "C:\Windows" 下的文件夹

```C#
Loaded += MainWindow_Loaded;

// .....

private void MainWindow_Loaded(object sender, RoutedEventArgs e)
{
    SetDirTreeViewDatas(@"C:\Windows");
}
```

# 附：关于 ObservableCollection 类



# 参考

- [WPF-TreeView获取文件夹目录、DataGrid获取目录下文件信息](https://www.cnblogs.com/amourjun/p/6534161.html)