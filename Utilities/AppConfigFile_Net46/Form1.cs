using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AppConfigFile_Net46
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var key1 = ConfigurationHelper.GetAppSettingValue("key1");
            var key1_defalut = ConfigurationHelper.GetAppSettingValue("key1", "key1_default");

            ConfigurationHelper.SetAppSettings("key1", "key1_value");
            ConfigurationHelper.SetAppSettings("key2", "key2_value");
            ConfigurationHelper.SetAppSettings("key3", "key3_value");

            ConfigurationHelper.SetAppSettings(new KeyValuePair<string, string>("1", "2"));
            ConfigurationHelper.SetAppSettings(new KeyValuePair<string, string>("1", "5"), new KeyValuePair<string, string>("2", "6"));


            ConfigurationHelper.SetAppSettings("keyM.1.2.3", "keyM.1.2.3_value");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ConfigurationHelper.GetAppSettingValue("key2"));

            ConfigurationHelper.SetAppSettings("key2", "key2_" + (new Random().Next()));

            MessageBox.Show(ConfigurationHelper.GetAppSettingValue("key2"));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show(ConfigurationHelper.GetAppSettingValue("key3"));

            ConfigurationHelper.DeleteAppSettings("key3");

            MessageBox.Show(ConfigurationHelper.GetAppSettingValue("key3"));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // 不会保存到文件
            ConfigurationManager.AppSettings.Set("key1", "修改!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var server1_connStr = ConfigurationManager.ConnectionStrings["SQLServer1"].ConnectionString;
            var mariadb_connStr = ConfigurationManager.ConnectionStrings["Mariadb"].ConnectionString;

            MessageBox.Show(server1_connStr);

            // 报错 该配置是只读的
            //ConfigurationManager.ConnectionStrings["SQLServer2"].ConnectionString = "server=127.0.0.1;database=数据库名; Ueer ID=用户;Password=密码";



            var connStr = "server=127.0.0.1;database=数据库名; Ueer ID=用户;Password=密码";
            var connStrName = "SQLServer2";
            SetConnectionStrings(connStrName, connStr);

            var server2_connStr = ConfigurationManager.ConnectionStrings["SQLServer2"].ConnectionString;

            SetConnectionStrings("SQLServer测试", connStr);

            var serverT_connStr = ConfigurationManager.ConnectionStrings["SQLServer测试"].ConnectionString;

            MessageBox.Show(serverT_connStr);
        }
        /// <summary>
        /// 修改 或 添加 ConnectionStrings 连接字符串 并 保存
        /// </summary>
        /// <param name="connStr"></param>
        /// <param name="connStrName"></param>
        /// <param name="providerName">SQLServer为 System.Data.SqlClient；</param>
        private static void SetConnectionStrings(string connStrName, string connStr, string providerName = null)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);//当前应用程序的配置文件
            var connStrSets = config.ConnectionStrings.ConnectionStrings[connStrName];
            if (connStrSets != null)
            {
                connStrSets.ConnectionString = connStr;
                if (!string.IsNullOrWhiteSpace(providerName))
                {
                    connStrSets.ProviderName = providerName;
                }
            }
            else
            {
                config.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings()
                {
                    ConnectionString = connStr,
                    Name = connStrName,
                    ProviderName = providerName
                });
            }
            config.Save(); //保存配置文件  // config.Save(ConfigurationSaveMode.Full);
            //ConfigurationManager.RefreshSection("connectionStrings"); // 刷新，更新缓存。无需重启，获取最新的配置值
            ConfigurationManager.RefreshSection(config.ConnectionStrings.SectionInformation.Name);
        }
    }
}
