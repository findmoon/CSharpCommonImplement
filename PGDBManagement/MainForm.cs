using CMCode.Handle;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

namespace PGDBManagement
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            #region 拖动标题栏、窗体，移动窗体
            TitlePanelTitle.MouseMove += WindowMove_MouseMove;
            TitlePanelTitle.MouseDown += WindowMove_MouseDown;
            TitleIconPicb.MouseMove += WindowMove_MouseMove;
            TitleIconPicb.MouseDown += WindowMove_MouseDown;

            MouseMove += WindowMove_MouseMove;
            MouseDown += WindowMove_MouseDown;
            #endregion

            #region 自定义标题栏 标题、icon、最大、最小、还原、关闭按钮和图标
            //TitlePanelTitle.ForeColor = Color.WhiteSmoke; // 标题文字颜色

            MinimizePicb.MouseEnter += MinimizePicb_MouseEnter;
            MinimizePicb.MouseLeave += MinimizePicb_MouseLeave;
            MaximizeNormalPicb.MouseEnter += MaximizeNormalPicb_MouseEnter;
            MaximizeNormalPicb.MouseLeave += MaximizeNormalPicb_MouseLeave;
            ClosePicb.MouseEnter += ClosePicb_MouseEnter;
            ClosePicb.MouseLeave += ClosePicb_MouseLeave;

            MinimizePicb.Click += MinimizePicb_Click;
            MaximizeNormalPicb.Click += MaximizeNormalPicb_Click;
            ClosePicb.Click += ClosePicb_Click;


            // 处理无Icon时Title标题的位置和最大化范围
            Load += CustomTitleBar_Load;
            #endregion

            #region 标题栏双击窗体最大化或正常
            TitlePanelTitle.DoubleClick += MaximizeNormalPicb_Click;
            #endregion

            progressBar.VisibleChanged += ProgressBar_VisibleChanged;

            //progressBar.Style = ProgressBarStyle.Marquee;
            //progressBar.Style = ProgressBarStyle.Continuous;
            //progressBar.Value = 100;
            //progressBar.Style = ProgressBarStyle.Blocks; // Continuous 似乎无区别

            //progressBar.Style = ProgressBarStyle.Marquee;


            #region 测试progressbar效果
            //var progressLabel = new Label();

            ////progressLabel.BackColor = Color.FromArgb(200,6, 176, 37); // 文字提示的背景颜色最好和进度条的一致
            //progressLabel.BackColor = SystemColors.ControlLight;
            //progressLabel.Parent = progressBar;
            //progressLabel.AutoSize = true;
            //progressLabel.Text = "处理中";
            //progressLabel.ForeColor = Color.GhostWhite;

            ////progressLabel.AutoSize = false;
            //progressLabel.Height = progressBar.Height - 4;
            //progressLabel.TextAlign = ContentAlignment.MiddleCenter;
            //progressLabel.Left = (progressBar.Width - progressLabel.Width) / 2;
            ////progressLabel.Top = (progressBar.Height-progressLabel.Height) / 2;
            //progressLabel.Top = -progressLabel.Height;


            //progressLabel.Parent = progressBar.Parent;
            //progressLabel.BackColor = Color.Transparent;
            //progressLabel.Left = progressBar.Left + (progressBar.Width - progressLabel.Width) / 2;
            //progressLabel.Top = progressBar.Top - progressLabel.Height - 2;
            #endregion

            #region 测试
            //progressBar.Visible = true; // 显示测试

            timer.Tick += (a, b) =>
            {
                progressBar.Value += 1;
                if (progressBar.Value >= 100)
                {
                    timer.Stop();
                }
            };
            #endregion


        }

        private void ProgressBar_VisibleChanged(object sender, EventArgs e)
        {
            // 禁用或启用
            installPSQLBtn.Enabled = !progressBar.Visible;
            initPSQLDBBtn.Enabled = !progressBar.Visible;
            ClosePicb.Enabled = !progressBar.Visible;
        }

        #region 方法1：简化方法 窗体移动,直接变化Left、Top
        private Point originMouseLocation_Simplify;
        private void WindowMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                #region 通过Left、Top计算直接+=变化即可
                Left += e.Location.X - originMouseLocation_Simplify.X;
                Top += e.Location.Y - originMouseLocation_Simplify.Y;
                #endregion
            }
        }
        private void WindowMove_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                originMouseLocation_Simplify = e.Location;
            }
        }
        #endregion

        #region 自定义标题栏操作 标题、icon、最大、最小、还原、关闭按钮和图标 
        private void ClosePicb_Click(object sender, EventArgs e)
        {
            Close();//Application.Exit();
        }

        private void MaximizeNormalPicb_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                //MaximizeNormalPicb.Image = Properties.Resources.Normal_16_16_Gray;
                MaximizeNormalPicb.Image = Properties.Resources.Normal_16_16_White;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                //MaximizeNormalPicb.Image = Properties.Resources.Maximize_16_16_Gray;
                MaximizeNormalPicb.Image = Properties.Resources.Maximize_16_16_White;
            }
        }

        private void MinimizePicb_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void ClosePicb_MouseLeave(object sender, EventArgs e)
        {
            //ClosePicb.Image = Properties.Resources.Close_16_16_Gray;
            ClosePicb.Image = Properties.Resources.Close_16_16_White;
            ClosePicb.BackColor = Color.Transparent;
        }

        private void ClosePicb_MouseEnter(object sender, EventArgs e)
        {
            ClosePicb.Image = Properties.Resources.Close_16_16_White;
            ClosePicb.BackColor = Color.Crimson;
        }

        private void MaximizeNormalPicb_MouseLeave(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                //MaximizeNormalPicb.Image = Properties.Resources.Maximize_16_16_Gray;
                MaximizeNormalPicb.Image = Properties.Resources.Maximize_16_16_White;
            }
            else
            {
                //MaximizeNormalPicb.Image = Properties.Resources.Normal_16_16_Gray;
                MaximizeNormalPicb.Image = Properties.Resources.Normal_16_16_White;
            }
            MaximizeNormalPicb.BackColor = Color.Transparent;
        }

        private void MaximizeNormalPicb_MouseEnter(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                MaximizeNormalPicb.Image = Properties.Resources.Maximize_16_16_Black;
            }
            else
            {
                MaximizeNormalPicb.Image = Properties.Resources.Normal_16_16_Black;
            }
            MaximizeNormalPicb.BackColor = SystemColors.GradientActiveCaption;
        }

        private void MinimizePicb_MouseLeave(object sender, EventArgs e)
        {
            //MinimizePicb.Image = Properties.Resources.Minimize_16_16_Gray;
            MinimizePicb.Image = Properties.Resources.Minimize_16_16_White;
            MinimizePicb.BackColor = Color.Transparent;
        }

        private void MinimizePicb_MouseEnter(object sender, EventArgs e)
        {
            MinimizePicb.Image = Properties.Resources.Minimize_16_16_Black;
            MinimizePicb.BackColor = SystemColors.GradientActiveCaption;
        }
        #region 加载时处理标题栏icon、最大化显示任务栏
        private void CustomTitleBar_Load(object sender, EventArgs e)
        {
            if (TitleIconPicb.Image == null)
            {
                TitlePanelTitle.Left -= TitleIconPicb.Width - 3;
            }
            MaximizedBounds = Screen.GetWorkingArea(this); // 设置最大化时显示为窗体所在工作区(不包含任务栏)  Screen.PrimaryScreen.WorkingArea
        }
        #endregion

        #endregion

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer() { Interval = 100 };
        private void button1_Click(object sender, EventArgs e)
        {
            ////progressBar.TextPositionModel = (CMControls.ProgressBarTextPositionModel)(new Random()).Next(0, 13);

            //progressBar.TextPositionModel = CMControls.ProgressBarTextPositionModel.Center;
            //progressBar.TextPositionModel = CMControls.ProgressBarTextPositionModel.Above;
            //progressBar.TextPositionModel = CMControls.ProgressBarTextPositionModel.AboveLeft;
            //progressBar.TextPositionModel = CMControls.ProgressBarTextPositionModel.AboveRight;
            //progressBar.TextPositionModel = CMControls.ProgressBarTextPositionModel.Below;
            //progressBar.TextPositionModel = CMControls.ProgressBarTextPositionModel.BelowLeft;
            //progressBar.TextPositionModel = CMControls.ProgressBarTextPositionModel.BelowRight;
            //progressBar.TextPositionModel = CMControls.ProgressBarTextPositionModel.Left;
            //progressBar.TextPositionModel = CMControls.ProgressBarTextPositionModel.Right;
            //progressBar.TextPositionModel = CMControls.ProgressBarTextPositionModel.OutterLeft;
            //progressBar.TextPositionModel = CMControls.ProgressBarTextPositionModel.OutterRight;
            //progressBar.TextPositionModel = CMControls.ProgressBarTextPositionModel.ProgressInner;
            //progressBar.TextPositionModel = CMControls.ProgressBarTextPositionModel.ProgressOutter;

            progressBar.Value = 0;



            timer.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //progressBar.DisplayMode = CMControls.ProgressBarDisplayMode.Progress;
            //progressBar.DisplayMode = CMControls.ProgressBarDisplayMode.NoText;
            //progressBar.DisplayMode = CMControls.ProgressBarDisplayMode.Percentage;
            //progressBar.DisplayMode = CMControls.ProgressBarDisplayMode.Text;
            //progressBar.DisplayMode = CMControls.ProgressBarDisplayMode.TextAndPercentage;
            progressBar.DisplayMode = CMControls.ProgressBarDisplayMode.TextAndProgress;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //progressBar.SetState(1);//normal (green)
            //progressBar.SetState(2); //

            //progressBar.SetState(Extensions.Color.Red);
            progressBar.SetState(ProgressBarExten.Color.None);

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (progressBar.Value == progressBar.Maximum)
            {
                progressBar.Value = 0;
                progressBar.Step = 2;
                progressBar.Maximum = 10;
                progressBar.Visible = true;
            }
            else
            {
                if (progressBar.Value + progressBar.Step == progressBar.Maximum)
                {
                    progressBar.Value += 1;
                }
                else
                {
                    progressBar.PerformStep();
                }
            }
        }

        private async void installPSQLBtn_ClickAsync(object sender, EventArgs e)
        {
            // 一键安装数据库
            var psqlInstallPath = @"C:\PostgreSQL";
            if (Directory.Exists(Path.Combine(psqlInstallPath, "pgsql")))
            {
                MessageBox.Show($"PostgreSQL的安装目录存在{Path.Combine(psqlInstallPath, "pgsql")}，\r\n请确保pgsql是否已经安装，若已经卸载请确保卸载干净并删除相关路径！");
                //return;
            }
            try
            {
                if (true)
                {

                }
                var userName = "myuser";
                var userPwd = "mypwd";// 用户 密码

                var dbName = "mydb";

                var psqlServiceName = "PostgreSQL-x64-14";

                // 1. 解压PostgreSQL，创建data

                progressBar.Value = 0;
                progressBar.Step = 2;
                progressBar.Maximum = 10;
                progressBar.Visible = true;

                // 首先执行，处于第一步等待状态
                progressBar.PerformStep();

                #region 测试跳过
                ////await ZipHelper.DecompressAsync(@"pgsql\postgresql-14.5-1-windows-x64-binaries.zip", psqlInstallPath);
                //await ZipHelper.DecompressAsync2(@"pgsql\postgresql-14.5-1-windows-x64-binaries.zip", psqlInstallPath, true);

                var psqlDataPath = Path.Combine(psqlInstallPath, "pgsql", "data");
                //Directory.CreateDirectory(psqlDataPath); 
                #endregion

                progressBar.PerformStep();

                // 2. pgsql/bin下初始化
                var psqlBinPath = Path.Combine(psqlInstallPath, "pgsql", "bin");

                #region 测试跳过
                //// 执行initdb
                //var result = await ExecApp.RunAsync(Path.Combine(psqlBinPath, "initdb.exe"), $"-E=UTF8 -D {psqlDataPath}");
                //if (result.ExceptError != null)
                //{
                //    throw new Exception($"初始化数据库时发生错误!{result.ExceptError.Message}");
                //} 
                #endregion

                progressBar.PerformStep();

                // 3. 注册psql服务
                #region 测试跳过
                //var result = await ExecApp.RunAsync(Path.Combine(psqlBinPath, "pg_ctl.exe"), $"register -N \"{psqlServiceName}\" -D \"{psqlDataPath}\"");
                //if (result.ExceptError != null || (string.IsNullOrEmpty(result.Output) && !string.IsNullOrEmpty(result.Error)))
                //{
                //    throw new Exception($"注册{psqlServiceName}服务失败：{result.ExceptError.Message ?? result.Error}");
                //} 
                //if (result.ExceptError != null || !ServiceController.GetServices().Exists(psqlServiceName))
                //{
                //    throw new Exception($"注册{psqlServiceName}服务失败：{result.ExceptError?.Message + result.Error}");
                //}
                #endregion

                progressBar.PerformStep();

                // 4. 启动服务
                // 不采用net命令管理启动服务，如果服务启动失败，仍会正常结束，并且获取不到未启动成功的信息。后续还需要判断服务状态才合理
                //var netStartResult = await ExecCMD.RunAsync($"net start {psqlServiceName}");

                // 直接使用服务控件管理启动服务，如果启动失败提示重启电脑

                var serviceController = new ServiceController(psqlServiceName);
                if (serviceController.Status != ServiceControllerStatus.Running)
                {
                    serviceController.Start();
                    try
                    {
                        // 超时等待将会引发异常
                        serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(5));
                    }
                    catch (Exception)
                    {
                        // 有一种进程启动，服务未启动的情况...
                    }
                    if (serviceController.Status != ServiceControllerStatus.Running)
                    {
                        var yesNoResult = MessageBox.Show("PostgreSQL已经安装成功，但是需要重启系统才能完成启动。\r\n如果现在重启，请确保已经保存所有的工作，并点击“是”。\r\n若需要后续手动重启，请点击“否”。", "提示", MessageBoxButtons.YesNo);
                        if (yesNoResult == DialogResult.Yes)
                        {
                            // 重启系统


                        }
                        else
                        {
                            // 关闭程序（暂时结束安装）
                        }
                        // 退出
                    }
                }

                //  注册表中记录相关操作进度，便于重启系统后可以继续执行

                //if (netStartResult.Contains()) // 输出内容判断成功还是失败
                //{
                //    throw new Exception($"启动{psqlServiceName}服务失败：{netStartResult}");
                //}

                //progressBar.PerformStep();
                progressBar.Value += 1;

                userName = "myuser1";
                userPwd = "mypwd1";
                // 5. 创建用户和数据库
                //var result_cuser = await ExecApp.RunAsync(Path.Combine(psqlBinPath, "createuser.exe"), $"-P {userName}", new string[] { userPwd, userPwd }); // 未必不能执行交互，因为实际服务未启动，肯定有问题，待测试，
                var result_cuser = await RunCMDPSqlCreateUserAsync(Path.Combine(psqlBinPath, "createuser.exe") + $" -P {userName}", userPwd);
                if (result_cuser.ExceptError != null || !string.IsNullOrWhiteSpace(result_cuser.Error))
                {
                    throw new Exception($"创建用户失败：{result_cuser.ExceptError?.Message}{Environment.NewLine}{ result_cuser.Error}");
                }

                #region 由于可能的失败输出乱码，尽量放在用户登陆后创建数据库
                ////var result_cdb = await ExecApp.RunAsync(Path.Combine(psqlBinPath, "createdb.exe"), $"{dbName} -O={userName}"); // 执行程序会返回乱码，指定编码也无法解决
                //var result_cdb = await ExecCMD.RunAsync(Path.Combine(psqlBinPath, "createdb.exe") + $" {dbName} -O={userName}");
                //if (result_cdb.ExceptError != null || !string.IsNullOrWhiteSpace(result_cdb.Error))
                //{
                //    throw new Exception($"创建数据库失败：{result_cdb.ExceptError?.Message }{Environment.NewLine}{ result_cdb.Error}");
                //} 
                #endregion

                //progressBar.PerformStep();
                progressBar.Value += 1;

                // 创建PgAdmin、psql开始菜单

                MessageBox.Show("数据库安装执行完成");
                progressBar.SetVisibleFalse();
            }
            catch (Exception ex)
            {
                // 清理执行的操作 只有第一步解压缩时发送错误才需要清理

                // 根据执行的步骤，在哪一步执行不同的错误处理，并保存记录到注册表，等待后续继续执行
                MessageBox.Show($"安装过程发生错误：{ex.Message}\r\n请确认错误并解决，清理{psqlInstallPath}路径下的内容后，重新安装！");
                progressBar.SetVisibleFalse();
            }
        }
        /// <summary>
        /// 执行cmd命令行创建用户，交互输入密码
        /// </summary>
        ///<param name="createCommand">执行的命令</param>
        /// <param name="userPwd">交互输入的口令密码</param>
        /// <returns>cmd命令执行的输出</returns>
        public static async System.Threading.Tasks.Task<ExecResult> RunCMDPSqlCreateUserAsync(string createCommand, string userPwd)
        {
            createCommand = createCommand.Trim().TrimEnd('&');

            string cmdFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "cmd.exe");// @"C:\Windows\System32\cmd.exe";
            using (Process p = new Process())
            {
                var result = new ExecResult();
                try
                {

                    p.StartInfo.FileName = cmdFileName;
                    p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                    p.StartInfo.RedirectStandardInput = true;   //接受来自调用程序的输入信息
                    p.StartInfo.RedirectStandardOutput = true;  //由调用程序获取输出信息
                    p.StartInfo.RedirectStandardError = true;   //重定向标准错误输出
                    p.StartInfo.CreateNoWindow = false;          //需要显示程序窗口（操作新打开的确认口令窗口）

                    p.Start();//启动程序

                    p.StandardInput.AutoFlush = false;
                    //向cmd窗口写入命令
                    p.StandardInput.WriteLine(createCommand);
                    p.StandardInput.Flush();

                    #region 由于新打开了窗口，尝试连续输入、或异步读取输出都无法与新窗口交互
                    // var output = string.Empty;
                    // var inputI = 0;
                    //p.OutputDataReceived += (sender, e)=> {
                    //    output+= e.Data;// 获取输出
                    //    //p.StandardInput.WriteLine(inputs[inputI]);
                    //    //inputI++;
                    //};

                    // 异步读取输出流
                    //p.BeginOutputReadLine();

                    // 循环输入交互信息 无效（不行）
                    //for (int i = 0; i < inputs.Length; i++)
                    //{
                    //    var input = inputs[i];
                    //    //// 写入标准输入
                    //    //p.StandardInput.WriteLine(input);

                    //    //p.StandardInput.Flush();
                    //    ////await p.StandardInput.FlushAsync();
                    //    //if (i == inputs.Length - 1)
                    //    //{
                    //    //    p.StandardInput.WriteLine("&exit");
                    //    //}
                    //}
                    #endregion

                    // 等待一会，等待新窗口打开
                    Thread.Sleep(500);

                    // 命令行窗口包标题-P前包含两个个空格，和传入的不符，因此无法查找到 @"管理员: C:\WINDOWS\system32\cmd.exe - C:\PostgreSQL\pgsql\bin\createuser.exe  -P myuser".Contains(command);
                    var windows = WndHelper.FindAllWindows(x => x.Title.Contains(@"C:\PostgreSQL\pgsql\bin\createuser.exe")); // x.Title.Contains(command)
                    var window = windows[0];

                    WndHelper.SendText(window.Hwnd, userPwd);
                    WndHelper.SendEnter(window.Hwnd);
                    // 等待
                    Thread.Sleep(100);

                    WndHelper.SendText(window.Hwnd, userPwd);
                    WndHelper.SendEnter(window.Hwnd);
                    // 等待 需要等待多一点再关闭，否则可能创建不成功
                    Thread.Sleep(500);

                    // 处理完后关闭窗口，否则后面一直阻塞
                    WndHelper.CloseWindow(window.Hwnd);
                    // 或者发送 exit 和回车

                    // 不能直接使用标准输出，会一直等待；需要关闭新打开的窗口（或执行完exti退出）
                    result.Output = await p.StandardOutput.ReadToEndAsync();
                    result.Error = await p.StandardError.ReadToEndAsync();
                    p.WaitForExit();//等待程序执行完退出进程。应在最后调用
                    p.Close();
                }
                catch (Exception ex)
                {
                    result.ExceptError = ex;
                }
                return result;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var lLongPathTest = new LongPathTest();
            lLongPathTest.Show();

            KeyUp += MainForm_KeyUp;
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
