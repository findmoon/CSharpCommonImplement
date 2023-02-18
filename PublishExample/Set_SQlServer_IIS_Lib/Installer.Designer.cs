namespace Set_SQlServer_IIS_Lib
{
    partial class Installer
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

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.iisSiteNameTxt = new System.Windows.Forms.TextBox();
            this.appPoolCombox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "网站名称";
            // 
            // iisSiteNameTxt
            // 
            this.iisSiteNameTxt.Location = new System.Drawing.Point(0, 0);
            this.iisSiteNameTxt.Name = "iisSiteNameTxt";
            this.iisSiteNameTxt.Size = new System.Drawing.Size(100, 21);
            this.iisSiteNameTxt.TabIndex = 0;
            // 
            // appPoolCombox
            // 
            this.appPoolCombox.FormattingEnabled = true;
            this.appPoolCombox.Location = new System.Drawing.Point(0, 0);
            this.appPoolCombox.Name = "appPoolCombox";
            this.appPoolCombox.Size = new System.Drawing.Size(121, 20);
            this.appPoolCombox.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "应用程序池";

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox iisSiteNameTxt;
        private System.Windows.Forms.ComboBox appPoolCombox;
        private System.Windows.Forms.Label label2;
    }
}