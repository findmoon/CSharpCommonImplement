**WPF中的换行符及TextBox中实现换行**


XAML中为

&#13;

C#代码中为

\r\n
或者：
Environment.NewLine

方法1：

```xml
<TextBlock Text="第一行&#x0a;第二行"/>
```

文字中间加上：
&#x0a;
就可以了。这个方法很有用，也可以用在ToolTip上：

```xml
<ToolTip Width="200" Content="'第一行' 
                   &#x0a;&#x0d;'第二行' 
                   &#x0a;&#x0d;'第三行'"/>
```

方法2：

```xml
<TextBlock xml:space="preserve">
第一行
第二行
</TextBlock>
```

Preserve能保留xaml中的空格


方法3：

```xml
<TextBlock Text="{Binding StringFormat='第一行{0}第二行{0}第三行',
                          Source={x:Static s:Environment.NewLine}}" />
```


[WPF TextBlock 的换行符](https://blog.csdn.net/soft2buy/article/details/17198921)