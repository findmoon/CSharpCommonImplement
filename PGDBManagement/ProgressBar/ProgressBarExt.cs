using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PGDBManagement.CMControls
{
    /// <summary>
    /// 基于原生ProgressBar进行的简要扩展，不改变原有样式不进行重绘，实现添加文字、文字位置固定和跟随进度条、进度的颜色。实际使用注意文字背景的一致
    /// 需要特别处理背景颜色变化位置，根据实际调整，搜索：背景颜色的变化位置
    /// 需要特别处理不同进度条长度时的延迟渲染的宽度比，搜索：delayWidth、delayWidth_Outer 修改
    /// 同时，此实现有着天生的缺陷，应为文字Label位置是更具Value计算的，而不是实际的进度位置（暂时无法获取准确进度，最正确的实现是根据进度位置实时调整文字Label位置）
    /// 因此，文字跟随时的效果，只适用于进度步骤比较多，比如Maximum-Minimum为100（左右），Value变化频率没有特别快同时幅度相对不是特别大且变化比较连续时，否则文字Label的跟随位置将非常不协调和不同步。
    /// 【文字跟随的效果，最好不要使用】
    /// 比较好的文字跟随，参见重写OnPaint的实现或自定义实现，或者能够准确获取进度位置的实现
    /// 
    /// 
    /// Progress进度步骤使用 (Maximum - Minimum)/Step 对比每次Step计算而来，因此Step默认设置为1
    /// 
    /// 设计器总是提示不能加载此控件（未能从工具箱加载ProgressBarExt、TextProgressBar控件，请移除...），或者总是在打开设计器时报错未声明ProgressBarExt对象的变量，但是实际存在，并且没有错误(可以运行且正常)，但是设计器打开时就会额外提示，原因不知
    /// 不是是否和命名空间与主程序的不同有关
    /// </summary>
    public class ProgressBarExt : ProgressBar
    {
        #region 字段
        /// <summary>
        /// 表示进度的文字
        /// </summary>
        Label progressLabel;

        // 跟随或固定
        private bool fixedText;
        private string _text;
        private ProgressBarTextPositionModel _textPositionModel;
        private ProgressBarDisplayMode _displayMode;

        private bool percent100Exec = false;

        Timer percent100ExecTimer = new Timer() { Interval = 80 }; // 频率更快、每次更小，尽量平滑
        private EventHandler percent100ExecTimerHandler;
        #endregion

        #region 属性
        /// <summary>
        /// 文本颜色
        /// </summary>
        [Category("高级")]
        public Color TextColor { get => progressLabel.ForeColor; set => progressLabel.ForeColor = value; }

        /// <summary>
        /// 文本模式，默认居中。文字跟随进度块时，仅上、下、内、外有效；固定时，仅内、外无效
        /// </summary>
        [Category("高级")]
        public ProgressBarTextPositionModel TextPositionModel
        {
            get => _textPositionModel; set
            {
                _textPositionModel = value;
                SetLabelTextAndPositionAndBackColor();
            }
        }

        /// <summary>
        /// 进度条文字是否固定，默认true。如果为false，文字跟随进度块，其位置在进度块的范围内调整；如果为true，则文字固定在进度条的某个位置（TextAlign指定）；Style==ProgressBarStyle.Marquee循环模式，跟随无效
        /// </summary>
        [Category("高级"), Description("默认true。如果为false，文字跟随进度块，其位置在进度块的范围内调整，Style==ProgressBarStyle.Marquee循环模式，跟随无效；如果为true，则文字固定在进度条的某个位置（TextAlign指定）")]
        public bool FixedText
        {
            get => fixedText; set
            {
                fixedText = value;
                SetLabelTextAndPositionAndBackColor();
            }
        }

        /// <summary>
        /// 文字提示显示模式，Progress模式的进度使用的是相对于 Maximum/Step 的形式，可以更好的对应Value变化和`PerformStep()`执行，具体参见SetProgressText()方法
        /// </summary>
        [Category("高级"), Description("文字提示显示模式，Progress模式的进度使用的是相对于 Maximum/Step 的形式")]
        public ProgressBarDisplayMode DisplayMode
        {
            get => _displayMode; set
            {
                _displayMode = value;
                SetLabelTextAndPositionAndBackColor();// SetProgressText();
            }
        }

        [Browsable(true)]
        public override string Text
        {
            get => _text; set
            {
                _text = value;

                if (string.IsNullOrWhiteSpace(_text))
                {
                    _text = "";
                }
                SetLabelTextAndPositionAndBackColor();// SetProgressText();
            }
        }


        /// <summary>
        /// ProgressBar没有Value改变的相关事件。在设置时执行相关修改
        /// </summary>
        public new int Value
        {
            get => base.Value; set
            {
                var setPosBack = true;
                if (value >= Maximum)
                {
                    value = Maximum;
                    if (base.Value == Maximum)
                    {
                        setPosBack = false;
                    }
                }
                else if (value <= Minimum)
                {
                    value = Minimum;
                    if (base.Value == Minimum)
                    {
                        setPosBack = false;
                    }
                }

                base.Value = value;
                if (setPosBack)
                {
                    if (value == 0)
                    {
                        progressLabel.Hide(); // 可以注销掉，未测试
                    }
                    SetLabelTextAndPositionAndBackColor();
                    if (value == 0)
                    {
                        progressLabel.Show();
                    }
                }
            }
        }
        /// <summary>
        /// ProgressBar最大值，执行Text、Position相关设置
        /// </summary>
        public new int Maximum
        {
            get => base.Maximum; set
            {
                base.Maximum = value;
                SetLabelTextAndPositionAndBackColor();
            }
        }
        /// <summary>
        /// ProgressBar最小值，执行Text、Position相关设置
        /// </summary>
        public new int Minimum
        {
            get => base.Minimum; set
            {
                base.Minimum = value;
                SetLabelTextAndPositionAndBackColor();
            }
        }
        public new int Step
        {
            get => base.Step; set
            {
                base.Step = value;
                SetProgressText();//为了计算文字显示变化 SetLabelTextAndPositionAndBackColor();
            }
        }


        /// <summary>
        /// 进度条控件内进度(track)的颜色
        /// </summary>
        [Browsable(false)]
        public Color ProgressColor { get => Color.FromArgb(6, 176, 37); }
        #endregion


        public ProgressBarExt()
        {
            progressLabel = new Label() { Text = "", AutoSize = true };
            // progressLabel.BackColor = Color.FromArgb(230, 230, 230); // 文字提示的背景颜色最好和进度条的一致 默认外层背景

            //progressLabel.ForeColor = Color.GhostWhite;

            FixedText = true;
            Text = "";

            StyleChanged += ProgressBarExt_StyleChanged;
            VisibleChanged += ProgressBarExt_VisibleChanged;
            Disposed += ProgressBarExt_Disposed;

            TextPositionModel = ProgressBarTextPositionModel.Center;
            DisplayMode = ProgressBarDisplayMode.Progress;

            Step = 1;
        }

        private void ProgressBarExt_Disposed(object sender, EventArgs e)
        {
            // Label不为其子控件时同样的释放处理
            if (!(progressLabel.Disposing || progressLabel.IsDisposed))
            {
                progressLabel.Dispose();
            }
        }

        private void ProgressBarExt_VisibleChanged(object sender, EventArgs e)
        {
            // Label不为其子控件时的隐藏处理
            progressLabel.Visible = Visible;
        }

        private void ProgressBarExt_StyleChanged(object sender, EventArgs e)
        {
            SetLabelTextAndPositionAndBackColor();
        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetLabelTextAndPositionAndBackColor();
        }

        #region 私有方法
        /// <summary>
        /// 设置TextLabel的背景和颜色，背景尽量和底层的一致
        /// </summary>
        void SetLabelTextAndPositionAndBackColor()
        {
            //this.SuspendLayout();
            SetProgressText();

            if (fixedText || Style == ProgressBarStyle.Marquee) // 循环模式，跟随无效
            {
                switch (TextPositionModel)
                {

                    case ProgressBarTextPositionModel.Above:
                        progressLabel.Parent = this.Parent;
                        progressLabel.Left = Left + (Width - progressLabel.Width) / 2;
                        progressLabel.Top = Top - progressLabel.Height - 2;
                        progressLabel.BackColor = Color.Transparent;
                        break;
                    case ProgressBarTextPositionModel.Below:
                        progressLabel.Parent = this.Parent;
                        progressLabel.Left = Left + (Width - progressLabel.Width) / 2;
                        progressLabel.Top = Bottom + 2;

                        progressLabel.BackColor = Color.Transparent;
                        break;
                    case ProgressBarTextPositionModel.Left:
                        progressLabel.Parent = this;
                        progressLabel.Left = 2;
                        progressLabel.Top = (Height - progressLabel.Height) / 2;


                        progressLabel.BackColor = ProgressColor;
                        break;
                    case ProgressBarTextPositionModel.Right:
                        progressLabel.Parent = this;
                        progressLabel.Left = Width - progressLabel.Width - 2;
                        progressLabel.Top = (Height - progressLabel.Height) / 2;

                        if (((float)(Value - Minimum)) / (Maximum - Minimum) * Width > (progressLabel.Left + progressLabel.Width / 2))
                        {
                            progressLabel.BackColor = ProgressColor;
                        }
                        else
                        {
                            progressLabel.BackColor = Color.FromArgb(230, 230, 230);
                        }
                        break;
                    case ProgressBarTextPositionModel.OutterLeft:
                        progressLabel.Parent = this.Parent;
                        progressLabel.Left = Left - progressLabel.Width - 2;
                        progressLabel.Top = Top + (Height - progressLabel.Height) / 2;

                        progressLabel.BackColor = Color.Transparent;
                        break;
                    case ProgressBarTextPositionModel.OutterRight:
                        progressLabel.Parent = this.Parent;
                        progressLabel.Left = Right + 2;
                        progressLabel.Top = Top + (Height - progressLabel.Height) / 2;

                        progressLabel.BackColor = Color.Transparent;
                        break;
                    case ProgressBarTextPositionModel.AboveLeft:
                        progressLabel.Parent = this.Parent;
                        progressLabel.Left = Left;
                        progressLabel.Top = Top - progressLabel.Height - 2;

                        progressLabel.BackColor = Color.Transparent;
                        break;
                    case ProgressBarTextPositionModel.AboveRight:
                        progressLabel.Parent = this.Parent;
                        progressLabel.Left = Right - progressLabel.Width;
                        progressLabel.Top = Top - progressLabel.Height - 2;

                        progressLabel.BackColor = Color.Transparent;
                        break;
                    case ProgressBarTextPositionModel.BelowLeft:
                        progressLabel.Parent = this.Parent;
                        progressLabel.Left = Left;
                        progressLabel.Top = Bottom + 2;

                        progressLabel.BackColor = Color.Transparent;
                        break;
                    case ProgressBarTextPositionModel.BelowRight:
                        progressLabel.Parent = this.Parent;
                        progressLabel.Left = Right - progressLabel.Width;
                        progressLabel.Top = Bottom + 2;

                        progressLabel.BackColor = Color.Transparent;
                        break;
                    case ProgressBarTextPositionModel.Center:
                    default:
                        progressLabel.Parent = this;
                        progressLabel.Left = (Width - progressLabel.Width) / 2;
                        progressLabel.Top = (Height - progressLabel.Height) / 2;
                        if ((((float)(Value - Minimum)) / (Maximum - Minimum)) > 0.5d)
                        {
                            progressLabel.BackColor = ProgressColor;
                        }
                        else
                        {
                            progressLabel.BackColor = Color.FromArgb(230, 230, 230);
                        }
                        break;
                }
            }
            else
            {

                switch (TextPositionModel)
                {
                    case ProgressBarTextPositionModel.Above:
                        progressLabel.Parent = this.Parent;

                        var textLeft1 = Left + (int)(((float)(Value - Minimum)) / (Maximum - Minimum) * Width) - progressLabel.Width - 2;
                        progressLabel.Left = textLeft1;
                        progressLabel.Top = Top - progressLabel.Height - 2;

                        progressLabel.BackColor = Color.Transparent;
                        break;
                    case ProgressBarTextPositionModel.Below:
                        progressLabel.Parent = this.Parent;

                        var textLeft2 = Left + (int)(((float)(Value - Minimum)) / (Maximum - Minimum) * Width) - progressLabel.Width - 2;
                        progressLabel.Left = textLeft2;
                        progressLabel.Top = Bottom + 2;

                        progressLabel.BackColor = Color.Transparent;
                        break;
                    case ProgressBarTextPositionModel.ProgressOutter:
                        progressLabel.Parent = this;

                        // 渲染滞后的大小，暂时定为0.04
                        var delayWidth_Outer = (int)(Width * 0.04f);

                        //var textLeft_Outter = (int)((Value - 1) / 100f * Width)- delayWidth_Outer;
                        var percentWidth_Outter = (int)(((float)(Value - Minimum)) / (Maximum - Minimum) * Width); // 使用delay，不用-1
                        var textLeft_Outter = percentWidth_Outter - delayWidth_Outer;
                        progressLabel.Left = textLeft_Outter > (Width - progressLabel.Width) ? Width - progressLabel.Width : textLeft_Outter;
                        progressLabel.Top = (Height - progressLabel.Height) / 2;

                        //if (textLeft_Outter > (Width - progressLabel.Width / 2))
                        if (percentWidth_Outter > (Width - progressLabel.Width / 3)) // 背景颜色的变化位置，可根据需要调整
                        {
                            progressLabel.BackColor = ProgressColor;
                        }
                        else
                        {
                            progressLabel.BackColor = Color.FromArgb(230, 230, 230);
                        }
                        break;
                    case ProgressBarTextPositionModel.ProgressInner:
                    default:
                        progressLabel.Parent = this;

                        // 移动的位置在显示中总是不正确，差些距离，但是debug似乎位置是合适的，感觉时value的改变未及时应用到进度条，而实际已经改了label位置，两者不协调导致。
                        // 延迟执行测试也未解决。无法完美显示位置，尤其是最后一小段
                        // 进度条中进度渲染的滞后，导致的这个问题

                        // 渲染滞后的大小，暂时定为0.05
                        var delayWidth = (int)(Width * 0.07f);

                        //var percentWidth = (int)((Value - 1) / 100f * Width);
                        var percentWidth = (int)(((float)(Value - Minimum)) / (Maximum - Minimum) * Width); // 使用delay，不用-1
                        //var textLeft_Inner =  percentWidth - progressLabel.Width - progressLabel.Width;                        

                        var textLeft_Inner = percentWidth - delayWidth;
                        #region 处理尾部最后一段延迟不协调
                        //if (textLeft_Inner < (Width - progressLabel.Width) && textLeft_Inner> progressLabel.Width)
                        //{
                        //    textLeft_Inner -=   progressLabel.Width;
                        //}
                        //if (percentWidth <= (Width - progressLabel.Width) )
                        //if (percentWidth <= (Width - (int)(progressLabel.Width * Value / 100f)) )
                        //if (percentWidth <= (Width - delayWidth))
                        //{
                        //    textLeft_Inner -= progressLabel.Width;
                        //}
                        //else
                        //{
                        //    //textLeft_Inner -= (int)(progressLabel.Width * (1 - Value / 100f));

                        //    textLeft_Inner = Value==100? (percentWidth - progressLabel.Width ): (textLeft_Inner-(int)(progressLabel.Width * Value / 100f));
                        //}
                        #endregion

                        textLeft_Inner -= progressLabel.Width;

                        // 如果有则停止，防止新的位置和 ==100时的位置两者错乱闪烁
                        if (percent100Exec) // 仍有写小问题，比如此处的停止，但不是大问题
                        {
                            percent100Exec = false;
                            percent100ExecTimer.Stop();
                            if (percent100ExecTimerHandler != null)
                            {
                                percent100ExecTimer.Tick -= percent100ExecTimerHandler;
                                percent100ExecTimerHandler = null;
                            }

                        }
                        if (Value == Maximum)// 需要保证只执行一次，停止之前的执行
                        {
                            // 如果已经位于最后delayWidth最后一部分的处理，则返回，等待处理结束
                            if (progressLabel.Left > (Width - progressLabel.Width - delayWidth + 2)) // 如果同时进行 FixedText TextPositionModel Value 等属性的赋值，导致SetLabelTextPositionAndBackColor()多次执行，最后移动位置的DelayWidth段仍然可以触发闪烁(textLabel位置变化的问题)，需要结合下面percent100Exec覆盖执行共同解决
                            {
                                if (progressLabel.Left > (Width - progressLabel.Width - 2))
                                {
                                    progressLabel.Left = Width - progressLabel.Width - 2;
                                }
                                return;
                            }
                            percent100Exec = true;
                            //textLeft_Inner = percentWidth - progressLabel.Width;
                            // 到达100后再延迟执行
                            //var timer = new Timer() { Interval = 80 }; // 频率更快、每次更小，尽量平滑

                            // 最后一段的增量
                            var inc = (int)(delayWidth * 0.12f);
                            percent100ExecTimerHandler = (a, b) =>
                            {
                                #region textLeft_Inner使用会有值错乱(在有了一次100%之后的循环会错乱，导致最后一段Label位置不走到第)，闭包
                                //textLeft_Inner += inc > 0 ? inc : 1;
                                //if (textLeft_Inner > (Width - progressLabel.Width - 2))
                                //{
                                //    textLeft_Inner = Width - progressLabel.Width - 2;
                                //}
                                //progressLabel.Left = textLeft_Inner < 0 ? 0 : textLeft_Inner;
                                #endregion
                                #region 直接使用progressLabel.Left
                                progressLabel.Left += inc > 0 ? inc : 1;
                                if (progressLabel.Left > (Width - progressLabel.Width - 2))
                                {
                                    progressLabel.Left = Width - progressLabel.Width - 2;
                                }
                                #endregion

                                progressLabel.Top = (Height - progressLabel.Height) / 2;

                                if (percentWidth < progressLabel.Width / 2) // 长度和位置总是那么怪
                                {
                                    progressLabel.BackColor = Color.FromArgb(230, 230, 230);
                                }
                                else
                                {
                                    progressLabel.BackColor = ProgressColor;
                                }
                                if (progressLabel.Left >= (Width - progressLabel.Width - 2))
                                {
                                    percent100Exec = false;
                                    percent100ExecTimer.Stop();
                                    if (percent100ExecTimerHandler != null)
                                    {
                                        percent100ExecTimer.Tick -= percent100ExecTimerHandler;
                                        percent100ExecTimerHandler = null;
                                    }
                                    //percent100ExecTimer.Dispose();
                                }
                            };
                            //percent100ExecTimer.Tag = new
                            //{
                            //    Inc = (int)(delayWidth * 0.1f),

                            //};
                            percent100ExecTimer.Tick += percent100ExecTimerHandler;
                            percent100ExecTimer.Start();
                        }

                        #region 同上，尾部最后一段的处理和其它
                        ////else if (percentWidth > (Width - progressLabel.Width))
                        //else if (percentWidth > (Width - (int)(progressLabel.Width * Value / 100f)))
                        //{
                        //    textLeft_Inner -= (int)(progressLabel.Width * (1 - Value / 100f)); // 减去的太少
                        //    //textLeft_Inner -= (int)(progressLabel.Width * (Value / 100f));
                        //    //textLeft_Inner -= (int)(progressLabel.Width * (Value / 100f)); // 中间过程都有减去的太少的处理
                        //}
                        // 如果有多个线程，同时执行Value等增，则会导致progressLabel.Left的位置就会递增增加，为什么？尤其是一个循环(到达100)之后下一个循环增加线程递增，Left位置的递增就非常明显，应该对应Value比例计算，不应该递增才对。原因为何？ 
                        #endregion

                        progressLabel.Left = textLeft_Inner < 0 ? 0 : textLeft_Inner;
                        progressLabel.Top = (Height - progressLabel.Height) / 2;

                        if (percentWidth <= progressLabel.Width / 2) // 背景颜色的变化位置，可根据需要调整 // 长度和位置总是那么怪
                        {
                            progressLabel.BackColor = Color.FromArgb(230, 230, 230);
                        }
                        else
                        {
                            progressLabel.BackColor = ProgressColor;
                        }
                        break;
                }
            }

            if (DesignMode)
            {
                this.Hide();
                this.Show();
            }
            //this.ResumeLayout();
        }


        /// <summary>
        /// 设置具体显示的文本，Progress进度步骤使用 (Maximum - Minimum)/Step 对比每次Step计算而来
        /// </summary>
        private void SetProgressText()
        {
            if (Style == ProgressBarStyle.Marquee)
            {
                progressLabel.Text = _text;
            }
            else
            {
                switch (DisplayMode)
                {
                    case ProgressBarDisplayMode.NoText:
                        progressLabel.Text = "";
                        break;
                    case ProgressBarDisplayMode.Percentage:
                        progressLabel.Text = $"{((float)(Value - Minimum)) / (Maximum - Minimum) * 100}%";
                        break;
                    case ProgressBarDisplayMode.Progress:
                        //progressLabel.Text = $"{Value - Minimum}/{Maximum - Minimum}";
                        progressLabel.Text = $"{(Value - Minimum) / Step}/{(Maximum - Minimum) / Step}";
                        break;
                    case ProgressBarDisplayMode.Text:
                        progressLabel.Text = _text;
                        break;
                    case ProgressBarDisplayMode.TextAndPercentage:
                        progressLabel.Text = $"{_text}: {((float)(Value - Minimum)) / (Maximum - Minimum) * 100}%";
                        break;
                    case ProgressBarDisplayMode.TextAndProgress:
                        //progressLabel.Text = $"{_text}: {Value - Minimum}/{Maximum - Minimum}";
                        progressLabel.Text = $"{_text}: {(Value - Minimum) / Step}/{(Maximum - Minimum) / Step}";
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region 公有方法，延迟设置ProgressBar.Visible=false
        /// <summary>
        /// 延迟设置ProgressBar.Visible=false
        /// </summary>
        /// <param name="timeout">延迟时间，单位毫秒，默认1200</param>
        public void SetVisibleFalse(int timeout = 1200)
        {
            var timer = new Timer() { Interval = timeout };
            timer.Tick += (a, b) =>
            {
                Visible = false;
                timer.Stop();
                timer.Dispose();
            };
            timer.Start();
        }

        public new void PerformStep()
        {
            Value += Step;
        }
        #endregion
    }
}
