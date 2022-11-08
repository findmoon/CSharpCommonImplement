using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace System.Text
{
    static public class Log
    {
        private static readonly ReaderWriterLockSlim writerLockSlim = new ReaderWriterLockSlim();

        //static Log(TextBox textBox)//静态构造函数必须无参
        //{
        //    LogTxtBox = textBox;
        //}
        /// <summary>
        /// log信息显示在的TextBox控件，不指定将不进行任何操作
        /// </summary>
        public static TextBox LogTxtBox { get; set; }
        /// <summary>
        /// 显示log的TextBox控件最大行数，默认99行,最小值为1。
        /// </summary>
        public static int LogTxtBoxMaxLines
        {
            get => _logTxtBoxMaxLines;
            set
            {
                if (value < 1)
                {
                    value = 99;
                }
                _logTxtBoxMaxLines = value;
            }
        }
        /// <summary>
        /// TextBox最大行数
        /// </summary>
        private static int _logTxtBoxMaxLines = 99;
        /// <summary>
        /// log文件保存的文件夹路径，默认为当前程序下log目录中
        /// </summary>
        public static string LogPath { get; set; } = "log";
        /// <summary>
        /// log文件保留的天数期限，超过将删除
        /// </summary>
        public static int LogExprireDays { get; set; } = 0;
        /// <summary>
        /// 日志生成类型，仅支持 "D"/"H"，标识按天或按小时生成，默认"D"
        /// </summary>
        public static string GenerateType
        {
            get => _generateType;
            set
            {
                switch (value.ToUpper())
                {
                    case "H":
                        value = "H";
                        break;
                    default:
                        value = "D";
                        break;
                }
                _generateType = value;
            }
        }
        /// <summary>
        /// 日志生成类型
        /// </summary>
        private static string _generateType = "D";

        #region //Log处理
        /// <summary>
        /// 处理日志信息的写入等【不推荐直接使用，而应该使用 DealLogInfoAsync 】
        /// </summary>
        /// <param name="logInfo">日志内容</param>
        /// <param name="isShowMessageBox">是否显示消息框展示日志信息</param>
        /// <param name="addTime">是否在日志内容前面添加时间标识，默认添加</param>
        public static void DealLogInfo(string logInfo, bool isShowMessageBox = false, bool addTime = true)
        {
            if (addTime)
            {
                logInfo = $"[{ DateTime.Now.ToString("yyyMMddHHmmss")}]: {logInfo}\r\n";
            }
            else
            {
                logInfo = $"{logInfo}\r\n";
            }
            //texBox log处理
            DoAppendTxtLog(logInfo);

            if (!string.IsNullOrWhiteSpace(LogPath))
            {
                //log file 
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }
                var logFile = DateTime.Now.ToString("yyyyMMdd");
                switch (_generateType)
                {
                    case "H":
                        logFile = DateTime.Now.ToString("yyyyMMddHH");
                        break;
                    case "D":
                    default:
                        break;
                }
                string fileName = Path.Combine(LogPath, logFile + ".log");
                try
                {
                    if (LogExprireDays > 0)
                    {
                        try
                        {
                            // 获取超过期限的log
                            var files = Directory.GetFiles(LogPath, "*.log");
                            var needDelFiles = files.Where(f => File.GetLastWriteTime(f).AddDays(LogExprireDays) < DateTime.Now);
                            foreach (var needDelFile in needDelFiles)
                            {
                                File.Delete(needDelFile);
                            }
                        }
                        catch (Exception ex)
                        {
                            logInfo += $"log文件最多保留{LogExprireDays}天，但删除过期的log文件时发生错误：{ex.Message}\r\n";
                        }
                    }

                    if (writerLockSlim.TryEnterWriteLock(TimeSpan.FromMinutes(10)))
                    {
                        File.AppendAllText(fileName, logInfo, Encoding.UTF8);//追加，不存在则创建
                    }
                    else
                    {
                        MessageBox.Show($"{fileName}文件被其他进程占用,无法写入日志！");
                    }

                }
                catch (Exception ex)
                {
                    DealLogInfoAsync(ex.Message, isShowMessageBox);
                }
                finally
                {
                    if (writerLockSlim.IsWriteLockHeld)
                    {
                        writerLockSlim.ExitWriteLock();
                    }
                }
            }
            if (isShowMessageBox)
            {
                MessageBox.Show(logInfo);
            }
        }
        /// <summary>
        /// 异步的方式处理日志记录
        /// </summary>
        /// <param name="logInfo">日志内容</param>
        /// <param name="isShowMessageBox">是否显示消息框展示日志信息</param>
        public static void DealLogInfoAsync(string logInfo, bool isShowMessageBox = false)
        {
#if NET45_OR_GREATER
            #region Task 异步处理
            // 如果Task的实例化和执行需要分开时，才应该这么执行。比如Task创建后有条件的执行；通常则推荐使用 Task.Run 或 TaskFactory.StartNew 方法
            //new Task(() =>
            //{
            //    DealLogInfo($"[{DateTime.Now.ToString("yyyMMddHHmmss")}]: {logInfo}", isShowMessageBox, false);
            //}).Start();

            Task.Run(() =>
            {
                DealLogInfo($"[{DateTime.Now.ToString("yyyMMddHHmmss")}]: {logInfo}", isShowMessageBox, false);
            });
            #endregion
#else
            #region BeginInvoke 的处理方式，改为 Task
            // 仅 BeginInvoke 
            //new Action(() =>
            //{
            //    DealLogInfo($"[{ DateTime.Now.ToString("yyyMMddHHmmss")}]: {logInfo}", isShowMessageBox, false);
            //}).BeginInvoke(null, null);

            // BeginInvoke 后 成对 调用 EndInvoke
            var logAction = new Action(() =>
            {
                DealLogInfo($"[{DateTime.Now.ToString("yyyMMddHHmmss")}]: {logInfo}", isShowMessageBox, false);
            });

            logAction.BeginInvoke(ar =>
            {
                logAction.EndInvoke(ar);
            }, null);
            #endregion
#endif

        }

        static void DoAppendTxtLog(string lineStr)
        {
            if (LogTxtBox == null || LogTxtBox.IsDisposed)
            {
                return;
            }

            LogTxtBox.BeginInvoke(new Action<string>(line =>
            {
                if (LogTxtBox.Lines.Length >= _logTxtBoxMaxLines)//文本行大于n行
                {
                    if (LogTxtBox.Lines.Length == 1)
                    {
                        LogTxtBox.Text = "";
                    }
                    else
                    {
                        var logStrings = LogTxtBox.Text.Split(new string[] { "\r\n" }, StringSplitOptions.None);

                        //写入之前 剔除LogTxt.Text的一大部分
                        var remLines = _logTxtBoxMaxLines * 2 / 3;

                        LogTxtBox.Text = string.Join("\r\n", logStrings, remLines, logStrings.Length - remLines);
                    }
                }
                //追加在logTxt尾部
                LogTxtBox.AppendText(line);
            }), lineStr);
        }
        #endregion
    }
}
