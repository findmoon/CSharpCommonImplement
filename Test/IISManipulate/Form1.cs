using HelperCollections;
using HelperCollections.IIS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var props= iisHelper.EnumerateProperties();

            iisPropertiesListBox.Items.Clear();
            iisPropertiesListBox.Items.AddRange(props.Select(p=>$"{p.Key}：{p.Value}").ToArray());

            var allWebs=iisHelper.GetAllWebSites();
            iisWebNamesListBox.Items.Clear();
            iisWebNamesListBox.Items.AddRange(allWebs.Select(w=>$"{w.Item1} - {w.Item2} - {w.Item3}").ToArray());


            var siteInfoList=iisHelper.GetSiteList();
            siteInfotTree.ShowNodeToolTips = true;

            siteInfotTree.Nodes.Clear();
            foreach (var siteInfo in siteInfoList)
            {
                var node=new TreeNode() { Name = siteInfo.Name, Text = siteInfo.Name, ToolTipText = "Name" };
                InitTreeView(node.Nodes, siteInfo);

                siteInfotTree.Nodes.Add(node);
            }

            var siteInfo_KVs= IISSiteHelper_DirectoryServices.GetFlatSiteList(siteInfoList);
            foreach (var siteInfo_KV in siteInfo_KVs)
            {
                siteInfotTreeTxt.Text += siteInfo_KV.Value + Environment.NewLine;
            }
            
            
            iisVersionTxt.Text=iisHelper.GetIISVersion();


      
        }

        public void InitTreeView(TreeNodeCollection nodes, SiteInfo siteInfo)
        {
            nodes.Add(new TreeNode { Name = siteInfo.Name, Text = siteInfo.Name, ToolTipText = "Name" });
            nodes.Add(new TreeNode { Name = siteInfo.IsApp.ToString(), Text = siteInfo.IsApp.ToString(), ToolTipText = "是否应用程序" });
            nodes.Add(new TreeNode { Name = siteInfo.Path, Text = siteInfo.Path, ToolTipText = "Path" });

            if (siteInfo.ServerBindings!=null)
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

                currNode.Name= currNode.Nodes[0].Name;
                currNode.Text= currNode.Nodes[0].Text;
                currNode.ToolTipText= currNode.Nodes[0].ToolTipText;

                nodes.Add(currNode);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_DirectoryServices();
            iisHelper.RemoveIISWebSite("MySiteTest1");

            MessageBox.Show("删除站点结束");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var iisHelper = new IISSiteHelper_DirectoryServices();
            iisHelper.CreateWebSite("MySiteTest1", "C:\\inetpub\\Test\\MySiteTest1", port: 1931);

            MessageBox.Show("结束创建站点");
        }
    }
}
