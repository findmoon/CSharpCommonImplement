using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace CommonEffect_Elements.TreeViewEffect
{
    /// <summary>
    /// DirFileDisplayWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DirFileDisplayWindow : Window
    {
        /// <summary>
        /// 要显示的文件夹路径
        /// </summary>
        private string displayFolderPath;

        public DirFileDisplayWindow()
        {
            InitializeComponent();

            // 设置初始化显示的路径
            displayFolderPath=Environment.GetFolderPath(Environment.SpecialFolder.System);

            Loaded += MainWindow_Loaded;

            // 选择TreeView菜单项
            directoryTreeView.SelectedItemChanged += DirectoryTreeView_SelectedItemChanged;
        }

        private async void DirectoryTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is DirFileRecord record)
            {
                // 处理读取选择的文件/文件夹。比如读取文件内容
                // Encoding.UTF7 似乎更好
                //var text = await EncoderAndDecoder.ReadStringFromFileAsync(record.dirOrFilePath, Encoding.UTF7);

               // ..........
            }
        }


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetDirTreeViewDatas(displayFolderPath);
        }

        /// <summary>
        /// 设置 directoryTreeView.ItemsSource
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        private void SetDirTreeViewDatas(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                return;
            }
            var directory = new ObservableCollection<DirFileRecord>
            {
                new DirFileRecord(folderPath)
            };
            directoryTreeView.ItemsSource = directory;
        }
    }
}
