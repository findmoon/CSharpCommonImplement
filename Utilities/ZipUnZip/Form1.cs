using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZipUnZip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonPro1_Click(object sender, EventArgs e)
        {
            try
            {
                // 压缩
                var testPath = "test";
                var zipFile = "test.zip";

                // 压缩并提示是否覆盖
                var isCompress = true;
                if (File.Exists(zipFile))
                {
                    var msgResult = MessageBox.Show($"{zipFile}已经存在，确定覆盖并继续压缩吗？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (msgResult != DialogResult.OK)//不继续
                    {
                        isCompress = false;
                    }
                }
                if (isCompress)
                {
                    ZipHelper.Compress(testPath, zipFile, isCompress);
                }

                // 压缩文件
                var aTxt = @"test\a.txt";
                ZipHelper.Compress(aTxt, true);

                // 压缩目录到当前
                ZipHelper.Compress(testPath, true);


                var aTxtZip = "aTxt.zip";
                #region 从文件创建
                //// 不能直接从文件创建压缩包，只能从文件夹创建压缩文件  报错：System.IO.IOException:目录名称无效。
                //ZipFile.CreateFromDirectory(aTxt, aTxtZip);
                #endregion


                // 解压缩
                var extraPath = "unZipDir";
                if (File.Exists(zipFile))
                {
                    ZipHelper.Decompress(zipFile, extraPath,true); // 覆盖目标
                }
                else
                {
                    MessageBox.Show("解压缩的文件不存在");
                }
                var aZip = @"test\a.zip";
                if (File.Exists(aZip))
                {
                    ZipHelper.Decompress(aZip);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"处理出错：{ex.Message}，考虑解压目标文件是否存在");
            }
        }

        private void buttonPro2_Click(object sender, EventArgs e)
        {
            var zipFile = "test.zip";
            using (FileStream zipToOpen = new FileStream(zipFile, FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry readmeEntry = archive.CreateEntry("Readme.txt");
                    using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                    {
                        writer.WriteLine("有关该压缩包的信息");
                        writer.WriteLine("========================");
                    }
                }
            }

        }

        private async void buttonPro3_Click(object sender, EventArgs e)
        {
            var text = File.ReadAllText(@"C:\Users\win7hostsver\Downloads\圆角.txt");
            var gzipText=await GZipHelper.CompressToBase64Async(text);

            var unGzipText =await GZipHelper.DeCompressFromBase64Async(gzipText);

            MessageBox.Show(unGzipText);
        }

        private async void buttonPro4_Click(object sender, EventArgs e)
        {
            var file = @"C:\Users\win7hostsver\Downloads\圆角.txt";
            var destFile = @"C:\Users\win7hostsver\Downloads\kkk.txt.gz";
            await GZipHelper.CompressFileAsync(file, destFile);

           await GZipHelper.DeCompressFileAsync(destFile);

            MessageBox.Show("成功");
        }

        private async void buttonPro5_Click(object sender, EventArgs e)
        {
            var text = File.ReadAllText(@"C:\Users\win7hostsver\Downloads\圆角.txt");
            var comBytes =await DeflateHelper.CompressStringAsync(text);

            var unGzipText =await DeflateHelper.DecompressStringAsync(comBytes);

            MessageBox.Show(unGzipText);
        }

        private async void buttonPro6_Click(object sender, EventArgs e)
        {
            var file = @"C:\Users\win7hostsver\Downloads\圆角.txt";
            var destFile = @"C:\Users\win7hostsver\Downloads\deflate压缩文件.df";
            await DeflateHelper.CompressFileAsync(file, destFile);

            await DeflateHelper.DeCompressFileAsync(destFile,Path.Combine(Path.GetDirectoryName(file), "deflate解压缩文件.txt"));

            MessageBox.Show("成功");
        }
    }
}
