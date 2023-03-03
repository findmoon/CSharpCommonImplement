**C#中的DirectorySecurity、FileSystemAccessRule处理目录、文件的用户权限**

[toc]

# 设置文件、文件夹权限

如下，简单的设置文件、文件夹权限的帮助类`FileDirRightHelper`：

```C#
/// <summary>
/// 文件文件夹权限设置帮助类
/// </summary>
public class FileDirRightHelper
{
    /// <summary>
    /// 设置文件夹权限 处理给EVERONE赋予所有权限
    /// </summary>
    /// <param name="dirPath">文件夹路径</param>
    /// <param name="role">角色，默认Users。常用的有Everyone、IIS_IUSRS、Administrators</param>
    /// <param name="rights">权限，默认 FileSystemRights.Modify</param>
    /// <param name="access">设置权限还是禁止权限</param>
    public static void SetDirRights(string dirPath, string role = "Users", FileSystemRights rights= FileSystemRights.Modify, AccessControlType access= AccessControlType.Allow)
    {
        ArgumentGuard.NotNullEmptyOrWhiteSpace(dirPath, nameof(dirPath),otherParams:role);
        DirectorySecurity fSec = new DirectorySecurity();
        fSec.AddAccessRule(new FileSystemAccessRule(role, rights, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, access));
        System.IO.Directory.SetAccessControl(dirPath, fSec);
    }


    /// <summary>
    /// 设置文件夹权限 处理给EVERONE赋予所有权限
    /// </summary>
    /// <param name="filePath">文件路径</param>
    /// <param name="role">角色，默认Users。常用的有Everyone、IIS_IUSRS、Administrators</param>
    /// <param name="rights">权限，默认 FileSystemRights.Modify</param>
    /// <param name="access">设置权限还是禁止权限</param>
    public static void SetFileRights(string filePath, string role = "Users", FileSystemRights rights = FileSystemRights.Modify, AccessControlType access = AccessControlType.Allow)
    {
        ArgumentGuard.NotNullEmptyOrWhiteSpace(filePath, nameof(filePath),null,role);
        FileSecurity fSec = new FileSecurity();
        fSec.AddAccessRule(new FileSystemAccessRule(role, rights, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, access));
        File.SetAccessControl(filePath, fSec);
    }
}
```

# 统一的参数检查方法，确保参数不为空 或 空字符串

`ArgumentException`、`ArgumentNullException`：

```C#
/// <summary>
/// 确保参数的类，检查参数不为null、empty
/// </summary>
public static class ArgumentGuard
{
    /// <summary>
    /// param、otherParams用于确保不为Null
    /// </summary>
    /// <param name="param"></param>
    /// <param name="paramName"></param>
    /// <param name="otherParams"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void NotNull(object param, string paramName, params string[] otherParams)
    {
        if (param is null)
        {
            throw new ArgumentNullException(paramName);
        }
        foreach (var otherParam in otherParams)
        {
            if (otherParam is null)
            {
                throw new ArgumentNullException();
            }
        }
    }

    public static void NotNullOrEmpty(string param, string paramName, string specifyexMsg = null)
    {
        if (string.IsNullOrEmpty(param))
        {
            throw new ArgumentException(specifyexMsg ?? "The string can not be empty.", paramName);
        }
    }
    /// <summary>
    /// param、otherParams用于确保不为NullEmptyOrWhiteSpace
    /// </summary>
    /// <param name="param"></param>
    /// <param name="paramName"></param>
    /// <param name="specifyexMsg">指定的特殊错误内容</param>
    /// <param name="otherParams"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void NotNullEmptyOrWhiteSpace(string param, string paramName, string specifyexMsg = null,params string[] otherParams)
    {
        if (string.IsNullOrWhiteSpace(param))
        {
            throw new ArgumentException(specifyexMsg ?? "The string can not be empty or WhiteSpace.", paramName);
        }
        foreach (var otherParam in otherParams)
        {
            if (string.IsNullOrWhiteSpace(otherParam))
            {
                throw new ArgumentException(specifyexMsg ?? "The string can not be empty or WhiteSpace.");
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="param"></param>
    /// <param name="paramName"></param>
    /// <param name="specifyexMsg">指定的特殊错误内容</param>
    /// <exception cref="ArgumentException"></exception>
    public static void NotNullOrEmpty<T>(IEnumerable<T> param, string paramName,string specifyexMsg=null)
    {
        if (param is null || param.Count() == 0)
        {
            throw new ArgumentException(specifyexMsg??"The collection can not be null or empty.", paramName);
        }
    }
}
```