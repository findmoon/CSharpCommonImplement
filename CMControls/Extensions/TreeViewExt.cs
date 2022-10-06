using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Windows.Forms
{
    public static class TreeViewExt
    {
        /// <summary>
        /// TreeNode的Tag存放当前节点的路径
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dirs"></param>
        /// <param name="containeDir">是否映射文件夹</param>
        /// <param name="onlyCurrDir">是否仅映射当前路径下的内容，未避免产生大量节点添加时卡顿，默认值为true</param>
        public static void MapDirToTreeNode(this TreeNode node, string dirs, bool containeDir = true, bool onlyCurrDir = true)
        {
            if (node == null)
            {
                throw new ArgumentNullException("参数不能为空");
            }
            if (!Directory.Exists(dirs))
            {
                throw new DirectoryNotFoundException($"{dirs}不存在");
            }

            // 显示的文字不存在时指定为文件名
            if (string.IsNullOrWhiteSpace(node.Text))
            {
                node.Text = Path.GetFileName(dirs);
            }
            node.Tag = dirs;

            var currFiles = Directory.GetFiles(dirs);
            if (currFiles.Length > 0)
            {
                node.Nodes.AddRange(currFiles.Select(f => {
                    var childNode = new TreeNode(Path.GetFileName(f));
                    childNode.Tag = Path.GetDirectoryName(f);
                    return childNode;
                }).ToArray());
            }

            if (containeDir)
            {
                //var files = Directory.GetDirectories(dirs,"*", SearchOption.AllDirectories);
                var currDirs = Directory.GetDirectories(dirs);
                foreach (var currdir in currDirs)
                {
                    var childNode = new TreeNode(currdir);
                    if (!onlyCurrDir)
                    {
                        // 递归调用
                        MapDirToTreeNode(childNode, currdir, containeDir, onlyCurrDir);
                    }
                    node.Nodes.Add(childNode);
                }
            }
        }
    }
}
