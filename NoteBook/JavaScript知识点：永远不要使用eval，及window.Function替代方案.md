**JavaScript知识点：永远不要使用eval，及window.Function替代方案**

[toc]

# eval() 介绍

**`eval()`** 函数会在 **当前作用域中** 执行传入的JavaScript代码字符串，返回字符串中代码的返回值。

```js
console.log(eval('2 + 2'));
// Expected output: 4

console.log(eval(new String('2 + 2')));
// Expected output: String { "2 + 2" }

console.log(eval('2 + 2') === eval('4'));
// Expected output: true

console.log(eval('2 + 2') === eval(new String('2 + 2')));
// Expected output: false
```

# 永远不要使用 eval！

1. `eval()` 容易受到攻击，其运行的字符串代码很容易被恶意修改，被运行恶意代码。
2. `eval()` 比其它替代方法速度更慢。因为它必须调用js解释器，而其他很多结构则可以被现代JS引擎优化。
3. 现代 JavaScript 解释器将 JavaScript 转换为机器代码，这就去除了额外的变量命名的概念。但是，使用`eval()`会强制浏览器进行冗长的变量名称查找，以确定变量在机器代码中的位置并设置其值。甚至，如果在eval中变更引用的变量类型，还会强制浏览器重新执行所有已经生成的机器代码以修正。

# window.Function 替代方案

- 使用eval实现的解析json的代码【不推荐】

```js
function looseJsonParse(obj){
    return eval("(" + obj + ")");
}
console.log(looseJsonParse(
   "{a:(4-1), b:function(){}, c:new Date()}"
))
```

- 使用Function实现的解析json的更好的代码【推荐】

```js
function looseJsonParse(obj){
    return Function('"use strict";return (' + obj + ')')();
}
console.log(looseJsonParse(
   "{a:(4-1), b:function(){}, c:new Date()}"
))
```

在没有 `eval` 的函数中，**即Function的执行，其对象在全局范围内被用来进行计算**，因此浏览器可以放心的假设 Date 是来自 window.Date 的而不是一个名为 Date 的局部变量。

如果在 `eval()` 的代码中，Date 可能被局部变量代替，如下所示：

```js
function Date(n){
    return ["Monday","Tuesday","Wednesday","Thursday","Friday","Saturday","Sunday"][n%7 || 0];
}
function looseJsonParse(obj){
    return eval("(" + obj + ")");
}
console.log(looseJsonParse(
   "{a:(4-1), b:function(){}, c:new Date()}"
))
// 输出：Object { a: 3, b: function(){}, c: [object Object] }
```

在 `eval()` 版本的代码中，浏览器被迫进行高代价的查找调用以检查是否存在名为 `Date()` 的任何局部变量。与 `Function()` 相比，这是非常低效的。

# Function() 内部如何调用局部变量



✖️



纯HTML和CSS实现一个简单通用的模态框