using System;
using System.Windows.Forms;
using WixSharp;
using WixSharp.UI.WPF;

namespace WixSharp_Managed_Setup_CustomWPFUI
{
    internal class Program
    {
        static void Main()
        {
            var project = new ManagedProject("MyProduct",
                              new Dir(@"%ProgramFiles%\My Company\My Product",
                                  new File("Program.cs")));

            project.GUID = new Guid("6fe30b47-2577-43ad-9095-1861ba25889b");

            // project.ManagedUI = ManagedUI.DefaultWpf; // all stock UI dialogs

            //custom set of UI WPF dialogs
            project.ManagedUI = new ManagedUI();

            project.ManagedUI.InstallDialogs.Add<WixSharp_Managed_Setup_CustomWPFUI.WelcomeDialog>()
                                            .Add<WixSharp_Managed_Setup_CustomWPFUI.LicenceDialog>()
                                            .Add<WixSharp_Managed_Setup_CustomWPFUI.FeaturesDialog>()
                                            .Add<WixSharp_Managed_Setup_CustomWPFUI.InstallDirDialog>()
                                            .Add<WixSharp_Managed_Setup_CustomWPFUI.ProgressDialog>()
                                            .Add<WixSharp_Managed_Setup_CustomWPFUI.ExitDialog>();

            project.ManagedUI.ModifyDialogs.Add<WixSharp_Managed_Setup_CustomWPFUI.MaintenanceTypeDialog>()
                                           .Add<WixSharp_Managed_Setup_CustomWPFUI.FeaturesDialog>()
                                           .Add<WixSharp_Managed_Setup_CustomWPFUI.ProgressDialog>()
                                           .Add<WixSharp_Managed_Setup_CustomWPFUI.ExitDialog>();

            //project.SourceBaseDir = "<input dir path>";
            //project.OutDir = "<output dir path>";

            project.BuildMsi();
        }
    }
}