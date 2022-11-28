**.NET(C#)中new Dictionary(字典)初始化值**

[.NET(C#)中new Dictionary(字典)初始化值(initializer默认值)](https://www.cjavapy.com/article/67/)

本文主要介绍在C#中new一个字典对象时，怎么指定字典的默认值。

**1、C# 4.0中指定默认值的方法**

```C#
class StudentName  
{  
    public string FirstName { get; set; }  
    public string LastName { get; set; }  
    public int ID { get; set; }  
}  
    Dictionary<int, StudentName> students = new Dictionary<int, StudentName>()  
    {  
        { 111, new StudentName {FirstName="Sachin", LastName="Karnik", ID=211}},  
        { 112, new StudentName {FirstName="Dina", LastName="Salimzianova", ID=317}},  
        { 113, new StudentName {FirstName="Andy", LastName="Ruth", ID=198}}  
    };  
//C# 4.0
Dictionary<string, string\> myDict =
    new Dictionary<string, string\> { { "key1", "value1" },
        { "key2", "value2" },
        { "key3", "value3" }
    };
Dictionary<string, List<string\>> myDictList =
    new Dictionary<string, List<string\>> { { "key1", new List<string\> () { "value1" } },
        { "key2", new List<string\> () { "value2" } },
        { "key3", new List<string\> () { "value3" } }
    };  
```

**2、C# 6.0中指定默认值的方法**

```C#
class StudentName  
{  
    public string FirstName { get; set; }  
    public string LastName { get; set; }  
    public int ID { get; set; }  
}  
   Dictionary<int, StudentName> students = new Dictionary<int, StudentName> () {  
                \[111\] = new StudentName { FirstName = "Sachin", LastName = "Karnik", ID = 211 },   
                \[112\] = new StudentName { FirstName = "Dina", LastName = "Salimzianova", ID = 317 },   
                \[113\] = new StudentName { FirstName = "Andy", LastName = "Ruth", ID = 198 }  
            };  
//C# 6.0
Dictionary<string, string\> dic =
    new Dictionary<string, string\> {
        \["key1"\] = "value1",
        \["key2"\] = "value2",
        \["key3"\] = "value3"
    };
Dictionary<string, List<string\>> dicList =
    new Dictionary<string, List<string\>> {
        \["key1"\] = new List<string\> () { "value1" },
        \["key2"\] = new List<string\> () { "value2" },
        \["key3"\] = new List<string\> () { "value3" }
    };
```
