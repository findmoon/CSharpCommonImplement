using HelperCollections.IIS;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace IISManipulate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_DirectoryServices();

            var props = iisHelper.EnumerateProperties();

            iisPropertiesListBox.Items.Clear();
            iisPropertiesListBox.Items.AddRange(props.Select(p => $"{p.Key}：{p.Value}").ToArray());

            var allWebs = iisHelper.GetAllWebSites();
            iisWebNamesListBox.Items.Clear();
            iisWebNamesListBox.Items.AddRange(allWebs.Select(w => $"{w.Item1} - {w.Item2} - {w.Item3}").ToArray());


            var siteInfoList = iisHelper.GetSiteList();
            siteInfotTree.ShowNodeToolTips = true;

            siteInfotTree.Nodes.Clear();
            foreach (var siteInfo in siteInfoList)
            {
                var node = new TreeNode() { Name = siteInfo.Name_ServerComment, Text = siteInfo.Name_ServerComment, ToolTipText = "Name_ServerComment" };
                InitTreeView(node.Nodes, siteInfo);

                siteInfotTree.Nodes.Add(node);
            }

            var siteInfo_KVs = IISSiteHelper_DirectoryServices.GetFlatSiteList(siteInfoList);
            foreach (var siteInfo_KV in siteInfo_KVs)
            {
                siteInfotTreeTxt.Text += siteInfo_KV.Value + Environment.NewLine;
            }

            iisVersionTxt.Text = iisHelper.IISVersion;
        }

        public void InitTreeView(TreeNodeCollection nodes, SiteInfo siteInfo)
        {
            nodes.Add(new TreeNode { Name = siteInfo.Name_ServerComment, Text = siteInfo.Name_ServerComment, ToolTipText = "Name_ServerComment" });
            nodes.Add(new TreeNode { Name = siteInfo.IsApp.ToString(), Text = siteInfo.IsApp.ToString(), ToolTipText = "是否应用程序" });
            nodes.Add(new TreeNode { Name = siteInfo.Path, Text = siteInfo.Path, ToolTipText = "Path" });

            if (siteInfo.ServerBindings != null)
            {
                foreach (var item in siteInfo.ServerBindings)
                {
                    nodes.Add(new TreeNode { Name = item, Text = item, ToolTipText = "绑定信息" });
                }
            }

            foreach (var item in siteInfo.Children)
            {
                var currNode = new TreeNode();

                InitTreeView(currNode.Nodes, item);

                currNode.Name = currNode.Nodes[0].Name;
                currNode.Text = currNode.Nodes[0].Text;
                currNode.ToolTipText = currNode.Nodes[0].ToolTipText;

                nodes.Add(currNode);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_DirectoryServices();
            iisHelper.RemoveIISWebSiteVirtualDir("MySiteTest1VD3");
            iisHelper.RemoveIISWebSiteVirtualDir("MySiteTest1VD1");
            iisHelper.RemoveIISWebSiteVirtualDir("MySiteTest1VD2");
            iisHelper.RemoveIISWebSiteVirtualDir("RootMySiteTest1VD1");
            iisHelper.RemoveIISWebSiteVirtualDir("RootMySiteTest1VD2");
            iisHelper.RemoveIISWebSiteVirtualDir("MySiteTest2_9");


            MessageBox.Show("删除站点结束");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_DirectoryServices();
            iisHelper.CreateWebSite("MySiteTest1", "C:\\inetpub\\Test\\MySiteTest1", port: 1931);

            MessageBox.Show("结束创建站点");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_DirectoryServices();
            iisHelper.StopWebSite("MySiteTest1");
            iisHelper.StartWebSite("MySiteTest1");
            iisHelper.ResetWebSite("MySiteTest1");

            MessageBox.Show("启用站点结束");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_DirectoryServices();
            iisHelper.CreateUpdateVDirApplication("MySiteTest1", "MySiteTest1VD1", @"C:\inetpub\Test\MySiteTest_VD");
            iisHelper.CreateUpdateVDirApplication("MySiteTest1", "MySiteTest1VD2", @"C:\inetpub\Test\MySiteTest_VD", false);
            iisHelper.CreateUpdateVDirApplication("MySiteTest1VD1", "MySiteTest1VD3", @"C:\inetpub\Test\MySiteTest_VD", false);

            // 创建 指定应用程序池
            iisHelper.CreateWebSite("MySiteTest2", @"C:\inetpub\Test\MySiteTest_VD", port: 1932, appPoolName:"NewAppName1");
            iisHelper.CreateUpdateVDirApplication("MySiteTest2", "MySiteTest2VD1", @"C:\inetpub\Test\MySiteTest_VD", appPoolName: "NewAppName1");
            iisHelper.CreateUpdateVDirApplication("MySiteTest2", "MySiteTest2VD2", @"C:\inetpub\Test\MySiteTest_VD", appPoolName: "NewAppNameNew1");

            MessageBox.Show("创建虚拟站点结束");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_DirectoryServices();
            var appPoolInfos=iisHelper.GetAppPools();

            appPoolsInfoListBox.Items.Clear();
            appPoolsInfoListBox.Items.AddRange(appPoolInfos.Select(p=>$"{p.AppPoolName} - {p.ManagedRuntimeVersion} - {p.ManagedPipelineMode} - {p.AppPoolIdentityType} - {p.AppPoolState} - {p.AppPoolCommand}")
                                                .ToArray());

            MessageBox.Show("结束");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_DirectoryServices();
            iisHelper.CreateUpdateAppPoolWithOutReturn("AppPoolCreate");

            MessageBox.Show("结束");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_MWA();
        
            iisVersionTxt.Text= iisHelper.GetIISVersion().ToString();

            MessageBox.Show($"是否安装：{iisHelper.IISInstalled()}");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_MWA();
            //iisHelper.CreateUpdateWebSite("MySiteTest3", "C:\\inetpub\\Test\\MySiteTest1", port: 1931);

            iisHelper.CreateUpdateWebSite("MySiteTest2", "C:\\inetpub\\Test\\MySiteTest1", port: 1933,sitePreloadEnabled:true,
                appPoolName:"anotherAppPool", startMode: AppPollStartMode.AlwaysRunning);

            MessageBox.Show("结束更新创建站点");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_MWA();
            iisHelper.RemoveApp("MySiteTest2VD1", "MySiteTest2");
            iisHelper.RemoveApp("MySiteTest2VD2", "MySiteTest2");

            iisHelper.RemoveVDir("MySiteTest2VD1", webSiteName:"MySiteTest2");
            iisHelper.RemoveVDir("MySiteTest2VD2", webSiteName:"MySiteTest2");

            MessageBox.Show("结束");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_MWA();
            iisHelper.RemoveIISWebSite("MySiteTest2");
            MessageBox.Show("结束");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_MWA();
            iisHelper.CreateUpdateVDirApplication("MySiteTest2", "appName1", "C:\\inetpub\\Test\\MySiteTest1");
            iisHelper.CreateUpdateVDirApplication("MySiteTest2", "vDir1", "C:\\inetpub\\Test\\MySiteTest1", false);
            iisHelper.CreateUpdateVDirApplication("MySiteTest2", "appName2", "C:\\inetpub\\Test\\MySiteTest1",appPoolName: "appName2AppPool");
            iisHelper.CreateUpdateVDirApplication("MySiteTest2", "appName1_vDir2", "C:\\inetpub\\Test\\MySiteTest1", false, "appName1", appPoolName: "appName2AppPool");

            MessageBox.Show("结束");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_MWA();

            iisHelper.StopWebSite("MySiteTest3");
            iisHelper.StartWebSite("MySiteTest3");

            MessageBox.Show("结束");
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //PSInstallIIS.Test();
            var restartNeeded=PSInstallIIS.Instll();
            if (restartNeeded)
            {
                if (MessageBox.Show("需要重启系统才能完全生效，现在重启吗？","提示！",MessageBoxButtons.YesNo)== DialogResult.Yes)
                {
                    // 重启处理
                }
            }

            MessageBox.Show("结束");
        }
    }
}
