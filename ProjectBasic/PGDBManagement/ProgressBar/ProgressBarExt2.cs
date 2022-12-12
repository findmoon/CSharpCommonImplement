using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PGDBManagement
{
    /// <summary>
    /// ProgressBarExt2，通过继承Panel实现，基本思路是通过一个Track作用的Label作为内部进度条，且可以显示文字，另一个Label作为进度条外的文字显示。通过定时器实现循环进度；通过value计算进度条的长度比例，实现进度位置
    /// 其他思路子啊给出的属性中说明中。暂未实际实现
    /// </summary>
    public class ProgressBarExt2 : Panel
    {
        bool fixedText; // 文本是否固定

        /// <summary>
        /// 表示进度的进度块，背景、长度、文字显示和变化
        /// </summary>
        Label progressTrackLabel;
        /// <summary>
        /// 固定文字时显示文字的Label，默认不可见，TextFixed==true时显示且progressTrackLabel.Text为空
        /// </summary>
        Label progressFixedTextLabel;


        /// <summary>
        /// 进度条显示的文字
        /// </summary>
        public string Text
        {
            get => fixedText ? progressFixedTextLabel.Text : progressTrackLabel.Text; set
            {
                progressFixedTextLabel.Text = value;
                if (fixedText)
                {
                    progressTrackLabel.Text = "";
                }
                else
                {
                    progressTrackLabel.Text = value;
                }
            }
        }

        /// <summary>
        /// 文本位置，默认居中
        /// </summary>
        public ContentAlignment TextAlign
        {
            get => progressTrackLabel.TextAlign; set
            {
                progressTrackLabel.TextAlign = value;
                switch (value)
                {
                    // 设置不同的progressFixedTextLabel在整个进度条上的位置
                    case ContentAlignment.TopLeft:
                        break;
                    case ContentAlignment.TopCenter:
                        break;
                    case ContentAlignment.TopRight:
                        break;
                    case ContentAlignment.MiddleLeft:
                        break;
                    case ContentAlignment.MiddleCenter:
                        break;
                    case ContentAlignment.MiddleRight:
                        break;
                    case ContentAlignment.BottomLeft:
                        break;
                    case ContentAlignment.BottomCenter:
                        break;
                    case ContentAlignment.BottomRight:
                        break;
                    default:
                        break;
                }

            }
        }

        /// <summary>
        /// 进度条文字是否固定，默认false，文字跟随进度块，其位置TextAlign在进度块的范围内调整；如果为true，则文字固定在进度条的某个位置（TextAlign指定）
        /// </summary>
        public bool FixedText
        {
            get => fixedText; set
            {
                fixedText = value;
                if (value)
                {
                    progressTrackLabel.Text = "";
                    progressFixedTextLabel.Visiable = true;
                }
                else
                {
                    progressTrackLabel.Text = progressFixedTextLabel.Text;
                    progressFixedTextLabel.Visiable = false;
                }

            }
        }

        public ProgressBarExt2()
        {
            Height = 25;

            progressTrackLabel = new Label();
            progressTrackLabel.BackColor = Color.FromArgb(6, 176, 37); // 文字提示的背景颜色最好和进度条的一致
            progressTrackLabel.Parent = this;
            progressTrackLabel.Text = "";
            progressTrackLabel.ForeColor = Color.GhostWhite;
            progressTrackLabel.AutoSize = false;

            progressTrackLabel.Left = (progressBar.Width - progressLabel.Width) / 2;
            progressTrackLabel.Top = (progressBar.Height - progressLabel.Height) / 2;

            progressFixedTextLabel.Text = new Label();
            progressFixedTextLabel.BackColor = Color.FromArgb(6, 176, 37); // 文字提示的背景颜色最好和进度条的一致
            progressFixedTextLabel.Parent = this;
            progressFixedTextLabel.Text = "";
            progressFixedTextLabel.ForeColor = Color.GhostWhite;
            progressFixedTextLabel.AutoSize = true;

            progressTrackLabel.Left = (progressBar.Width - progressLabel.Width) / 2;
            progressTrackLabel.Top = (progressBar.Height - progressLabel.Height) / 2;



            TextAlign = ContentAlignment.MiddleCenter;
        }
    }
}
