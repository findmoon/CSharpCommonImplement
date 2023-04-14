**linux shell内置的type 查看一个命令类型、位置**

[toc]

type 命令用于查看一个命令的类型（是否是内建命令）、位置、别名等。

```sh
# type --help
type: type [-afptP] name [name ...]
    Display information about command type.

    For each NAME, indicate how it would be interpreted if used as a
    command name.

    Options:
      -a        display all locations containing an executable named NAME;
                includes aliases, builtins, and functions, if and only if
                the `-p' option is not also used

```

- `type type`

```sh
# type type
type is a shell builtin
```

- `type ls`

```sh
# type ls
ls is aliased to `ls --color=auto'
```

- `type date`

```sh
# type date
date is hashed (/usr/bin/date)
```
