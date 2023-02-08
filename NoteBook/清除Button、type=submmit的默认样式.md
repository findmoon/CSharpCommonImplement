**清除Button、type=submmit、input的默认样式、placeholder的文字颜色**

[toc]

- `<button>按钮</button>`、`<input type="submit" name="" value="登陆" />`、`<button type="submit">提交</button>`

```css
button, [type='submmit']{
    margin: 0;
    padding: 0;
    border: none;   //  1px solid transparent;  //自定义边框
    outline: none;  //  消除默认点击蓝色边框效果
    background-color: transparent;
}
```

- input 输入框的默认属性

```css
 input{  
	background:none;  
	outline:none;  
	border:none;
  }
```

- 去掉 input 选中时的蓝色框

```css
input:focus{
    border:none;
}
```

- 修改 placeholder属性 的文字颜色

```css
input::-webkit-input-placeholder { 
    /* WebKit browsers */ 
    color: #ccc; 
} 
input:-moz-placeholder { 
    /* Mozilla Firefox 4 to 18 */ 
    color: #ccc; 
} 

/* 不确定是否正确 */
input::-moz-input-placeholder { 
    /* Mozilla Firefox 19+ */ 
    color: #ccc; 
} 

input:-ms-input-placeholder { 
    /* Internet Explorer 10+ */ 
    color: #ccc; 
}
```

- `input[type=search]` 去除输入框中系统自带的清除小图标

```css
input[type=search]::-webkit-search-cancel-button{
    -webkit-appearance: none;  /* 此处去掉默认的小× */
    appearance:none;
}
```

- input 清除自动填充的背景

`input`选中时会有自动填充的下拉选项功能，此时会有一个默认的背景，可以使用下面方法清除

```css
input:-webkit-autofill {
  transition: background-color 5000s ease-in-out 0s;
}
```

改变字体样式

```css
input {
	-webkit-text-fill-color: #9cc5ec;  //颜色是设置成你需要的颜色
}
```

- 去除input[type=number]的默认样式

```css
input[type=number] {
    -moz-appearance:textfield;
}
input[type=number]::-webkit-inner-spin-button,
input[type=number]::-webkit-outer-spin-button {
    -webkit-appearance: none;
    margin: 0;
}
```

- CSS清除浏览器默认样式

浏览器很多的默认样式并不友好，但同时很多默认样式也很有用。关于对其清除或重置，可以参考 [CSS清除浏览器默认样式](https://blog.csdn.net/qq_36667170/article/details/104689877)


**简单粗暴的处理**：

```css
*{
	margin:0px;
	padding: 0px;
}
```

**CSS reset**：将所有样式都去除，后续还需要每个都重新设置

官网：https://meyerweb.com/eric/tools/css/reset/


**CSS Normalize**：一个可以定制的CSS文件，它让不同的浏览器在渲染网页元素的时候形式更统一。

`Normalize.css` 能干什么：

保留有用的默认值，不同于许多 CSS 的重置
标准化的样式，适用范围广的元素。
纠正错误和常见的浏览器的不一致性。
一些细微的改进，提高了易用性。


项目地址：https://github.com/necolas/normalize.css/


- [CSS：关于 input 框 “-webkit-appearance: none”样式使用问题](https://blog.csdn.net/weixin_42220533/article/details/82348509)