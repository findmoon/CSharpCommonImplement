**html表单的readonly属性**

> 基本出自 [Readonly属性有啥用？](https://zhuanlan.zhihu.com/p/415585990)

# `readonly` 介绍

`readonly`用于表示表单控件不可变，即不可被用户修改。

`readonly`属性只能用于文本输入的表单控件，即`input`和`textarea`，`input`的`type`必须为 text|url|email|search|... 等这几种文本输入。

`input[type='file']` `input[type='radio']` 不能使用 `readonly`。

`select`/`button`表单控件不支持`readonly`，可以使用`disabled`属性。

如果一个`input`元素加了`readonly`属性,会有以下几点影响：

1. 不允许用户修改值；
2. 会被添加伪类`:read-only`（相对应的，如果没加readonly，则会有这个伪类`:read-write`）；
3. 不会参加表单校验。

# 与Disabled的不同

1. `readonly`态的表单项会被提交，`disabled`态的不会。这是最明显的差异；

2. `readonly`态的元素能被`focus`、能触发点击事件, `disabled`态的不能；

3. `disabled`态的表单元素往往会被浏览器加上特殊样式，如灰色背景等，`readonly`不会。

对于 Disabled 属性的表单元素来说：

- 表单详情页如果用禁用态表单往往满屏灰底输入框，比较丑；

- 有些浏览器disabled态输入框，内容无法被选中；

- 没法focus这一点，对无障碍阅读以及习惯用Tab键的人不友好。