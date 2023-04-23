**Powershell和C#互操作，Powershell调用C#程序，C#执行Powershell脚本**

[toc]

# PowerShell 执行 C# 代码

因为 PowerShell 基于 .NET，所以，PowerShell可以直接访问所有的 .NET 库，同时，也可以直接从 PowerShell 执行 C# 代码。

## 读取 C# 文件代码

比如有如下的C#代码文件`Calculator.cs`：

```C#
public class Calculator
{
    private int _base;

    public Calculator(){}

    public Calculator(int base){
        _base = base;
    }

    public static int Add(int a, int b)
    {
        return a + b;
    }

    public int Multiply(int a, int b)
    {
        return a * b;
    }

    public int BasePlusMultiply(int a, int b)
    {
        return _base + a * b;
    }
}
```

这是一个非常简单的类，提供一个无参构造函数和一个有参构造函数，一个静态方法、两个实例方法。

PowerShell 执行 C# 代码文件的步骤如下：

> 本质是，**将C#文件内容读取为字符串，不能将C#代码放置在命名空间内，否则会导致`Add-Type`无法读取定义的类型**。
> 
> **即使使用 C# 10 的文件范围命名空间声明也不行，同样导致无法找到类型**。

0. `Get-Content` 获取 C# 代码文件的内容。
1. `Add-Type` 读取 C# 代码，将class填加到 PowerShell session。
2. 对于静态方法，可以直接调用。
3. 对于实例方法，需要使用 `New-Object` 实例化类对象，然后调用实例方法。

> PowerShell调用方法或构造函数，基本和C#的形式一样，`()`内直接传参即可。
> 
> `-Raw` 参数用于将 `Get-Content` 获取的结果作为变量（文本字符串变量）(传递给变量名)。

```powershell
$source = Get-Content -Raw -Path ".\Calculator.cs"
Add-Type -TypeDefinition "$source"

# Call a static method
[Calculator]::Add(4, 3)

# Create an instance and call an instance method
$calculatorObject = New-Object Calculator
$calculatorObject.Multiply(5, 2)

# Create an instance use constructor of params and call an instance method
$calculatorObject1 = New-Object Calculator(6)
$calculatorObject1.BasePlusMultiply(5, 2)
```

保存为脚本文件 `PowerShellCallCSharpCode.ps1`，执行结果如下：

```sh
PS E:\CSharp\CommonImplement\Test\Test\PowerShellExecCSharp> .\PowerShellCallCSharpCode.ps1
7
10
16
```

> 带有命名空间时，不知道 `[Calculator]` 改为 `[namespace.Calculator]` 是否可以正常使用？【未测试】
> 
> ```sh
> Line |
>    5 |  [Calculator]::Add(4, 3)
>      |  ~~~~~~~~~~~~
>      | Unable to find type [Calculator].
> ```

## 执行 C#代码 字符串

同样的，可以直接执行 C#代码组成的字符串。如下字符串变量：

> 成对的 `@" xxx "@` 输入多行字符串。

```sh
PS C:\>$source = @"
public class BasicTest
{
  public static int Add(int a, int b)
    {
        return (a + b);
    }
  public int Multiply(int a, int b)
    {
    return (a * b);
    }
}
"@

PS C:\>Add-Type -TypeDefinition $source
PS C:\>[BasicTest]::Add(4, 3)
PS C:\>$basicTestObject = New-Object BasicTest
PS C:\>$basicTestObject.Multiply(5, 2)
```

## 关于从 PowerShell 运行 C#程序源代码

上面的执行可以看到，仅仅是运行C#代码的字符串。没有命名空间等。

而如何实现执行真正的C#程序源代码，而不仅仅是一段C#代码的字符串？可以参考 [Weekend Scripter: Run C# Code from Within PowerShell](https://devblogs.microsoft.com/scripting/weekend-scripter-run-c-code-from-within-powershell/) 这篇介绍。




# 参考

- [Run a C# .cs file from a PowerShell Script](https://stackoverflow.com/questions/24868273/run-a-c-sharp-cs-file-from-a-powershell-script)

- 