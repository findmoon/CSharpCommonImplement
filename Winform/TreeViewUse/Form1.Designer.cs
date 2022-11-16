namespace TreeViewUse
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("菜单1-1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("菜单1", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("菜单2-1");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("菜单2-2");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("菜单2", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("菜单3");
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.FullRowSelect = true;
            this.treeView1.ItemHeight = 36;
            this.treeView1.Location = new System.Drawing.Point(0, 42);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "nodeMenu1_1";
            treeNode1.Text = "菜单1-1";
            treeNode2.Name = "nodeMenu1";
            treeNode2.Text = "菜单1";
            treeNode3.Name = "nodeMune2_1";
            treeNode3.Text = "菜单2-1";
            treeNode4.Name = "nodeMenu2_2";
            treeNode4.Text = "菜单2-2";
            treeNode5.Name = "nodeMenu2";
            treeNode5.Text = "菜单2";
            treeNode6.Name = "nodeMenu3";
            treeNode6.Text = "菜单3";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode5,
            treeNode6});
            this.treeView1.ShowLines = false;
            this.treeView1.ShowPlusMinus = false;
            this.treeView1.Size = new System.Drawing.Size(172, 403);
            this.treeView1.TabIndex = 0;
            // 
            // treeView2
            // 
            this.treeView2.Location = new System.Drawing.Point(249, 42);
            this.treeView2.Name = "treeView2";
            this.treeView2.Size = new System.Drawing.Size(239, 396);
            this.treeView2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "树形菜单/多级菜单";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(247, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "映射加载文件/文件夹";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeView2);
            this.Controls.Add(this.treeView1);
            this.Name = "Form1";
            this.Text = "TreeView控件的使用";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TreeView treeView2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

