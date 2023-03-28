using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace CommonEffect_Elements.TreeViewEffect
{
    /// <summary>
    /// 文件/文件夹信息记录类
    /// 通过指定路径递归加载子路径和子文件，如果遇到目录较深可能会耗时和卡顿
    /// </summary>
    public class DirFileRecord
    {
        /// <summary>
        /// 目录深度，默认从原始初始化时指定的路径开始，目录深度为3
        /// 如需加载更深的目录，则可以增加 DirDepth 的大小，并调用 ChildDirFiles 方法？【暂未实现】
        /// </summary>
        public static int DirDepth { get; set; } = 3;

        /// <summary>
        /// 还需要维护当前的目录深度 【暂时未实现】
        /// </summary>
        static int CurrDirDepth { get; set; }

        /// <summary>
        /// 文件或文件夹路径
        /// </summary>
        public string dirOrFilePath { get; }
        /// <summary>
        /// 文件或文件夹名称
        /// </summary>
        public string Name { get;  }

        /// <summary>
        /// 是否为 文件 【不存在为 null】
        /// </summary>
        public bool? isFile { get; set; }
        /// <summary>
        /// 文件或文件夹的图标路径
        /// </summary>
        public string IconPath { get; set; }

        public DirFileRecord(string path)
        {
            dirOrFilePath = path;
            Name= System.IO.Path.GetFileName(path);
            if (Directory.Exists(path))
            {
                isFile = false;
            }
            else if (File.Exists(path))
            {
                isFile=true;
            }
        }


        public IEnumerable<DirFileRecord> ChildDirFiles
        {
            get
            {
                if (isFile==false)
                {
                    var Info = new DirectoryInfo(dirOrFilePath);
                    // Info.GetDirectories("*", SearchOption.TopDirectoryOnly)
                    var dirs = from di in Info.GetDirectories()
                           select new DirFileRecord(di.FullName);
                    return dirs.Concat(from di in Info.GetFiles()
                                       select new DirFileRecord(di.FullName));
                }

                //return new DirFileRecord[0];
                return null;
            }
        }
    }
}
