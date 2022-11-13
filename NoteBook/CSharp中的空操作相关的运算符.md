**弄明白C#中的可空类型、可空上下文、空操作和与?相关的运算符：?、?.、??、??=、!、?、IsNullOrWhiteSpace、MemberNotNull特性**

[toc]

> 可空上下文、空操作相关的运算符，需要 C# 8.0 。可空值类型在 C# 2.0 时就已经可用，C#8.0开始，引入了可空引用类型。

# `?` 可空类型声明

`?` 用于声明一个可空类型，包括 **可空引用类型、可空值类型**。

```C#
int? a=null;
string? b=null;
```

明确在可空类型不为空时，获取其成员或属性时，可以使用空包容运算符`!`：

```C#
b!.Length;
```

可空值类型是通过泛型可空类型 [System.Nullable<T>](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1) 实现的。其声明的简写形式就是 `T?`。

作为应用类型，`string?` 和 `string` 都由 `System.String` 类型表示。而值类型 `int?` 和 `int` 则由 `System.Nullable<System.Int32>` 和 `System.Int32` 表示。

# 可空上下文 Nullable contexts

可空上下文 用于表示所有默认类型不可为空，只有使用使用 `?` 显式声明的类型才是可空类型。

可空上下文的启用可以有效避免`System.NullReferenceException`异常的问题，只有明确可为空的变量，才能为null值，否则 **所有引用类型的变量都被解释为不可为空**。除非显式设置为null，这就避免的null值和null引用。

可空上下文的启用取值有：disable、enable、warnings、annotations(注释)。

**从 .net6 开始，项目默认是启用可空上下文的。**

`可空上下文/可空类型` 的启用方式有三种：

- 项目属性 -> 生成 -> 可为 Null 的类型：

![](img/20221113183447.png)  

- 项目文件`.csproj`中，添加`<Nullable>enable</Nullable>`节点：

```C#
<PropertyGroup>
  <OutputType>WinExe</OutputType>
  <TargetFramework>net6.0-windows</TargetFramework>
  <Nullable>enable</Nullable>
  <UseWPF>true</UseWPF>
</PropertyGroup>
```

- 预编译或预处理指令 `#nullable enable` 用于在文件中的一段代码内或范围中，局部启用可空上下文。

```C#
//enable表示启用
#nullable enable
//disable表示停用
#nullable disable
```

# 可空值类型的的判断（是否为null、是否有值、类型转换）

使用 类型模式的`is`操作符 或 `as`操作符，可以将 可空值类型 的值转换为值类型，若果为null，则不能转换。如下：

```C#
int? a = 31;
if (a is int valueOfA)
{
    Console.WriteLine($"a = {valueOfA}");
}
else
{
    Console.WriteLine("a 没有值");
}

// a = 31
```

`Nullable<T>.HasValue` 判断是否有值；`Nullable<T>.Value` 获取可空类型的值。

```C#
int? b = 10;
if (b.HasValue)
{
    Console.WriteLine($"b = {b.Value}");
}
else
{
    Console.WriteLine("b 没有值");
}

// b = 10
```

可以直接与 `null` 比较，判断可空类型是否为`null`：

```C#
int? c = 7;
if (c != null)
{
    Console.WriteLine($"c = {c.Value}");
}
else
{
    Console.WriteLine("c 没有值");
}

// c = 7
```

可空类型如果为null，强制类型转换会抛出异常：

```C#
int? n = null;

//int m1 = n;    // 不能编译
int n2 = (int)n; // 可以编译，但如果a为null，则抛出异常
```

# GetUnderlyingType() 获取可空类型的原类型、使用 typeof 获取可空值类型的类型（而不是 GetType()）



# `?.`、`?[]` 空条件运算符 `Null-conditional operators`

这是最常用的方式。`空条件运算符`用于成员访问或元素访问，仅在对象不为null时，才会返回成员或元素，否则返回null。

- 如果 `a` 等于 null，则 `a?.x` 或 `a?[x]` 的结果为 null。
- 如果 `a` 不等于 null，则 `a?.x` 或 `a?[x]` 等同于 `a.x` 或 `a[x]`。

```C#
A?.B?.Do(C);
A?.B?[C];
```

等同：

```C#
if(A != null){
    if(A.B != null){
        A.B.Do(C);
        A.B[C];
    }
}
```

# `??` 和 `??=`

- `??` 空合并运算符（`null-coalescing operator`，coalesce [ˌkəʊəˈles]）：如果表达式的值为`null`，则返回右边的值；如果不为`null`，则返回当前值。

空合并运算符`??` 表示，如果当前值为空时，则取后面的值，否则获取当前值。省去了对当前值是否为`null`的判断。类似于获取一个默认值。

