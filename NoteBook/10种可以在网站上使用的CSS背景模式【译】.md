**10种可以在网站上使用的CSS背景模式【译】**

[toc]

正在为网站背景寻找灵感？参考这些可以在各种情况下使用的 CSS 模式。

![](img/20230310100924.png)

背景图案（`背景模式`）可以从根本上改变网站的外观。使用CSS可以轻松创建优雅的背景图案，这将使你的网站设计更上一层楼。

以下是可以在项目中直接使用的 10 种背景模式的列表。

> The code in these examples is available in a [GitHub repository](https://github.com/Yuvrajchandra/CSS-Background-Patterns) and is free for you to use under the [MIT license](https://choosealicense.com/licenses/mit/).

# 1. 黑色六边形 - The Black Hexagon

这种黑色六边形图案提供了一个非常整洁的六边形网格背景。在这种背景下，标题清晰可见。如果你正在设计任何技术或体系结构网站，则可以使用此模式。

![](img/20230310101359.png)

- HTML代码

```html
<h1>The Black Hexagon</h1>
```

- CSS代码

```css
body {
    font-family: 'Share Tech', sans-serif;
    font-size: 68px;
    color: white;
   display: flex;
    jsutify-content: center;
   align-items: center;
   margin: 0;
    width: 100vw;
    height: 100vh;
    text-shadow: 8px 8px 10px #0000008c;
    background-color: #343a40;
    background-image: url("data:image/svg+xml,%3Csvg xmlns='http://www.w3.org/2000/svg' width='28' height='49' viewBox='0 0 28 49'%3E%3Cg fill-rule='evenodd'%3E%3Cg id='hexagons' fill='%239C92AC' fill-opacity='0.25' fill-rule='nonzero'%3E%3Cpath d='M13.99 9.25l13 7.5v15l-13 7.5L1 31.75v-15l12.99-7.5zM3 17.9v12.7l10.99 6.34 11-6.35V17.9l-11-6.34L3 17.9zM0 15l12.98-7.5V0h-2v6.35L0 12.69v2.3zm0 18.5L12.98 41v8h-2v-6.85L0 35.81v-2.3zM15 0v7.5L27.99 15H28v-2.31h-.01L17 6.35V0h-2zm0 49v-8l12.99-7.5H28v2.31h-.01L17 42.15V49h-2z'/%3E%3C/g%3E%3C/g%3E%3C/svg%3E"), linear-gradient(to right top, #343a40, #2b2c31, #211f22, #151314, #000000);
}
 
h1 {
    margin: 20px;
}
```


# 2. 蓝带/蓝条 - The Blue Strips

蓝色条带背景图案使用线性渐变，即 CSS 属性 `linear-gradient` 在背景上创建渐变条带。

你可以更改背景颜色和渐变颜色以满足你的要求。

- HTML代码

```html
<div class="patterns pt1"></div>
```

- CSS代码

```css
body {
    margin: 0px;
}
 
.patterns {
    width: 100vw;
    height: 100vw;
}
 
.pt1 {
    background-size: 50px 50px;
    background-color: #0ae;
    background-image: -webkit-linear-gradient(rgba(255, 255, 255, .2) 50%, transparent 50%, transparent);
    background-image: -moz-linear-gradient(rgba(255, 255, 255, .2) 50%, transparent 50%, transparent);
    background-image: -ms-linear-gradient(rgba(255, 255, 255, .2) 50%, transparent 50%, transparent);
    background-image: -o-linear-gradient(rgba(255, 255, 255, .2) 50%, transparent 50%, transparent);
    background-image: linear-gradient(rgba(255, 255, 255, .2) 50%, transparent 50%, transparent);
}
```
