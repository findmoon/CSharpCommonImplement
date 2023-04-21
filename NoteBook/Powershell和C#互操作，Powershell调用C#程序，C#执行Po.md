**Powershell和C#互操作，Powershell调用C#程序，C#执行Powershell脚本**

[toc]

# PowerShell 执行 C# 代码

因为 PowerShell 基于 .NET，所以，PowerShell可以直接访问所有的 .NET 库，同时，也可以直接从 PowerShell 执行 C# 代码。

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

0. `Get-Content` 获取 C# 代码文件的内容。
1. `Add-Type` 读取 C# 代码，将class填加到 PowerShell session。
2. 对于静态方法，可以直接调用。
3. 对于实例方法，需要使用 `New-Object` 实例化类对象，然后调用实例方法。

```powershell
$source = Get-Content -Path ".\Calculator.cs"
Add-Type -TypeDefinition "$source"

# Call a static method
[Calculator]::Add(4, 3)

# Create an instance and call an instance method
$calculatorObject = New-Object Calculator
$calculatorObject.Multiply(5, 2)

```

# 参考

[How to Run C# Code from PowerShell](https://www.byteinthesky.com/powershell/how-to-run-c-sharp-code-from-powershell/)