```C#
int? a = null;

Console.WriteLine(a??double.NaN); // NaN

// b 默认值为 10
var b = a??10;
```

等同：

```C#
if(a == null){
    Console.WriteLine(double.NaN); // NaN
    var b = 10;
}
else
{
    Console.WriteLine(a); // NaN
    var b = a;
}
```

借助 `??` 可以方便的实现可空类型到非可空类型的转换：

```C#
int? a = 28;
int b = a ?? -1;

int? c = null;
int d = c ?? -1;
```


- `??=` 空合并赋值运算符（`null-coalescing assignment operator`）：当左边的变量为`null`时，将右边的值赋值给该变量；否则不赋值。

```C#
List<int> numbers = null;
int? a = null;

(numbers ??= new List<int>()).Add(5);
Console.WriteLine(string.Join(" ", numbers));  // 输出: 5

numbers.Add(a ??= 10);
Console.WriteLine(string.Join(" ", numbers));  // 输出: 5 10
Console.WriteLine(a);  // 输出: 10
```

等同于

```C#
if( numbers == null){
    numbers = new List<int>();
}
numbers.Add(5);

if( a == null){
    a = 10;
}
numbers.Add(a);
```

# IsNullOrWhiteSpace、MemberNotNull特性

# `!` 空包容或空抑制运算符 `null-forgiving operator`/`null-suppression operator`

`!`是一个一元后缀运算符，位于变量的后面。其实称之为 `空抑制运算符` 含义更加贴切。`!`用于获取一个可空类型的非空，或者 将可空类型转换为非空。

即 `x!` 声明 可为空的引用类型的表达式 `x` 不为 `null`。

空包容运算符在运行时不起作用，它仅通过更改表达式的 null 状态来影响编译器的静态流分析。在运行时其结果为 `x` 的结果。

> 一元前缀`!`运算符 是 逻辑非运算符。

比如一个最简单的使用：

```C#
Test MyTest2()
{
    Test? a = new Test();
    //Test? a = null ;

    var b = a ;

    var c = a!;

    return a!;
}
```

空包容运算符 可以用来测试参数的验证逻辑。比如下面的类：

```C#
#nullable enable
public class Person
{
    public Person(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

    public string Name { get; }
}
```

> `#nullable enable` 预处理指令 用于 启动可空类型。

在测试中，为了验证参数，可以如下处理：

```C#
[TestMethod, ExpectedException(typeof(ArgumentNullException))]
public void NullNameShouldThrowTest()
{
    var person = new Person(null!);
}
```

启用可空类型的情况下，是不允许将 null 传递给 string 类型的，需要为 `string?` 可空字符串才行。将会产生警告：`Warning CS8625: Cannot convert null literal to non-nullable reference type`

如何不使用 空包容运算符 ：

![](img/20221113161411.png)  

通过使用 包容运算符，告知编译器传递 null 是预期行为，不应发出警告。

在明确知道某个表达式不为 null 时，也可以使用 空包容运算符。比如，下面 `IsValid` 方法返回`true`，则说明其参数不是 null，则可以取消 null 类型引用：

```C#
public void NullTest()
{
    Person? p = new Person("John");
    if (IsValid(p))
    {
        Console.WriteLine($"Found {p!.Name}");
    }
}

public static bool IsValid(Person? person)
=> person is not null && person.Name is not null;
```

如果不使用 null包容运算符，编译器将为 `p.Name` 生成警告：`Warning CS8602: Dereference of a possibly null reference`。

`IsValid` 方法中，可以使用 `NotNullWhen` 特性，当方法返回 true 时，方法的参数不能是 null：

```C#
public static bool IsValid2([NotNullWhen(true)] Person? person)
=> person is not null && person.Name is not null;
```

# `?:` 条件运算符`conditional operator` 或 三元条件运算符

三元条件运算符的语法如下：当条件为真`true`时，取值`consequent`，否则（`false`时）取值`alternative`。

```C#
condition ? consequent : alternative
```

C#9.0 之前，条件表达式中两个可能取值的类型必须相同

```C#
var rand = new Random();

int? x = (rand.NextDouble() > 0.5) ? 12 : 0;

IEnumerable<int> xs = x ==0 ? new int[] { 0, 1 } : new int[] { 2, 3 };
```

从 C#9.0 开始，两个取值的类型可以相同，也可以能够隐式转换的类型。

```C#
var rand = new Random();

int? x = (rand.NextDouble() > 0.5) ? 12 : null;

IEnumerable<int> xs = x is null ? new List<int>() { 0, 1 } : new int[] { 2, 3 };
```

# 参考

- [?? and ??= operators (C# reference)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator)
- [! （null 包容）运算符](https://learn.microsoft.com/zh-cn/dotnet/csharp/language-reference/operators/null-forgiving)
- [Nullable value types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-value-types)