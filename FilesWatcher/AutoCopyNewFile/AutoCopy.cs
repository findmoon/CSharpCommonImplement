using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilesWatcher.AutoCopyNewFile
{
    public partial class AutoCopy : Form
    {
        private FileSystemWatcher watcher;
        public AutoCopy()
        {
            InitializeComponent();
            watcher = new FileSystemWatcher();
            watcher.EnableRaisingEvents = false;

            watcher.Created += File_Created;
        }

        // 自动复制
        private void button1_Click(object sender, EventArgs e)
        {
            var sourcePath = sourcePathTxt.Text.Trim();
            var targetPath = targetPathTxt.Text.Trim();
            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(targetPath) || sourcePath == targetPath ||
                !Directory.Exists(sourcePath) || !Directory.Exists(targetPath))
            {
                MessageBox.Show("源路径和目标路径不能为空，且不能相同，并且路径须均存在！");
                return;
            }

            watcher.Path = sourcePath;
            if (watcher.EnableRaisingEvents)
            {
                watcher.EnableRaisingEvents = false;
                Log.DealLogInfoAsync("自动复制文件已停止");
                button1.Text = "开始自动复制";
                button1.BackColor = Control.DefaultBackColor;
            }
            else
            {
                watcher.EnableRaisingEvents = true;
                Log.DealLogInfoAsync("自动复制文件已开始");
                button1.Text = "关闭自动复制";
                button1.BackColor = Color.LightSeaGreen;
            }
            sourcePathTxt.Enabled = !watcher.EnableRaisingEvents;
            targetPathTxt.Enabled = !watcher.EnableRaisingEvents;
            sourceBtn.Enabled = !watcher.EnableRaisingEvents;
            targetBtn.Enabled = !watcher.EnableRaisingEvents;
            overWriteChk.Enabled = !watcher.EnableRaisingEvents;
        }

        private void File_Created(object sender, FileSystemEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(CopyNewFile, new WatcherCopyModel()
            {
                TargetFile = Path.Combine(targetPathTxt.Text.Trim(), e.Name),
                SourceFile = e.FullPath,
                Name = e.Name,
                OverWriteExist = overWriteChk.Checked
            });
        }

        private void CopyNewFile(object state)
        {
            var copyModel = (WatcherCopyModel)state;
            while (true)
            {
                try
                {
                    System.IO.File.Copy(copyModel.SourceFile, copyModel.TargetFile, copyModel.OverWriteExist);
                    Log.DealLogInfoAsync($"新文件{copyModel.Name}已经复制到目标{copyModel.TargetFile}");
                    break;
                }
                catch (DirectoryNotFoundException ex)
                {
                    Log.DealLogInfoAsync($"新文件{copyModel.Name}复制时，源或目标目录不存在，无法复制，Err：{ex.Message}", true);
                    return;
                }
                catch (FileNotFoundException ex)
                {
                    Log.DealLogInfoAsync($"新文件{copyModel.Name}复制时，源文件不存在，无法复制，Err：{ex.Message}", true);
                    return;
                }
                catch (Exception ex)
                {
                    Log.DealLogInfoAsync($"info：{ex.Message}");
                    Thread.Sleep(300);
                }
            }
        }

        private void sourceBtn_Click(object sender, EventArgs e)
        {
            var folderDialog=new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                sourcePathTxt.Text = folderDialog.SelectedPath;
            }
        }

        private void targetBtn_Click(object sender, EventArgs e)
        {
            var folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                targetPathTxt.Text = folderDialog.SelectedPath;
            }
        }
    }
}
