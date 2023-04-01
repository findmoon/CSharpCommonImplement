**查看PowerShell版本的几种方式总结**

[toc]

**推荐使用 `$PSVersionTable.PSVersion` 获取Powershell版本！**

# `$PSVersiontable`

## `$PSVersiontable`

```sh
> $PSVersiontable

Name                           Value
----                           -----
PSVersion                      5.1.19041.2364
PSEdition                      Desktop
PSCompatibleVersions           {1.0, 2.0, 3.0, 4.0...}
BuildVersion                   10.0.19041.2364
CLRVersion                     4.0.30319.42000
WSManStackVersion              3.0
PSRemotingProtocolVersion      2.3
SerializationVersion           1.1.0.1

```

```sh
> $PSVersiontable

Name                           Value
----                           -----
PSVersion                      7.3.3
PSEdition                      Core
GitCommitId                    7.3.3
OS                             Microsoft Windows 10.0.19045
Platform                       Win32NT
PSCompatibleVersions           {1.0, 2.0, 3.0, 4.0…}
PSRemotingProtocolVersion      2.3
SerializationVersion           1.1.0.1
WSManStackVersion              3.0

```

## `$PSVersiontable.PSVersion`

```sh
> $PSVersiontable.PSVersion

Major  Minor  Patch  PreReleaseLabel BuildLabel
-----  -----  -----  --------------- ----------
7      3      3
```

## 远程

```sh
> Invoke-Command -ComputerName 10.0.0.5 -ScriptBlock {$PSVersionTable.PSVersion} -Credential $cred

Major  Minor  Build  Revision PSComputerName
-----  -----  -----  -------- --------------
5      1      17763  592      10.0.0.5
```

# Get-Host

PowerShell has a concept known as hosts. A host is a program that is hosting the PowerShell engine. It is not the PowerShell engine itself. The PowerShell console or a code editor with an integrated terminal are PowerShell hosts.

## (Get-Host).Version

```sh
> (Get-Host).Version

Major  Minor  Build  Revision
-----  -----  -----  --------
7      3      3      -1
```

## 检查远程计算机上的 Powershell 版本

```sh
> Invoke-Command -ComputerName 10.0.0.5 -ScriptBlock {Get-Host} -Credential $cred

Major  Minor  Build  Revision PSComputerName
-----  -----  -----  -------- --------------
1      0      0      0        10.0.0.5
```

# `$host.Version`

## `$host.Version`

`$host` 的返回结果 与 `Get-Host` 相同。

`$host.Version`查看：

```sh
> $host.Version

Major  Minor  Build  Revision
-----  -----  -----  --------
7      3      3      -1
```

## `$host.Version`检查远程计算机的PS版本

```sh
> Invoke-Command -ComputerName 10.0.0.5 -ScriptBlock {$host.Version} -Credential $cred

Major  Minor  Build  Revision PSComputerName
-----  -----  -----  -------- --------------
1      0      0      0        10.0.0.5
```

# 注册表

## `Get-ItemProperty` 获取注册表

`HKLM:\SOFTWARE\Microsoft\PowerShell\3\PowerShellEngine` 中 `PowerShellVersion`。

使用 `Get-ItemProperty` 获取：

```sh
> (Get-ItemProperty -Path HKLM:\SOFTWARE\Microsoft\PowerShell\3\PowerShellEngine -Name 'PowerShellVersion').PowerShellVersion
5.1.17134.1
```

**[version] 转换下版本类型**

```sh
> [version](Get-ItemProperty -Path HKLM:\SOFTWARE\Microsoft\PowerShell\3\PowerShellEngine -Name 'PowerShellVersion').PowerShellVersion

Major  Minor  Build  Revision
-----  -----  -----  --------
5      1      17134  1
```

## `reg query` 查询注册表

```sh
> reg query HKLM\SOFTWARE\Microsoft\PowerShell\3\PowerShellEngine /v PowerShellVersion

HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\3\PowerShellEngine
    PowerShellVersion    REG_SZ    5.1.19041.1
```

## 远程计算机上的注册表

```sh
> $scriptBlock = {
    [version](Get-ItemProperty -Path HKLM:\SOFTWARE\Microsoft\PowerShell\3\PowerShellEngine -Name 'PowerShellVersion').PowerShellVersion
}
> Invoke-Command -ComputerName 10.0.0.5 -ScriptBlock $scriptBlock -Credential $cred

Major  Minor  Build  Revision PSComputerName
-----  -----  -----  -------- --------------
5      1      17763  1        10.0.0.5
```

# 参考

- [4 Ways to Check your PowerShell Version (Good AND Bad)](https://adamtheautomator.com/powershell-version/)