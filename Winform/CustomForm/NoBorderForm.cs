using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomForm
{
    /// <summary>
    /// 借助Win32 API ReleaseCapture 和 SendMessage 实现拖动窗体
    /// </summary>
    public partial class NoBorderForm : Form
    {
        public NoBorderForm()
        {
            InitializeComponent();

            //FormBorderStyle = FormBorderStyle.None;

            var noborderEventDrag = new NoBorderFormMove();
            noborderEventDrag.Show();
            var noborderDragResize = new NoBorderFormDragResize();
            noborderDragResize.Show();

            #region 无效
            //var shadowFormNo = new ShadowFormNo(this);

            //////如果需要设置属性，可以用以下方法，不设置也行，类中已有默认值
            ////shadowFormNo.ShadowOpacity = 100;
            ////shadowFormNo.ShadowBlur = 8;
            ////shadowFormNo.ShadowSpread = 6;
            ////shadowFormNo.ShadowH = 8;
            ////shadowFormNo.ShadowV = 8;
            ////shadowFormNo.CornerRound = 4;
            ////shadowFormNo.ShadowColor = Color.Black;

            //////shadowFormNo.Show();
            #endregion

            var shadowForm = new ShadowForm();
            shadowForm.Show();
            
            var customTitleBar = new CustomTitleBar();
            customTitleBar.Show();
            var customTitleBar2 = new CRoundPanelRoundModelForTitle();
            customTitleBar2.Show();

            var customRoundTitleBar =new CustomRoundTitleBar();
            customRoundTitleBar.Show();

            var normalForm = new NormalForm();
            normalForm.Show();

            var noborder2 = new NoBorderForm2();
            noborder2.Show();

            //var t = new Test();
            //t.Show();
        }

    }
}
