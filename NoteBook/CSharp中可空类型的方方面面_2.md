**C#中可空值类型的类型判断、?. ?[] ?? ??=等运算符**

[toc]

# 可空值类型的类型及基础类型

**GetUnderlyingType() 获取可空类型的基础类型、使用 typeof 获取可空值类型的类型（而不是 GetType()）、不使用 is 判断可空类型**

## GetUnderlyingType 获取基础类型

`Nullable.GetUnderlyingType(type)` 获取可空(值)类型的基础类型，如果不为可空(值)类型（基础类型后面没有基础类型），则返回 `null`。

```C#
Debug.WriteLine(Nullable.GetUnderlyingType(typeof(int)));       // [无基础类型]
Debug.WriteLine(Nullable.GetUnderlyingType(typeof(int?)));  // System.Int32
Debug.WriteLine(Nullable.GetUnderlyingType(typeof(DateTime)));  // [无]
Debug.WriteLine(Nullable.GetUnderlyingType(typeof(DateTime?))); // System.DateTime
Debug.WriteLine(Nullable.GetUnderlyingType(typeof(MyTest)));    // [无]
// Debug.WriteLine(Nullable.GetUnderlyingType(typeof(MyTest?)));
```

因此，可以实现判断是否为可空类型的方法 `IsNullable`：

```C#
bool IsNullable(Type type) => Nullable.GetUnderlyingType(type) != null;
```

## typeof 获取可空值类型的类型

使用 `typeof` 获取可空值类型的 `System.Type` 实例，然后判断该类型是否为可空类型：

```C#
Debug.WriteLine($"int? is {(IsNullable(typeof(int?)) ? "nullable" : "non nullable")} value type");
Debug.WriteLine($"int is {(IsNullable(typeof(int)) ? "nullable" : "non-nullable")} value type");

// int? is nullable value type
// int is non-nullable value type
```

**上面获取可空类型的基础类型的方法，不要使用 `Object.GetType` 获取 Type 实例。这是因为在可空值类型的实例上调用 `Object.GetType` 方法，会先装箱为 Object 对象。** 可空值类型装箱的非空实例的基础类型等于值的装箱类型。即，`GetType` 返回的是装箱后对象的`Type`实例。

```C#
int? a = 17;
Type typeOfA = a.GetType();
Debug.WriteLine(typeOfA.FullName);  // System.Int32

int b = 16;
Type typeOfB = b.GetType();
Debug.WriteLine(typeOfB.FullName);  // System.Int32
```

## 不使用`is`判断可空类型实例

同样，不要使用`is`操作符判断可空类型的实例。如下所示，`is int`和`is int?`都适用于`int`和`int?`类型，无法通过 `is` 操作符判断是否是可空值类型还是基础类型。 

```C#
int? a = 16;
if (a is int)
{
    Debug.WriteLine("int? instance is compatible with int");
}
if (a is int?)
{
    Debug.WriteLine("int? instance is compatible with int?");
}

int b = 15;
if (b is int?)
{
    Debug.WriteLine("int instance is compatible with int?");
}
if (b is int)
{
    Debug.WriteLine("int instance is compatible with int");
}

// int? instance is compatible with int
// int? instance is compatible with int?
// int instance is compatible with int?
// int instance is compatible with int
```

> `underlying type` 基础类型（感觉也可以翻译为`原类型`），表示的是值类型、.NET CLS中规定的基础类型。对应可空值类型原来的类型。
> 
> `primitive types` 原始类型。 

# `?.`、`?[]` 空条件运算符 `Null-conditional operators` 不为空时取值

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

# `??` 和 `??=` 空合并运算符

- `??` 空合并运算符（`null-coalescing operator`，coalesce [ˌkəʊəˈles]）：如果表达式的值为`null`，则返回右边的值；如果不为`null`，则返回当前值。

空合并运算符`??` 表示，如果当前值为空时，则取后面的值，否则获取当前值。省去了对当前值是否为`null`的判断。类似于获取一个默认值。

> `??` 也翻译为 `空结合运算符`。

```C#
int? a = null;

Debug.WriteLine(a??double.NaN); // NaN

// b 默认值为 10
var b = a??10;
```

等同：

```C#
if(a == null){
    Debug.WriteLine(double.NaN); // NaN
    var b = 10;
}
else
{
    Debug.WriteLine(a); // NaN
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

空条件和空结合运算符一起使用：

```C#
List<int>? nullInts=null;
Debug.WriteLine($"nullInts的元素数量为：{nullInts?.Count ?? 0}");
nullInts = new List<int> { 1 };
Debug.WriteLine($"nullInts的元素数量为：{nullInts?.Count ?? 0}");

// nullInts的元素数量为：0
// nullInts的元素数量为：1
```

- `??=` 空合并赋值运算符（`null-coalescing assignment operator`）：当左边的变量为`null`时，将右边的值赋值给该变量；否则不赋值。（即，为空时赋值，不为空则不赋值）

```C#
List<int> numbers = null;
int? a = null;

(numbers ??= new List<int>()).Add(5);
Debug.WriteLine(string.Join(" ", numbers));  // 输出: 5

numbers.Add(a ??= 10);
Debug.WriteLine(string.Join(" ", numbers));  // 输出: 5 10
Debug.WriteLine(a);  // 输出: 10
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

# 参考

- [?? and ??= operators (C# reference)](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator)
- [Nullable value types](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-value-types)
