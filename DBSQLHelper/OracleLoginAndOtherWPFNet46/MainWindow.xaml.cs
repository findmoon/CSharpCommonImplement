using System;
using System.Data;
using System.Windows;

namespace OracleLoginAndOtherWPFNet46
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // OracleConnectionStringBuilder
                using (var sqlHelper = OracleSQLHelper.WindowsAuthentication(DBName.Text.Trim(),serverIp.Text.Trim()))
                {
                    MessageBox.Show("Windows用户认证登陆Oracle成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+ Environment.NewLine+ ex.LastMessage() + Environment.NewLine + ex.StackTrace);
            }
        }
    }
}
