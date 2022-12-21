**MessageBox显示对话框模式的所属窗体问题，及WPF中ShowDialog模式所属窗体问题**

[toc]

- `MessageBox.Show`设置对话框所属窗体

默认，如果 当前激活的窗体不是所在的应用窗体，显示的`MessageBox`对话框将不会以当前应用窗体为`Dialog`模式，即可以随便点击应用窗体，`Dialog`模式不会针对任何窗体有效。

因此，如果想要严格的`Dialog`，可以显式设置所属窗体`ownedWindow`。

```C#
    try
    {
        // Oracle连接
    }
    catch (OracleException ex)
    {
        //MessageBox.Show($"连接Oracle出错：{ex.LastMessage()}，请确认问题后重试!");

        // 默认，如果 当前激活的窗体不是所在的应用窗体，显示的MessageBox对话框将不会以当前应用窗体为Dialog模式，即可以随便点击应用窗体
        MessageBox.Show(Application.Current.MainWindow, $"连接Oracle出错：{ex.LastMessage()}，请确认问题后重试!");

        MessageBox.Show("程序将退出");
        Application.Current.Shutdown();
    }
```