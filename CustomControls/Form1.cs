using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControls
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // FlatStyle.System 将会取消圆形（椭圆）效果，恢复默认的按钮 在设计器中修改FlatStyle也一样，虽然预览还是圆，但是执行会变为默认形状
            // 如何在设计器中隐藏 FlatStyle 属性，阻止修改？
            // circleButton1.FlatStyle = FlatStyle.System;
        }
    }
}
