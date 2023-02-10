using System;
using System.Diagnostics;
using WixSharp;

namespace WixSharp_Setup
{
    internal class Program
    {
        static void Main()
        {
            var outputModelDir = Debugger.IsAttached ? "Debug" : "Release";
            var project = new Project(@"PublishExample\ClickOnceWPFFx",
                              new Dir(@"%ProgramFiles%\CodeMissing\ClickOnceWPFFx",
                              new DirFiles(@"Release\Bin\*.*")
                              //new DirFiles(outputModelDir + @"\*.*")
                              ));

            //project.GUID = new Guid("6fe30b47-2577-43ad-9095-1861ba25889b");
            project.GUID = new Guid();
            //project.SourceBaseDir = "<input dir path>";
            //project.OutDir = "<output dir path>";

            project.BuildMsi();
        }
    }
}