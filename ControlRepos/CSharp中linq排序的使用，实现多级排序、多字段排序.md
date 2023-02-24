**C#中linq排序的使用，实现多级排序、多字段排序**

[toc]

# OrderBy 排序

直接参见官网 [对数据排序 (C#) ](https://learn.microsoft.com/zh-cn/dotnet/csharp/programming-guide/concepts/linq/sorting-data)示例

- **Primary Ascending Sort**

```C#
string[] words = { "the", "quick", "brown", "fox", "jumps" };  
  
IEnumerable<string> query = from word in words  
                            orderby word.Length  
                            select word;  
  
foreach (string str in query)  
    Console.WriteLine(str);  
  
/* This code produces the following output:  
  
    the  
    fox  
    quick  
    brown  
    jumps  
*/  
```

- **Secondary Descending Sort**

```C#
string[] words = { "the", "quick", "brown", "fox", "jumps" };  
  
IEnumerable<string> query = from word in words  
                            orderby word.Length, word.Substring(0, 1) descending  
                            select word;  
  
foreach (string str in query)  
    Console.WriteLine(str);  
  
/* This code produces the following output:  
  
    the  
    fox  
    quick  
    jumps  
    brown  
*/
```

# 示例

```C#
var myKeys = new string[] { "1:1",
  "3:1", "2:10", "2:3", "2:16", "2:9", "1:10", "3:3", "1:3", "1:16", "1:9", "2:6" };

var myKeys_Ordered1 = myKeys.OrderBy(k => k[2]).ThenBy(k => k[0]).ToArray();
// ["1:1","1:10","1:16","2:10","2:16","3:1","1:3","2:3","3:3","2:6","1:9","2:9"]

var myKeys_Ordered = myKeys.OrderBy(k => { 
    if(short.TryParse(k.Substring(2),out short r))
    {
        return r;
    }
    return 0;
}).ThenBy(k => k[0]).ToArray();
// [  "1:1",
//    "3:1",
//    "1:3",
//    "2:3",
//    "3:3",
//    "2:6",
//    "1:9",
//    "2:9",
//    "1:10",
//    "2:10",
//    "1:16",
//    "2:16"]
```

如下才是想要实现的效果，先按`:`后的排序，再按`:`前排序：

```C#
var myKeys = new string[] {"2:6", "1:1", "3:1", "1:3", "10:2", "3:2", "9:2", "10:1", "3:3", "5:1", "16:1", "9:1", "15:2" };

var myKeys_Ordered = myKeys.OrderBy(k => int.Parse(k.Split(':')[1])).ThenBy(k => int.Parse(k.Split(':')[0])).ToArray();

// [ "1:1",
//   "3:1",
//   "5:1",
//   "9:1",
//   "10:1",
//   "16:1",
//   "3:2",
//   "9:2",
//   "10:2",
//   "15:2",
//   "1:3",
//   "3:3",
//   "2:6" ]
```
# linq对象方法：OrderBy...ThenBy （标准查询运算符）

```C#
xxx.OrderBy(u=>u.id).ThenByDescendiing(u=>u.time);
xxx.OrderByDescending(u=>u.id).ThenByDescendiing(u=>u.time);
xxx.OrderByDescending(u=>u.id).ThenBy(u=>u.time);
```

# linq查询表达式：Orderby 字段

```C#
 var values=from u in xx
　　　　　　Orderby u.id ascending, u.time descending
　　　　　　select u;

var values=from u in xx
　　　　　　Orderby u.id descending , u.time ascending
　　　　　　select u;
```