using CMCode.Call;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OracleSQLPlusCMDExecSqlFile
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 直接使用，整个窗体拖拽
            // 但是，如果窗体内有TextBox文本框，点击文本框并拖动，将会发生错误 System.InvalidOperationException:“调度程序进程已挂起，但消息仍在处理中。”
            MouseMove += (sender, e) =>
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    DragMove();
                }
            };
            // 如何获取 所有的内部 TextBox 并通知设置事件处理？
            infoTxt.MouseMove += (sender, e) =>
            {
                e.Handled= true;
            };

            Deactivated += MainWindow_Deactivated;

            timer = new Timer(obj =>
            {
                Debug.WriteLine(enterCount);
                enterCount = 0;
                timer.Change(Timeout.Infinite, Timeout.Infinite);
            }, null, Timeout.Infinite, Timeout.Infinite);
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            viewSQLContentBtn.Visibility= Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.hadExeced == true)
            {
                MessageBox.Show("已经初始化过！");
                return;
            }

            string tempFile = System.IO.Path.Combine(System.IO.Path.GetTempPath(),$"OracleUserInit_{System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.GetRandomFileName())}.sql");
            File.WriteAllText(tempFile, sqlScript);

            var result = ExecCMD.Run("sqlplus /@neximdb as sysdba @" + tempFile);

            File.Delete(tempFile);

            infoTxt.Text = result.Output;

            if (result.ExceptError!=null || !string.IsNullOrWhiteSpace(result.Error) || !result.Output.Contains("success!"))
            {
                MessageBox.Show("发生了问题，可能无法初始化");
            }
            else
            {
                Properties.Settings.Default.hadExeced= true;
                Properties.Settings.Default.Save();
                MessageBox.Show("结束！");
            }
        }

        string sqlScript = @"SET SERVEROUTPUT ON;

DECLARE cnt number;

BEGIN
    -- 判断时，username必须为全大写
	select count(1) into cnt FROM all_users where username='ASKRETRIVEUSER';
	IF cnt = 0 THEN
        execute immediate 'create user ASKRetriveUser identified by Ask2022';
        execute immediate 'grant create session to ASKRetriveUser';
        execute immediate 'grant connect to  ASKRetriveUser';
        execute immediate 'grant select any table to ASKRetriveUser';
	END IF;
    DBMS_OUTPUT.PUT_LINE('执行成功, success!');
END;
/

EXIT";

        int enterCount = 0;
        private Timer timer;


        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                enterCount += 1;
                if (enterCount >= 5)
                {
                    viewSQLContentBtn.Visibility= Visibility.Visible;
                }
                timer.Change(800, Timeout.Infinite);
            }
            else
            {
                enterCount = 0;
            }
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            infoTxt.Text = $"sqlplus /@neximdb as sysdba{Environment.NewLine}{Environment.NewLine}{sqlScript}";
        }

        private void viewSQLContentBtn_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.P)
            {
                timer.Change(Timeout.Infinite, Timeout.Infinite);
                enterCount += 1;
                Debug.WriteLine(enterCount);
                if (enterCount >= 10)
                {
                    Properties.Settings.Default.hadExeced = false;
                    Properties.Settings.Default.Save();
                    MessageBox.Show("OK!");
                }
                timer.Change(800, Timeout.Infinite);

                e.Handled= true;
            }
            else
            {
                enterCount = 0;
            }

            // 停止事件上传，否则 key 事件上传到Window窗体(或父级元素)事件处理 中，可能会有处理冲突
            
        }
    }
}
