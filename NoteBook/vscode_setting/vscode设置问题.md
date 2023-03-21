**关于 VSCode 设置的问题**

1. `!+tab`无法自动生成html片段的问题。

应该很早之前出现过这个问题，当时的解决办法是，在 vscode 的设置中，修改`emmet`，比如 `"emmet.triggerExpansionOnTab": true`。

这次出现后，也尝试过升级vscode（也可能是升级导致的tab无效）。

如下，是`settings.json`文件中已有的设置，tab触发已经打开。

```json
    "emmet.excludeLanguages": [
        "markdown",
        "html"
    ],
    "emmet.triggerExpansionOnTab": true,
    "emmet.showSuggestionsAsSnippets": true,
```

**`!+tab`、`.class+tab`(css语法生成html)都是不可用的，这种补全都是依靠 Emmet 实现的，和其设置有关。**

如果设置无效，**还可以通过输入 `html5`、`html:5` 等 + tab 的形式生成HTML片段，替代`!+tab`。**

最后，在死马当活马骑医的情况下，尝试删除上面`emmet.triggerExpansionOnTab`、`emmet.excludeLanguages`、`emmet.showSuggestionsAsSnippets`等的设置。然后 重启vscode，竟然发现 **`!+tab`、`.class+tab`(css语法生成html)恢复正常了！！！**

2. 去除入下

```json
	"[javascript]": {
		"editor.defaultFormatter": "HookyQR.beautify"
	},
```

