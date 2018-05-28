using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DBTesterUI.Models.Config;

namespace DBTesterUI.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DbItemsModel ItemsModel => ChooseDbsTab.DataContext as DbItemsModel;
        private DbShardGroupsModel ShardGroupsModel => DefineShardsTab.DataContext as DbShardGroupsModel;
        private DbDataModel DataModel => DefineDataTab.DataContext as DbDataModel;
        private DbTestModel TestModel => TestTab.DataContext as DbTestModel;



        public MainWindow()
        {
            InitializeComponent();
        }

        void UpdateElement(FrameworkElement element)
        {
            Dispatcher.Invoke(() =>
            {
//                var data = element.DataContext;
//                element.DataContext = null;
//                element.DataContext = data;
            });
        }
    }
}