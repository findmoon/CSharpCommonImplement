**C#生成唯一值的方法汇总【转】**

[toc]

> 原文 [C#生成唯一值的方法汇总](https://blog.csdn.net/huwei2003/article/details/79882353)
>


生成唯一值的方法很多，下面就不同环境下生成的唯一标识方法一一介绍  
  
一、在 .NET 中生成  
  
1、直接用.NET Framework 提供的 Guid() 函数，此种方法使用非常广泛。GUID（全局统一标识符）是指在一台机器上生成的数字，它保证对在同一时空中的任何两台计算机都不会生成重复的 GUID 值（即保证所有机器都是唯一的）。关于GUID的介绍在此不作具体熬述，想深入了解可以自行查阅MSDN。代码如下：  
  

```csharp
  using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 namespace ConsoleApplication1
 {
     class Program
     {
         static void Main(string[] args)
         {
             string _guid = GetGuid();
             Console.WriteLine("唯一码：{0}\t长度为：{1}\n去掉连接符：{2}", _guid, _guid.Length, _guid.Replace("-", ""));
             string uniqueIdString = GuidTo16String();
             Console.WriteLine("唯一码：{0}\t长度为：{1}", uniqueIdString, uniqueIdString.Length);
             long uniqueIdLong = GuidToLongID();
             Console.WriteLine("唯一码：{0}\t长度为：{1}", uniqueIdLong, uniqueIdLong.ToString().Length);
         }
         /// <summary>
         /// 由连字符分隔的32位数字
         /// </summary>
         /// <returns></returns>
         private static string GetGuid()
         {
             System.Guid guid = new Guid();
             guid = Guid.NewGuid();
             return guid.ToString();
         }
         /// <summary>  
         /// 根据GUID获取16位的唯一字符串  
         /// </summary>  
         /// <param name=\"guid\"></param>  
         /// <returns></returns>  
         public static string GuidTo16String()
         {
             long i = 1;
             foreach (byte b in Guid.NewGuid().ToByteArray())
                 i *= ((int)b + 1);
             return string.Format("{0:x}", i - DateTime.Now.Ticks);
         }
         /// <summary>  
         /// 根据GUID获取19位的唯一数字序列  
         /// </summary>  
         /// <returns></returns>  
         public static long GuidToLongID()
         {
             byte[] buffer = Guid.NewGuid().ToByteArray();
             return BitConverter.ToInt64(buffer, 0);
         }   
     }
 }
```

  
2、用 DateTime.Now.ToString("yyyyMMddHHmmssms") 和 .NET Framework 提供的 RNGCryptoServiceProvider() 结合生成，代码如下：  
  

```csharp
 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 using System.Threading;
 namespace ConsoleApplication1
 {
     class Program
     {
         static void Main(string[] args)
         {
             string uniqueNum = GenerateOrderNumber();
             Console.WriteLine("唯一码：{0}\t 长度为：{1}", uniqueNum, uniqueNum.Length);
             //测试是否会生成重复
               Console.WriteLine("时间+RNGCryptoServiceProvider()结合生成的唯一值，如下：");
             string _tempNum = string.Empty;
             for (int i = 0; i < 1000; i++)
             {
                 string uNum = GenerateOrderNumber();
                 Console.WriteLine(uNum);
                 if (string.Equals(uNum, _tempNum))
                 {
                     Console.WriteLine("上值存在重复，按Enter键继续");
                     Console.ReadKey();
                 }
                 //Sleep当前线程，是为了延时，从而不产生重复值。可以把它注释掉测试看
                 Thread.Sleep(300);
                 _tempNum = uNum;
             }
         }
         /// <summary>
         /// 唯一订单号生成
         /// </summary>
         /// <returns></returns>
         public static string GenerateOrderNumber()
         {
             string strDateTimeNumber = DateTime.Now.ToString("yyyyMMddHHmmssms");
             string strRandomResult = NextRandom(1000, 1).ToString();
             return strDateTimeNumber + strRandomResult;
         }
         /// <summary>
         /// 参考：msdn上的RNGCryptoServiceProvider例子
         /// </summary>
         /// <param name="numSeeds"></param>
         /// <param name="length"></param>
         /// <returns></returns>
         private static int NextRandom(int numSeeds, int length)
         {
             // Create a byte array to hold the random value.  
             byte[] randomNumber = new byte[length];
             // Create a new instance of the RNGCryptoServiceProvider.  
             System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
             // Fill the array with a random value.  
             rng.GetBytes(randomNumber);
             // Convert the byte to an uint value to make the modulus operation easier.  
             uint randomResult = 0x0;
             for (int i = 0; i < length; i++)
             {
                 randomResult |= ((uint)randomNumber[i] << ((length - 1 - i) * 8));
             }
             return (int)(randomResult % numSeeds) + 1;
         }
     }
 }
```

  
3、用 `[0-9A-Z] + Guid.NewGuid()` 结合生成特定位数的唯一字符串，代码如下：  
  

```csharp
using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 namespace ConsoleApplication1
 { 
     class Program
     {
         static void Main(string[] args)
         {
             string uniqueText = GenerateUniqueText(8);
             Console.WriteLine("唯一码：{0}\t 长度为：{1}", uniqueText, uniqueText.Length);
             //测试是否会生成重复 
               Console.WriteLine("由[0-9A-Z] + NewGuid() 结合生成的唯一值，如下：");
             IList<string> list = new List<string>();
             for (int i = 1; i <= 1000; i++)
             {
                 string _uT = GenerateUniqueText(8);
                 Console.WriteLine("{0}\t{1}", list.Count, _uT);
                 if (list.Contains(_uT))
                 {
                     Console.WriteLine("{0}值存在重复", _uT);
                     Console.ReadKey();
                 }
                 list.Add(_uT);
                 //if (i % 200 == 0)
                 //{
                     //Console.WriteLine("没有重复，按Enter键往下看");
                     //Console.ReadKey();
                 //}
             }
             list.Clear();
         }
 
         /// <summary>
         /// 生成特定位数的唯一字符串
         /// </summary>
         /// <param name="num">特定位数</param>
         /// <returns></returns>
         public static string GenerateUniqueText(int num)
         {
             string randomResult = string.Empty;
             string readyStr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
             char[] rtn = new char[num];
             Guid gid = Guid.NewGuid();
             var ba = gid.ToByteArray();
             for (var i = 0; i < num; i++)
             {
                 rtn[i] = readyStr[((ba[i] + ba[num + i]) % 35)];
             }
             foreach (char r in rtn)
             {
                 randomResult += r;
             }
             return randomResult;
         }
     }
 }
```

4、用日期 用 DateTime.Now.ToString("yyyyMMddHHmmssms") + Guid.NewGuid()作种子生成随机数组合生成唯一值，代码如下：

```csharp
/// <summary>
/// 生成订单编码,20位，日期部分在二次随机数
/// </summary>
/// <returns></returns>
public static string CreateOrderNo()
{
    var strNo = DateTime.Now.ToString("yyMMddHHmmssms");
        var iSeed = CreateRandSeed();
        var iSeed2 = NextRandom(1000, 2);
        iSeed = iSeed - 500 + iSeed2;
        var rnd = new Random(iSeed);
        var strExt = rnd.Next(1000, 10000);
        strNo += strExt;

        //再拼接2位
        rnd = new Random(iSeed - 1);
        strExt = rnd.Next(10, 99);
        strNo += strExt;
        return strNo;
}
/// <summary>  
/// 参考：msdn上的RNGCryptoServiceProvider例子  
/// </summary>  
/// <param name="numSeeds"></param>  
/// <param name="length"></param>  
/// <returns></returns>  
public static int NextRandom(int numSeeds, int length)
{
    // Create a byte array to hold the random value.    
    byte[] randomNumber = new byte[length];
    // Create a new instance of the RNGCryptoServiceProvider.    
    System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
    // Fill the array with a random value.    
    rng.GetBytes(randomNumber);
    // Convert the byte to an uint value to make the modulus operation easier.    
    uint randomResult = 0x0;
    for (int i = 0; i < length; i++)
    {
        randomResult |= ((uint)randomNumber[i] << ((length - 1 - i) * 8));
    }
    return (int)(randomResult % numSeeds) + 1;
}  
public static int CreateRandSeed()
{
    var iSeed = 0;
    var guid = Guid.NewGuid().ToString();
    iSeed = guid.GetHashCode();
    return iSeed;
} 
```

  
  
  
二、在JS中生成GUID，类似.NET中的 Guid.NewGuid()，代码如下：  

```javascript
 function newGuid() { //方法一：
     var guid = "";
     var n = (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
     for (var i = 1; i <= 8; i++) {
         guid += n;
     }
     return guid;
 }
 function newGuid() { //方法二：
     var guid = "";
     for (var i = 1; i <= 32; i++) {
         var n = Math.floor(Math.random() * 16.0).toString(16);
         guid += n;
         if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
             guid += "-";
     }
     return guid;
 }
```

  
三、在SQL存储过程生成GUID，代码如下：  
  

```sql
-- =============================================
 -- Author:      JBen
 -- Create date: 2012-06-05
 -- Description: 生成唯一标识ID，公共存储过程，可设置在别的存储过程调用此存储过程传不同的前缀
 -- =============================================
 ALTER PROCEDURE [dbo].[pro_CreateGuid] 
     @Prefix NVARCHAR(10),
     @outputV_guid NVARCHAR(40) OUTPUT
 AS
 BEGIN
     -- SET NOCOUNT ON added to prevent extra result sets from
     -- interfering with SELECT statements.
     SET NOCOUNT ON;
     -- Insert statements for procedure here
     SET @outputV_guid = @Prefix + REPLACE(CAST(NEWID() AS VARCHAR(36)),'-','')
 END
```