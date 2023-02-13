using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using WixSharp;
using WixSharp.CommonTasks;

namespace WixSharp_Setup
{
    internal class Program
    {
        //static string useReplacePath;

        static void Main()
        {

            var outputModelDir = Debugger.IsAttached ? "Debug" : "Release";

            // Company Name 、Product Name 从程序中获取

            // 要制作安装包的程序，默认固定放在 .\Product\ （Product\）下。不放在..\Product\或..\..\Product\，否则使用相对目录还需要在项目的上级目录和生成的Debug/Release上级目录中都复制一份；重点是不复制到项目的上级目录，而是当前目录
            // 编译当前安装包项目之前，应该将项目编译生成到此目录下。【构建/生成-事件-生成前事件 执行MSBuild命令行，待确认】
            // 或者，使用绝对路径【参见下面，搜索"绝对路径"】

            // 安装包项目生成后，自动执行，复制Product\，并 生成安装包【属性-构建/生成-事件-生成后事件 中填写 `"$(TargetPath)"` 即可】。安装包目录默认为 Setup\ 

            var productPath = "Product\\";
            var company = "CodeMissing";
            var product = "ClickOnceWPFFx";

            var mainExeName = $"{product}.exe";


            //WixSharp.File mainExeFile;
            // @"%ProgramFiles%\My Company\My Product"
            var productDir = new Dir($@"%ProgramFiles%\{company}\{product}");
            //var currFiles=new WixSharp.File[] { mainExeFile=new WixSharp.File($"{productPath}{mainExeName}") };
            #region 这种方式，获取到的子文件下的文件 会 被平铺的 复制到安装目录下。应该保持子目录相对不变。不能这么简单处理
            //productDir.Files = currFiles.Combine(Directory.GetFiles(@"{productPath}", "*.*", SearchOption.AllDirectories)
            //                    .Where(f=> !f.EndsWith(mainExeName)) // 过滤重复    
            //                    .Select(f=>new WixSharp.File(f))); // .ToArray()
            #endregion


            //useReplacePath = new DirectoryInfo(productPath).Parent.FullName;
            var wixFiles_Dirs = GetAllProductRequiredFiles(productPath);


            productDir.Files = wixFiles_Dirs.Item1;
            // 无法赋值子目录。赋值会报错 is not a valid long name because it contains illegal characters.  Legal long names contain no more than 260 characters and must contain at least one non-period character.  Any character except for the follow may be used: \ ? | > < : / * ".
            // 如何处理非法字符？
            productDir.Dirs = wixFiles_Dirs.Item2;

            #region // 此处查找，永远会报错 Enumerable 错误   System.InvalidOperationException: 序列不包含任何元素
            ////也可以不过滤重复，全部加载后，从 productDir.Files 查找 mainExeName 的 mainExeFile
            //mainExeFile = productDir.Files?.Where(f => f.TargetFileName.EndsWith(mainExeName)).SingleOrDefault();
            //if (mainExeFile != null)
            //{
            //    // 快捷方式
            //    mainExeFile.Shortcuts = new[]
            //            {
            //             //new FileShortcut(mainExeName, "INSTALLDIR"), // 安装目录下 快捷方式
            //             new FileShortcut(mainExeName, "%Desktop%"),
            //             new FileShortcut(mainExeName, $@"%ProgramMenu%\{product}") // 开始菜单 快捷方式
            //         };
            //} 
            #endregion

            var project = new Project(@"ClickOnceWPFFx",
                              productDir

                              //new Dir(@"%ProgramFiles%\CodeMissing\ClickOnceWPFFx",
                              ////new File(@"..\ClickOnceWPFFx\bin\" + outputModelDir + @"\ClickOnceWPFFx.exe")
                              //new DirFiles(@"..\ClickOnceWPFFx\bin\" + outputModelDir + @"\*.*")

                              // 缺点是，必须在当前安装项目的目录的同一级下，存在 DirFiles(xxx) 指定的目录；但是该目录应该是在生成的Debug或Release同一级下存在，并在后续被打包进安装包中的。
                              // 构建事件中，生成前安装程序构建到 DirFiles(xxx) 指定的目录；构建后再将其复制到 相对于安装项目生成目录 的文件夹下
                              // 或者，直接使用绝对路径

                              ////new File(@"Release\ClickOnceWPFFx.exe")
                              ////new DirFiles(outputModelDir + @"\*.*")
                              //)
                              );

            // 查找mainExeFile，创建快捷方式
            project.FindFile(f => f.Name.EndsWith(mainExeName))
                   .First()
                   .Shortcuts = new[]
                                {
                                    new FileShortcut(mainExeName, "%Desktop%"),
                                    new FileShortcut(mainExeName, $@"%ProgramMenu%\{product}") // 开始菜单 快捷方式
                                };

            //project.GUID = new Guid("6fe30b47-2577-43ad-9095-1861ba25889b");
            project.GUID = new Guid("EB4F0307-1179-456E-BF83-41A71803800E");
            //project.SourceBaseDir = "<input dir path>";
            //project.OutDir = "<output dir path>";

            // 指定当前项目的bin目录？。以查找bin下面的Product，即对应的 ..\..\Product\
            // 指定源基目录为绝对路径，上面需要绝对路径的地方，还可以继续使用相对路径。！！！？
            //project.SourceBaseDir = Path.GetFullPath(productPath);
            //project.SourceBaseDir = Path.GetFullPath(productPath);

            // 也使用绝对路径 【生成后事件执行，将以安装项目文件所在目录为基础目录】
            project.OutDir = $@"bin\{outputModelDir}\Setup";



            project.BuildMsi();
        }

