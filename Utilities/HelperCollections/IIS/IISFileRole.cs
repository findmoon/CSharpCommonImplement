using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HelperCollections.IIS
{
    /// <summary>
    /// 设置IIS相关的文件/文件夹的权限 设置文件权限
    /// </summary>
    public class IISFileRole
    {
        #region 设置文件夹权限
        /// <summary>
        /// 设置 NetfxV4TmpASPFiles 文件夹的权限为 IIS_IUSR 【可能不存在路径】
        /// 当前标识(IIS APPPOOL\xxx)没有对“C:\Windows\Microsoft.NET\Framework\v4.0.30319\Temporary ASP.NET Files”的写访问权限。
        /// </summary>
        /// <param name="rights">权限，默认修改权限。不推荐 FileSystemRights.FullControl</param>
        public static void SetDirIIS_IUSRRole_forNetfxV4TmpASPFiles(FileSystemRights rights = FileSystemRights.Modify)
        {
            var targetDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Microsoft.NET\Framework\v4.0.30319\Temporary ASP.NET Files");

            SetDirIIS_IUSRSRole(targetDir, rights);
        }
        /// <summary>
        /// 设置文件夹权限 处理给EVERONE赋予所有权限
        /// </summary>
        /// <param name="targetDir">文件夹路径</param>
        /// <param name="rights">权限，默认修改权限。不推荐 FileSystemRights.FullControl</param>
        public static void SetDirEveryoneRole(string targetDir, FileSystemRights rights = FileSystemRights.Modify)
        {
            SetDirRights(targetDir, "Everyone", rights);
        }
        /// <summary>
        /// 设置文件夹权限 处理给EVERONE赋予所有权限
        /// </summary>
        /// <param name="targetDir">文件夹路径</param>
        /// <param name="rights">权限，默认修改权限。不推荐FileSystemRights.FullControl</param>
        public static void SetDirIIS_IUSRSRole(string targetDir, FileSystemRights rights= FileSystemRights.Modify)
        {
            SetDirRights(targetDir, "IIS_IUSRS", rights);
        }
        /// <summary>
        /// 设置文件夹权限
        /// </summary>
        /// <param name="targetDir">文件夹路径</param>
        /// <param name="userOrUserGroup"></param>
        /// <param name="rights"></param>
        public static void SetDirRights(string targetDir,string userOrUserGroup, FileSystemRights rights)
        {
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }
            DirectorySecurity fSec = new DirectorySecurity();
            fSec.AddAccessRule(new FileSystemAccessRule(userOrUserGroup, rights, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            System.IO.Directory.SetAccessControl(targetDir, fSec);
        }

        #endregion
     #region 设置文件权限
        /// <summary>
        /// 设置文件夹权限 处理给EVERONE赋予所有权限
        /// </summary>
        /// <param name="targetFile">文件路径</param>
        /// <param name="rights">权限，默认修改权限。不推荐 FileSystemRights.FullControl</param>
        public static void SetFileEveryoneRole(string targetFile, FileSystemRights rights = FileSystemRights.Modify)
        {
            SetFileRights(targetFile, "Everyone", rights);
        }
        /// <summary>
        /// 设置文件夹权限 处理给EVERONE赋予所有权限
        /// </summary>
        /// <param name="targetFile">文件路径</param>
        /// <param name="rights">权限，默认修改权限。不推荐FileSystemRights.FullControl</param>
        public static void SetFileIIS_IUSRSRole(string targetFile, FileSystemRights rights= FileSystemRights.Modify)
        {
            SetDirRights(targetFile, "IIS_IUSRS", rights);
        }
        /// <summary>
        /// 设置文件夹权限
        /// </summary>
        /// <param name="targetFile">文件路径</param>
        /// <param name="userOrUserGroup"></param>
        /// <param name="rights"></param>
        public static void SetFileRights(string targetFile,string userOrUserGroup, FileSystemRights rights)
        {
            //if (!File.Exists(targetFile))
            //{
            //    File.Create(targetFile);
            //}
            FileSecurity fSec = new FileSecurity();
            fSec.AddAccessRule(new FileSystemAccessRule(userOrUserGroup, rights, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
            File.SetAccessControl(targetFile, fSec);
        }

        #endregion
    }
}
