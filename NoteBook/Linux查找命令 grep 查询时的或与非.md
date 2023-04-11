**Linux查找命令 grep 查询时的或与非，即查询包含多个内容中一种、同时包含多个、不包含**

[toc]

grep —— print lines matching a pattern

我们以查询正在运行的进程为例。我通过`node server.js`启动了一个node进程作为后台守护进程运行。现在想要查找它的进程id。

# grep 或，满足包含多个内容中任何一个

## grep OR 使用 '|'

如下，查询包含 node 或 PID 的内容 `grep "node\|PID"` 【`\`不能省略，否则无效】

```sh
$ ps -aux | grep "node\|PID"
USER         PID %CPU %MEM    VSZ   RSS TTY      STAT START   TIME COMMAND
root         774  0.0  0.1  18292  2016 ?        Ss   Jan16   0:00 /usr/sbin/mcelog --ignorenodev --daemon --foreground
root     1520763  0.0  1.9 503916 33696 pts/0    Sl   12:03   0:00 node server.js
root     1546984  0.0  0.0  12140  1200 pts/0    R+   13:52   0:00 grep --color=auto node\|PID

```

## grep 正则 '-E'

此时就可以使用正则表达式查询了。

```sh
$ ps -aux | grep -E "node|PID"
USER         PID %CPU %MEM    VSZ   RSS TTY      STAT START   TIME COMMAND
root         774  0.0  0.1  18292  2016 ?        Ss   Jan16   0:00 /usr/sbin/mcelog --ignorenodev --daemon --foreground
root     1520763  0.0  1.9 503916 33696 pts/0    Sl   12:03   0:00 node server.js
root     1548028  0.0  0.0  12140  1124 pts/0    S+   13:56   0:00 grep --color=auto -E node|PID

```

## egrep 即 grep -E 的别名

`egrep` 就是 `grep -E`：

```sh
$ ps -aux | egrep "node|PID"
USER         PID %CPU %MEM    VSZ   RSS TTY      STAT START   TIME COMMAND
root         774  0.0  0.1  18292  2016 ?        Ss   Jan16   0:00 /usr/sbin/mcelog --ignorenodev --daemon --foreground
root     1520763  0.0  1.9 503916 33696 pts/0    Sl   12:03   0:00 node server.js
root     1548548  0.0  0.0  12140  1164 pts/0    R+   13:58   0:00 grep -E --color=auto node|PID

```

## grep -e 指定多个参数

使用 `grep -e` 选项你仅可以传递一个参数。在一个命令中使用多个 `-e` 选项可以指定多个OR条件。

```sh
$ ps -aux | grep -e node -e PID
USER         PID %CPU %MEM    VSZ   RSS TTY      STAT START   TIME COMMAND
root         774  0.0  0.1  18292  2016 ?        Ss   Jan16   0:00 /usr/sbin/mcelog --ignorenodev --daemon --foreground
root     1520763  0.0  1.9 503916 33696 pts/0    Sl   12:03   0:00 node server.js
root     1551252  0.0  0.0  12140  1048 pts/0    S+   14:09   0:00 grep --color=auto -e node -e PID
```


# grep 与，同时满足多个内容

grep 命令没有 AND 操作符。但是，可以模拟实现它。

## 使用正则 '-E pattern1.*pattern2'

`grep -E 'pattern1.*pattern2' filename` 查询文件内满足 `pattern1 任意字符 pattern2` 的内容

`grep -E 'pattern1.*pattern2|pattern2.*pattern1' filename` 保证要查询的 `pattern1` `pattern2` 前后顺序可以任意。

```sh
$ ps -aux | grep -E "no.*ser"
root     1520763  0.0  1.9 503916 33696 pts/0    Sl   12:03   0:00 node server.js
root     1555251  0.0  0.0  12140  1144 pts/0    S+   14:26   0:00 grep --color=auto -E no.*ser
```

## 使用多个 grep 命令

可以使用多个 ‘grep’ 命令加管道(pipe)的方式来实现 AND 操作。

`grep -E 'pattern1' filename | grep -E 'pattern2'`

`grep 'pattern1' filename | grep 'pattern2'`

```sh
$ ps -aux | grep "no" | grep "ser"
root     1520763  0.0  1.9 503916 33696 pts/0    Sl   12:03   0:00 node server.js
```

# grep 非 - NOT

使用 `grep -v` 你可以模拟NOT操作：

> -v, --invert-match
> 
> Invert the sense of matching, to select non-matching lines.

使用 `-v` 选项来反向匹配，选择不匹配的行。

`grep -v 'pattern1' filename`

# 参考

- [grep-or-and-not-operators](https://link.jianshu.com/?t=http://www.thegeekstuff.com/2011/10/grep-or-and-not-operators/)