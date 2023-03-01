C#中的DirectorySecurity、FileSystemAccessRule处理目录、文件的用户权限


```C#
/// <summary>
/// 设置文件夹权限 处理给EVERONE赋予所有权限
/// </summary>
/// <param name="FileAdd">文件夹路径</param>
public void SetFileRole()
{
    string FileAdd = this.Context.Parameters["targetdir"].ToString();
    FileAdd = FileAdd.Remove(FileAdd.LastIndexOf('\\'), 1);
    DirectorySecurity fSec = new DirectorySecurity();
    fSec.AddAccessRule(new FileSystemAccessRule("Everyone", FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
    System.IO.Directory.SetAccessControl(FileAdd, fSec);
}
```