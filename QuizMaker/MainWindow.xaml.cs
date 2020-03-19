using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProblemEditor
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel();
        }

        private void TreeView_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            TreeView treeview = (TreeView)sender;

            if (treeview.SelectedItem == null)
                return;

            // Context menu create
            ContextMenu contextMenu = new ContextMenu();

            MenuItem add = new MenuItem();
            add.Header = "Add Problem";
            add.Click += AddNodeClick;

            MenuItem delete = new MenuItem();
            delete.Header = "Delete Node";
            delete.Click += Delete_Click;

            contextMenu.Items.Add(add);
            contextMenu.Items.Add(delete);

            treeview.ContextMenu = contextMenu;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (MyTreeview.SelectedItem != null)
            {
                MainViewModel model = (MainViewModel)this.DataContext;
                if (model == null)
                    return;

                BaseItem item = (BaseItem)MyTreeview.SelectedItem;
                if (item == null) { return; }
                model.DeleteCommand?.Execute(item);
            }
        }

        private void AddNodeClick(object sender, RoutedEventArgs e)
        {
            MainViewModel model = (MainViewModel)this.DataContext;
            if (model == null)
                return;

            BaseItem item = (BaseItem)MyTreeview.SelectedItem;
            if (item != null)
            {
                model.AddNode(item);
            }                
        }

        private void MyTreeview_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            MainViewModel model = (MainViewModel)this.DataContext;
            if (model == null)
                return;

            if (MyTreeview.SelectedItem != null)
            {
                model.SelectCommand?.Execute((BaseItem)MyTreeview.SelectedItem);
            }
            
        }
    }
}
