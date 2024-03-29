**ASP.net Core 自定义验证的实现及常见正则总结**

[toc]


```C#
namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// 正则验证的静态字段。形式为 [xxx]Reg[N]\[xxx]Reg[N]ErrMsg 成对出现，分别表示正则及该正则的错误消息
    /// </summary>
    public static class RegValidate
    {
        /// <summary>
        /// 必须包含字母数组和特殊字符，长度8~16 的正则 【可考虑添加其他特殊字符】
        /// </summary>
        public const string PwdReg = @"^(?=.*[0-9])((?=.*[A-Z])|(?=.*[a-z]))(?=.*[!@#$%^&*,\._])[0-9a-zA-Z!@#$%^&*,\\._]{8,16}$";
        /// <summary>
        /// 必须包含字母数组和特殊字符，长度8~16 的正则错误消息
        /// </summary>
        public const string PwdRegErrMsg = "必须包含字母数组和特殊字符，长度8~16";

        /// <summary>
        /// 必须包含大写、小写字母、数组和特殊字符，长度8~16 的正则
        /// </summary>
        public const string PwdReg1 = @"^(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z])(?=.*[!@#$%^&*,\._])[0-9a-zA-Z!@#$%^&*,\\._]{8,16}$";
        /// <summary>
        /// 必须包含大写、小写字母、数组和特殊字符，长度8~16 的正则错误消息
        /// </summary>
        public const string PwdReg1ErrMsg = "必须包含大小字母数组和特殊字符，长度8~16";

        /// <summary>
        /// 必须包含大写、小写字母、数组和特殊字符，长度至少6位 的正则
        /// </summary>
        public const string PwdReg2 = @"^\S*(?=\S{6,})(?=\S*\d)(?=\S*[A-Z])(?=\S*[a-z])(?=\S*[!@#$%^&*?])\S*$";
        /// <summary>
        /// 必须包含大写、小写字母、数组和特殊字符，长度至少6位 的正则错误消息
        /// </summary>
        public const string PwdReg2ErrMsg = "必须包含大小字母数组和特殊字符，长度至少6位";


        #region 密码强度的正则
        /// <summary>
        /// 密码强度正则。弱密码：全是数字或全是字母，6-16个字符
        /// </summary>
        public const string WeakPwdReg = @"^[0-9]{6,16}$|^[a-zA-Z]{6,16}$";
        /// <summary>
        /// 密码强度正则。中密码：数字和26个英文字母，6-16个字符
        /// </summary>
        public const string MediumPwdReg = @"^[A-Za-z0-9]{6,16}$";
        /// <summary>
        /// 密码强度正则。强密码：由数字、26个英文字母或者下划线组成的字符串，6-16个字符
        /// </summary>
        public const string StrongPwdReg = @"^\w{6,16}$";

        #endregion

        /// <summary>
        /// 手机号 的正则。ASP.NET Core 提供有 [Phone]，但考虑国内手机号的规则
        /// </summary>
        public const string PhoneReg = @" ^ 1(3\d|4[5-9]|5[0-35-9]|6[2567]|7[0-8]|8\d|9[0-35-9])\d{8}$";
        /// <summary>
        /// 手机号 的正则错误消息
        /// </summary>
        public const string PhoneRegErrMsg = "请输入正确的手机号";


        /// <summary>
        /// 邮箱地址 的正则，支持中文、多级域名。ASP.NET Core 提供有 [EmailAddress]，但考虑中文邮箱名(@前)
        /// 汉字的范围是 [\u4e00-\u9fa5]
        /// </summary>
        public const string EmailReg = @"^[A-Za-z0-9-_\u4e00-\u9fa5]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$";
        /// <summary>
        /// 邮箱地址 的正则错误消息，支持中文
        /// </summary>
        public const string EmailRegErrMsg = "请输入正确的邮箱";


        /// <summary>
        /// com、cn或net域名的邮箱地址 的正则，支持多级域名。ASP.NET Core 提供有 [EmailAddress]
        /// </summary>
        public const string EmailReg1 = @"^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(com|cn|net)$";
        /// <summary>
        /// com、cn或net域名的邮箱地址 的正则错误消息，支持多级域名
        /// </summary>
        public const string EmailReg1ErrMsg = "请输入正确的邮箱";


        /// <summary>
        /// Url 的正则。ASP.NET Core 提供有 [Url]
        /// </summary>
        public const string UrlReg = @"^((https?):)?\/\/([^?:/]+)(:(\d+))?(\/[^?]*)?(\?(.*))?";
        /// <summary>
        /// Url 的正则错误消息
        /// </summary>
        public const string UrlRegErrMsg = "请输入正确的Url";


        /// <summary>
        /// 16进制颜色 的正则。
        /// </summary>
        public const string ColorHexReg = @"^#([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$";
        /// <summary>
        /// 16进制颜色 的正则错误消息
        /// </summary>
        public const string ColorHexRegErrMsg = "请输入正确的Hex格式的颜色值(#开头)";


        /// <summary>
        /// 数字【或 金融金额】 的正则。
        /// 支持 负号-、千分位,、小数点.、小数最多保留2位
        /// </summary>
        public const string NumReg = @"^-?\d+(,\d{3})*(\.\d{1,2})?$";
        /// <summary>
        /// 数字【或 金融金额】 的正则错误消息
        /// </summary>
        public const string NumRegErrMsg = "请输入正确的Hex格式的颜色值(#开头)";


        /// <summary>
        /// 身份证号 的正则。
        /// </summary>
        public const string IDCardReg = @"^[1-9]\d{5}(18|19|20)\d{2}(0[1-9]|10|11|12)(0[1-9]|[1-2]\d|30|31)\d{3}[\dX]$";
        /// <summary>
        /// 身份证号 的正则错误消息
        /// </summary>
        public const string IDCardRegErrMsg = "身份证号码";


        /// <summary>
        /// 提取HTML元素属性 的正则。
        /// </summary>
        public const string ExtraHtmlReg = @"<\w+\s*([^>\/]*)\s*\/?\s*>";
        /// <summary>
        /// 提取HTML元素属性 的正则错误消息
        /// </summary>
        public const string ExtraHtmlRegErrMsg = "不是正确的HTML元素【用于判断Html/XML元素不太严格，比如未指定开头结尾^$、未考虑闭合标签】，但是可以用于提取";
        /* 参见如下js的实现，<\w+\s*([^>\/]*)\s*\/?\s*> 提取标签属性，然后 ([^\s=]+)(=(["'])(.*?)\3)? 全局模式(MatchAll) 获取每个属性对
         const str = `<div class="header-box" name="header">`
         const mt = str.match(/<\w+\s*([^>\/]*)\s*\/?\s*>/)
         // properties 即 属性字符串，即 class="header-box" name="header"
         const properties = mt[1]
         const mt1 = properties.match(/([^\s=]+)(=(["'])(.*?)\3)?/g)
         const obj = {}
         if (mt1) {
           mt1.forEach(p => {
             const kv = p.trim().split('=')
             obj[kv[0].trim()] = kv[1].trim().slice(1, -1)
           })
         }
         // obj => { class: 'header-box', name: 'header' }
         */

    }
}
```