        private static Tuple<WixSharp.File[], WixSharp.Dir[]> GetAllProductRequiredFiles(string productPath)
        {
            // 获取绝对路径再递归处理，否则会有问题，比如Dirs获取不正确(多出)
            productPath = Path.GetFullPath(productPath);
            //var useReplacePath = new DirectoryInfo(productPath).Parent.FullName;
            var useReplacePath = productPath;

            WixSharp.File[] wixFiles = new WixSharp.File[] { };
            // 循环获取文件、子文件夹
            var files = Directory.GetFiles(productPath, "*.*");
            if (files.Length > 0)
            {
                //wixFiles = files.Select(f => new WixSharp.File(f.Replace(useReplacePath, ""))).ToArray();
                wixFiles = files.Select(f => new WixSharp.File(f)).ToArray();
            }

            var wixDirs = new List<Dir>();
            var dirs = Directory.GetDirectories(productPath);
            if (dirs.Length > 0)
            {
                foreach (var dir in dirs)
                {
                    // Any character except for the follow may be used: \ ? | > < : / * ". 替换去除非法字符
                    //var wixDir = new Dir(dir.Replace(useReplacePath, "").TrimStart('\\'));
                    var wixDir = new Dir(dir);

                    // 无效字符，最终确认是名字违规，名字要对应替换和修改。 <Directory Id="Template" Name="Template"> Name不能有\，要对应实际的文件夹名，用于安装包安装时的生成
                    wixDir.Name = wixDir.Name.Replace(useReplacePath, "").TrimStart('\\');

                    var wixDirSubFiles_Dirs = GetAllProductRequiredFiles(dir);

                    if (wixDirSubFiles_Dirs.Item1.Length > 0)
                    {
                        wixDir.AddFiles(wixDirSubFiles_Dirs.Item1);
                    }
                    if (wixDirSubFiles_Dirs.Item2.Length > 0)
                    {
                        wixDir.AddDirs(wixDirSubFiles_Dirs.Item2);
                    }

                    wixDirs.Add(wixDir);
                }
            }

            return Tuple.Create(wixFiles, wixDirs.ToArray());
        }
    }
}