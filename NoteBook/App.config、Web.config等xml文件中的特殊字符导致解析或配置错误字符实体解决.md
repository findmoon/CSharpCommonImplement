**App.config、Web.config等xml文件中的特殊字符导致解析或配置错误，使用字符实体解决**

[toc]

# xml配置错误

如下，如果`Web.config`配置文件出现特殊字符，会直接报错XML格式不正确！`HTTP 错误 500.19 - Internal Server Error`：

![](img/20230307175022.png)

原因在于 xml的配置项的值存在特殊字符`&`，从导致错误。

解决办法，使用`&`的字符实体`&amp;`代替特殊字符。

> 字符实体，也叫做转义字符，是专门针对类xml格式的特殊字符的处理。比如`<`、`>`、`&`、`"`、空格等字符。

> 关于 XML 格式不正确问题的排查，首先要确认的就是是否有特殊字符。其次，可以通过换行配置项、删减配置项，以缩小错误提示的范围。

# HTML特殊转义字符列表

最常用的几个字符实体：

<table style="background: rgba(236, 236, 236, 1); border-collapse: collapse" border="0">
    <tbody valign="top">
    <tr>
        <td style="border: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p><span style="font-size: 14pt">显示</span></p>
        </td>
        <td style="border-top: 1.5pt solid rgba(255, 255, 255, 1); border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center"><strong>说明</strong></p>
        </td>
        <td style="border-top: 1.5pt solid rgba(255, 255, 255, 1); border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center"><strong>实体名称</strong></p>
        </td>
        <td style="border-top: 1.5pt solid rgba(255, 255, 255, 1); border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center"><strong>实体编号</strong></p>
        </td>
    </tr>
    <tr>
        <td style="border-top: none; border-left: 1.5pt solid rgba(255, 255, 255, 1); border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">&nbsp;
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">空格</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;nbsp;</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;#160;</p>
        </td>
    </tr>
    <tr>
        <td style="border-top: none; border-left: 1.5pt solid rgba(255, 255, 255, 1); border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p><span style="font-size: 14pt">&lt;</span></p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">小于</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;lt;</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;#60;</p>
        </td>
    </tr>
    <tr>
        <td style="border-top: none; border-left: 1.5pt solid rgba(255, 255, 255, 1); border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p><span style="font-size: 14pt">&gt;</span></p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">大于</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;gt;</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;#62;</p>
        </td>
    </tr>
    <tr>
        <td style="border-top: none; border-left: 1.5pt solid rgba(255, 255, 255, 1); border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p><span style="font-size: 14pt">&amp;</span></p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;符号</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;amp;</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;#38;</p>
        </td>
    </tr>
    <tr>
        <td style="border-top: none; border-left: 1.5pt solid rgba(255, 255, 255, 1); border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p><span style="font-size: 14pt">"</span></p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">双引号</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;quot;</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;#34;</p>
        </td>
    </tr>
    <tr>
        <td style="border-top: none; border-left: 1.5pt solid rgba(255, 255, 255, 1); border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p><span style="font-family: Estrangelo Edessa; font-size: 14pt">©</span></p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">版权</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;copy;</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;#169;</p>
        </td>
    </tr>
    <tr>
        <td style="border-top: none; border-left: 1.5pt solid rgba(255, 255, 255, 1); border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p><span style="font-family: Estrangelo Edessa; font-size: 14pt">®</span></p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">已注册商标</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;reg;</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;#174;</p>
        </td>
    </tr>
    <tr>
        <td style="border-top: none; border-left: 1.5pt solid rgba(255, 255, 255, 1); border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p><span style="font-family: Estrangelo Edessa; font-size: 14pt">™</span></p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">商标（美国）</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center"><span style="font-family: Estrangelo Edessa; font-size: 18px">™</span></p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;#8482;</p>
        </td>
    </tr>
    <tr>
        <td style="border-top: none; border-left: 1.5pt solid rgba(255, 255, 255, 1); border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p><span style="font-size: 14pt">×</span></p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">乘号</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;times;</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;#215;</p>
        </td>
    </tr>
    <tr>
        <td style="border-top: none; border-left: 1.5pt solid rgba(255, 255, 255, 1); border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p><span style="font-size: 14pt">÷</span></p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">除号</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;divide;</p>
        </td>
        <td style="border-top: none; border-left: none; border-bottom: 1.5pt solid rgba(255, 255, 255, 1); border-right: 1.5pt solid rgba(255, 255, 255, 1); padding: 1px" valign="middle">
            <p style="text-align: center">&amp;#247;</p>
        </td>
    </tr></tbody></table>