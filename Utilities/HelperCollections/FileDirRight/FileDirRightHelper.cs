using HelperCollections.ArgumentGuardHandle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HelperCollections
{
    /// <summary>
    /// 文件文件夹权限设置帮助类
    /// </summary>
    public class FileDirRightHelper
    {
        /// <summary>
        /// 设置文件夹权限 处理给EVERONE赋予所有权限
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <param name="role">角色[用户或用户组]，默认Users。常用的有Everyone、IIS_IUSRS、Administrators</param>
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
        /// <param name="role">角色[用户或用户组]，默认Users。常用的有Everyone、IIS_IUSRS、Administrators</param>
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
}
