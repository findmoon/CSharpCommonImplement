using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
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
