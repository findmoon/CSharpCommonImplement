using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControlSplitButton
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            splitButton_AboveLeft.SplitMenuStripAlign = ToolStripDropDownDirection.AboveLeft;
            splitButton_AboveRight.SplitMenuStripAlign = ToolStripDropDownDirection.AboveRight;
            splitButton_Left.SplitMenuStripAlign = ToolStripDropDownDirection.Left;
            splitButton_Right.SplitMenuStripAlign = ToolStripDropDownDirection.Right;
            splitButton_BelowLeft.SplitMenuStripAlign = ToolStripDropDownDirection.BelowLeft;

      

            //下拉等宽
            splitButton1.SplitMenuStripWidth = CMControls.SplitButtonDropWidth.EquivalWidth;

            
                  // 无法获得ContextMenu的宽高，无法处理Align
                  var contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(new MenuItem("按钮1"));
            contextMenu.MenuItems.Add(new MenuItem("按钮2"));
            contextMenu.MenuItems.Add(new MenuItem("按钮3"));
            

            splitButton2.SplitMenu = contextMenu;
            //splitButton2.SplitMenuAlign = LeftRightAlignment.Right;

            /// <summary>
            ///// SplitMenuStrip显示后，调整其位置
            ///// </summary>
            ///// <param name="sender"></param>
            ///// <param name="e"></param>
            //void SplitMenuStrip_Opened(object sender, EventArgs e)
            //{
            //    var menuStrip = (ContextMenuStrip)sender;
            //    menuStrip.SetBounds(menuStrip.Left - menuStrip.Width, menuStrip.Top, 0, 0, BoundsSpecified.Location);
            //}

            //contextMenuStrip1.Size
        }
    }
}
