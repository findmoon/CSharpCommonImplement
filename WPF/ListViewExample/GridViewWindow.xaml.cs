using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ListViewExample
{
    /// <summary>
    /// GridViewWindow.xaml 的交互逻辑
    /// </summary>
    public partial class GridViewWindow : Window
    {
        public GridViewWindow()
        {
            InitializeComponent();

            // 右键菜单的位置
            //listViewContextMenu.Placement = PlacementMode.Right | PlacementMode.Bottom;
            //listViewContextMenu.HorizontalOffset = 0;
            //listViewContextMenu.VerticalOffset = 0;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