基本参考自 [这些常用正则表达式是怎么写出来的？](https://juejin.cn/post/7073360739410378760#heading-3)【极其推荐这篇】

它和 [手机号验证最新正则表达式「建议收藏」](https://cloud.tencent.com/developer/article/2054311) 介绍的验证国内手机号吗，基本是最正确的。


> 汉字的范围是 `[\u4e00-\u9fa5]`

----------------------------------------


[c#最新验证是否电话号码和手机号码](https://blog.csdn.net/fangyuan621/article/details/120844904)

[ASP.NET Core通过特性实现参数验证](https://www.cnblogs.com/abnerwong/p/15204270.html)


 


[允许标记为PhoneAttribute或UrlAttribute的字段使用空字符串](https://cloud.tencent.com/developer/ask/sof/91353)


[邮箱、手机号正则表达式分析](https://juejin.cn/post/7047029397894725668)

[正则表达式——常用正则表达式验证有效数字、密码、真实姓名、邮箱、身份证号码及其正则捕获懒惰性原理](https://juejin.cn/post/7120506362680836127)


[C#-验证国内手机号码的最准确正则表达式](https://www.jianshu.com/p/37cb110604fb)

--------------------
-------------------------------------

[正则表达式不要背](https://juejin.cn/post/6844903845227659271)

[学会正则很简单，前端大佬悉心整理的正则表达式笔记](https://juejin.cn/post/7050007484953542692)

[【正则表达式】正则表达式简单总结](https://juejin.cn/post/7063733368461983752)

[厉害！这篇正则表达式竟写的如此详尽](https://juejin.cn/post/7132628898453880840#heading-0)

[正则表达式是如何运作的？](https://juejin.cn/post/7203285698073116727#heading-0)

