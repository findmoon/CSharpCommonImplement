using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CustomControlRound
{
    /// <summary>
    /// 带锯齿的圆角窗体
    /// </summary>
    [Obsolete("创建新的Region实现圆角，会有锯齿")]
    public partial class RoundFormWithAlias : Form
    {        
        public RoundFormWithAlias()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.None;
            Paint += RoundForm_Paint;
        }

        private void RoundForm_Paint(object sender, PaintEventArgs e)
        {
            CurveRound(this, 25, 0.1f);
        }

        /// <summary>
        /// AddClosedCurve 方法的圆角，将Control设置为圆角
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="p_1">曲线到直线的交点</param>
        /// <param name="tension">表示圆角曲线的弯曲程度，0~1，0表示最小弯角(最锐拐角)，1表示最平滑弯曲</param>
        private void CurveRound(Control sender, int p_1, float tension)
        {
            GraphicsPath oPath = new GraphicsPath();
            oPath.AddClosedCurve(new Point[] {
                new Point(0, sender.Height / p_1),
                new Point(sender.Width / p_1, 0),
                new Point(sender.Width - sender.Width / p_1, 0),
                new Point(sender.Width, sender.Height / p_1),
                new Point(sender.Width, sender.Height - sender.Height / p_1),
                new Point(sender.Width - sender.Width / p_1, sender.Height),
                new Point(sender.Width / p_1, sender.Height),
                new Point(0, sender.Height - sender.Height / p_1)
            },
            tension);
            sender.Region = new Region(oPath);
        }


    }
}
