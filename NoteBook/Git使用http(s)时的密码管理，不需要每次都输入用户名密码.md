**Git使用http(s)时的密码管理，不需要每次都输入用户名密码【Windows凭据的管理和位置、git-credential、Git Credential Manager】**

[toc]

# Git 的认证凭据管理器

## Git Credential Manager

测试搭建本地git服务器时，突然发现 Git 使用 http(s) 连接操作远程仓库，不需要每次都输入用户名密码了。记得很早之前是需要每次push、pull时都要输入密码的。而现在不需要了，一定是进行了密码管理，将用户名密码保存后，在push、pull时再自动验证。


如下，在git clone时，提示输入用户名密码：

![](img/20230202093235.png)  

可以看到 Git Credential Manager 凭据管理器。后续的操作不再需要输入秘密。

## Git 的凭据管理

Mac系统下，git默认使用 osxkeychain 辅助程序来管理密码，每次需要提供用户名和密码的时候，osxkeychain辅助程序都默默填写好了。

Windows系统下，可以使用git-credential-winstore。如果安装Git时，安装了GitGUI，则提供的是`git-credential-manager`（基本也是默认的管理工具，Git-2.15开始，默认将`credential.helper`设置成manager）。

其他的设置项：`git-credential-store`在文件中用明文存储密码；`git-credential-cache`将密码保存在内存中。

**查看`credential.helper`的当前设置**：

```sh
$ git config --get credential.helper
manager
```

## 删除本地保存的 git 密码凭据【Windows下的凭据管理器】

> `git credential-manager uninstall` 命令应该可以清除当前的用户密码凭据。

默认 `Git Credential Manager` 会在第一次输入用户密码后，将其保存在Windows的 凭据管理器 中。

查看 git 保存的凭据：搜索“凭据管理器”，或，控制面板->用户账户->凭据管理器->Windows 凭据：

![](img/20230202151746.png)  

如果想要更改远程仓库连接的验证用户，可以在此处编辑，或删除后，重新输入其他权限的用户密码。

# 查看当前Git仓库的通信协议为 http(s) 还是 SSH

`git remote -v` 可以查看当前仓库的协议。

- **`git@`开头的为 SSH 通信**

```sh
$ git remote -v
origin  git@gitee.com:findmoon/csharp-my.git (fetch)
origin  git@gitee.com:findmoon/csharp-my.git (push)
```

- **`http`协议**

```sh
$ git remote -v
origin  http://192.168.103.130:5021/FirstRepo.git (fetch)
origin  http://192.168.103.130:5021/FirstRepo.git (push)
```

> **只有git协议才可以使用ssh-keys文件；https协议只支持账户密码输入。**

不同的协议，对应着不同的认证方式，也就决定了是否需要用户名密码，及如何管理认证。

SSH 连接直接配置公钥私钥，将公钥保存到Git服务器，之后直接使用即可。但即使如此，如果 `git clone` 等操作中使用是http协议，仍然需要输入用户名密码。

http连接则需要用户名密码，但可以考虑使用密码缓存、密码本地存储(一般明文保存在本地，不安全，不推荐)、认证凭据管理器等，实现密码的保存，在需要时git会直接使用，而不用每次都输入。

# 查看修改本地仓库的通信协议

查看当前远程仓库连接用的url（其实也是验证当前使用 http(s) 或 SSH 协议）

```sh
git config --get remote.origin.url
```

如下，返回url为 SSH 的 `git@xxx`：

```sh
$ git config --get remote.origin.url
git@gitee.com:findmoon/csharp-my.git
```

如下，返回url为http：

```sh
$ git config --get remote.origin.url
http://192.168.103.130:5021/FirstRepo.git
```

如果是`http`，每次操作远程库时，比如`git push`、`git pull`等，都需要用户名密码验证。只有修改当前连接远程仓库的协议，才能改用 SSH 的 ssh-keys 免密认证。

**`git remote set-url`修改远程仓库`origin`的url**，origin后为git协议的仓库地址：

```sh
git remote set-url origin git@gitee.com:findmoon/csharp-my.git
```

`git remote set-url`本质上修改的是本地仓库`.git`目录下的`config`文件：

![](img/20230202141721.png)  

# Git 中存储密码凭据的方式

## 方式一【全局】：命令行配置保存密码 credential.helper store

直接执行如下命令，启用 `credential.helper` 保存Git用户密码.

这个配置是全局的，也就是作用与当前系统(用户)下所有的库。

```sh
git config --global credential.helper store
```

- 该命令的作用，就是配置 用户目录 下的`.gitconfig`文件中增加 `helper = store`；

> Linux下的查看路径：`vim ~/.gitconfig`；Windows下当前用户路径：`%USERPROFILE%`

```ini
[user]
        name = user
        email = user@email.com
[credential]
        helper = store
```

在拉取或推送时，只需要输入一次密码，输入后会被保存在用户目录的`.git-credentials`，后续不在需要输入。

- 用户名和密码 保存在 用户目录 的 `.git-credentials` 文件中。

其文件内容的形式如下：

```sh
https://{username}:{password}@github.com
```

Github目前只能用Token码来代替密码（不支持直接使用密码），具体见[官方文档](https://link.zhihu.com/?target=https%3A//docs.github.com/en/github/authenticating-to-github/keeping-your-account-and-data-secure/creating-a-personal-access-token)。

在仍然使用http连接的情况下，可以将 `.git-credentials` 中的 `password` 改为 `Token`：

```sh
https://{你的github账号名}:{你的Token码}@github.com
```

## 方式二【全局】：命令行配置缓存密码 credential.helper cache （默认15分钟）

除了保存密码，为了增加一点点安全性，还可以设置缓存密码，设置缓存过期时间等。

```sh
git config --global credential.helper cache
git config --global credential.helper 'cache --timeout=3600'
```

## 方式三：`git config --global`命令中取消`--global`修改当前仓库

## 方式四：当前仓库免密码配置

编辑本地仓库目录下`.git`文件夹中的 config 配置文件，修改其中的 url：

```ini
[remote "origin"]
	url = http://192.168.103.130:5021/FirstRepo.git
```

为：

```ini
[remote "origin"]
	url = http://user:password@192.168.103.130:5021/FirstRepo.git
```

即，**在 `https://` 之后，增加 `用户名:密码@`。**

> 该方式等同于修改远程url：`git remote set-url origin http://user:password@192.168.103.130:5021/FirstRepo.git`

## 密码管理的其他操作

- 清除缓存（保存）在git中的用户名和密码

```sh
git credential-manager uninstall
```

- `git config --global credential.helper wincred` 使用Windows凭据保存用户名和密码



# 参考

- [git如何push时不输入密码?](https://www.zhihu.com/question/31836445)
- [Git 的使用——提交避免输入用户名和密码](https://zhuanlan.zhihu.com/p/358721423)
- [填坑:git-credential](https://blog.csdn.net/qq_37627370/article/details/88560887)