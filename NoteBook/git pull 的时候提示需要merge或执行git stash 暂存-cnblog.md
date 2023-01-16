**git pull 的时候提示需要 merge 或执行 git stash 储藏**

[toc]

出现这个问题的原因在于 本地仓库工作目录中内容已经修改，但是当前修改没有 `add` 添加到暂存区、也没有 `commit` 提交到本地仓库。`git pull` 拉取时发现了修改冲突，要想 merge 合并，需要本地进行提交后进行冲突的合并处理，或者，执行 `git stash` 先将当前修改`储藏`起来（当前修改`储藏`后工作目录下将看不到，恢复为上一次提交版本的文件状态），后面进行恢复后的合并。

> 实际中 推荐使用 `git stash save`，可以添加相关的 `message` 信息，而不是 `git stash`

# git pull 提示需要 merge 或 git stash 的解决

> 除了此处介绍的使用 `git stash`，还可以根据需要执行本地的仓库的添加和提交。然后再拉取解决合并冲突的部分。
> 
> 此处用 `git stash` 只是想先将修改的部分`储藏`其后，后续决定是将其丢弃？，还是合并到拉取的最新仓库中？。

> [git pull 的时候提示需要merge](https://blog.csdn.net/weixin_39728460/article/details/83833811)

解决流程如下：

1. `git stash` 将修改`储藏`在本地

![](https://img2023.cnblogs.com/blog/1108935/202212/1108935-20221225215724027-170129037.png)  

2. `git stash list` 查看`储藏`的版本号，`stash@{0}`就是当前的版本号

![](https://img2023.cnblogs.com/blog/1108935/202212/1108935-20221225215723685-1366216246.png)  

3. `git pull` 拉取远程仓库

4. 保留`储藏`的内容，合并或解决合并冲突

4.1. `git stash pop stash@{0}` 还原`储藏`的内容。

还原时通常会执行合并，如果无法自动合并，则会列出有冲突的文件，需要手动处理冲突内容。

比如：

```sh
Auto-merging c/environ.c
CONFLICT (content): Merge conflict in c/environ.c
```

4.2. 处理文件的冲突部分

冲突的处理，就和正常 git 提交后冲突合并的处理一样了。如下，查看冲突的内容：

![](https://img2023.cnblogs.com/blog/1108935/202212/1108935-20221225215723077-1934662042.png)  

`Updated upstream` 和`=====`之间的内容就是`pull`下来的远程仓库内容；`=====`和`stashed changes`之间的内容就是本地修改的内容。根据实际情况决定保留哪个，还是两个修改都保留。

5. 删除`储藏`的内容 `git stash drop <stash@{id}>`

`git stash drop <stash@{id}>` 用于删除指定的`储藏`的`stash`。默认删除最新的，即 `stash@{0}`。

`git stash clear` 清除，删除所有`stash`。

> `git stash pop stash@{id}` 和 `git stash apply stash@{id}` 区别
> 
> `pop`执行完后，除了将`储藏`的修改恢复，还会自动删除此版本号的`储藏``stash`； `apply`则需手动删除对应的`储藏``stash`。

# git stash 的使用场景

> 其实从下面的解释可以了解到，stash 表示的是`储藏`，将所有未提交的修改（包括暂存的和非暂存的）都存储起来，用于以后的恢复。`储藏`更贴切。（之前称为暂存...）

`git stash`（git储藏）可用于以下情形：

- 发现有一个类是多余的，想删掉它又担心以后需要查看它的代码，想保存它但又不想增加一个脏的提交。这时就可以考虑`git stash`。

- 使用`git`的时候，往往使用分支（`branch`）解决任务切换问题，例如，我们往往会建一个自己的分支去修改和调试代码, 如果别人或者自己发现原有的分支上有个不得不修改的`bug`，通常会把完成一半的代码`commit`提交到本地仓库，然后切换分支去修改`bug`，改好之后再切换回来。这样的话往往`log`上会有大量不必要的记录。其实如果我们不想提交完成一半或者不完善的代码，但是却不得不去修改一个紧急`Bug`，那么，**使用`git stash`就可以将你当前未提交到本地（和服务器）的代码推入到 Git 的栈中，这时候你的工作区间和上一次提交的内容是完全一样的，所以你可以放心的修`Bug`，等到修完`Bug`，提交到服务器上后，再使用`git stash apply`将以前进行到一半的工作应用回来**。

- 经常有这样的事情发生，当你正在进行项目中某一部分的工作，里面的东西处于一个比较杂乱的状态，而你想转到其他分支上进行一些工作。问题是，你不想提交进行了一半的工作，但是不提交，以后你无法回到这个工作点。解决这个问题的办法就是`git stash`命令。**储藏(stash)可以获取你工作目录的中间状态 —— 也就是你修改过的被追踪的文件和暂存的变更 —— 并将它保存到一个未完结变更的堆栈中，随时可以重新应用**。

> 本部分及后续，主要出自 [git stash用法小结](https://www.cnblogs.com/tocy/p/git-stash-reference.html) 

# git stash 的用法

1. stash 储藏当前修改

**`git stash`会把所有未提交的修改（包括暂存的和非暂存的）都保存起来，用于后续恢复当前工作目录。**

`stash`是本地的，不会提交到本地`git`仓库，也不会通过`git push`命令上传到`git server`上。

**实际应用中推荐给每个`stash`加一个`message`，用于记录版本，使用`git stash save`取代`git stash`命令。**

如下所示：

```sh
$ git stash save "test-cmd-stash"
Saved working directory and index state On autoswitch: test-cmd-stash
HEAD 现在位于 296e8d4 remove unnecessary postion reset in onResume function
$ git stash list
stash@{0}: On autoswitch: test-cmd-stash
```

2. 重新恢复存储的 stash

通过 `git stash pop` 或 `git stash pop stash@{0}` 恢复之前存储的 stash。该命令会将缓存堆栈中的第一个`stash`删除，并将对应的修改应用(或恢复)到当前的工作目录下。

3. `git stash list`查看现有`stash`

4. `git stash drop` 移除`stash`

`git stash drop stash@{id}` 删除指定 id 的 `stash`，比如删除第一个最新的`stash`：`git stash drop` 或 `git stash drop stash@{0}`

5. `git stash clear` 删除所有缓存的`stash`

6. 查看指定`stash`的`diff`

`git stash show` 或 `git stash show stash@{id}` 查看`stash`的`diff`。

添加 `-p` 或 `--patch` 查看特定 stash 的全部 diff，比如：`git stash show -p`

7. `git stash branch <branch_name>` 从`stash`创建分支

这是一个很棒的捷径，用来恢复`储藏`的工作然后在新的分支上继续当时的工作。

如果你`储藏`了一些工作，暂时不去理会，然后继续在你`储藏`工作的分支上工作，后面在重新应用`储藏`的工作时可能会碰到一些问题。比如尝试应用的变更是针对一个你那之后修改过的文件，就会碰到一个归并冲突的问题，并且必须去手动解决它。如果你想用更方便的方法来重新检验你`储藏`的变更，可以运行 `git stash branch`，这会创建一个新的分支，检出你储藏工作时的所处的提交，重新应用你`存储`的工作，如果成功，将会丢弃`储藏stash`。

8. **`储藏`未跟踪（很重要），或忽略的文件**

默认情况下，`git stash`会缓存下列文件：

- 添加到暂存区的修改（`staged changes`）
- Git跟踪的但并未添加到暂存区的修改（`unstaged changes`）

但，**不会缓存以下文件：**

- 在工作目录中新的文件（`untracked files`）
- 被忽略的文件（`ignored files`）

`git stash` 命令提供了相关的参数用于缓存上面两种类型的文件：

- **使用 `-u` 或者 `--include-untracked` 可以 stash untracked 文件。**
- 使用 `-a` 或者 `--all` 选项可以 stash 当前目录下的所有修改。

