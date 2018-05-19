using System.Threading;
using System.Windows;
using System.Windows.Controls;
using DBTesterUI.Models.Config;

namespace DBTesterUI.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DbItemsModel DbItemsModel => ChooseDbsTab.DataContext as DbItemsModel;
        private DbShardGroupsModel DbShardGroupsModel => DefineShardsTab.DataContext as DbShardGroupsModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        void UpdateElement(FrameworkElement element)
        {
            Dispatcher.Invoke(() =>
            {
                var data = element.DataContext;
                element.DataContext = null;
                element.DataContext = data;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TabControl1.SelectedIndex = 1;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TabControl1.SelectedIndex = 0;
        }

        private void DbListCheckbox_OnChecked(object sender, RoutedEventArgs e)
        {
            UpdateElement(ChooseDbsTab);
        }

        private void Next1Button_OnClick(object sender, RoutedEventArgs e)
        {
            DefineShardsTab.DataContext = new DbShardGroupsModel(DbItemsModel);
            TabControl1.SelectedIndex = 1;
        }

        private void Back2_OnClick(object sender, RoutedEventArgs e)
        {
            TabControl1.SelectedIndex = 0;
        }

        private void MachinesCountUpDown_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateElement(DefineShardsTab);
        }

        private void AddShardGroupButton_OnClick(object sender, RoutedEventArgs e)
        {
            DbShardGroupsModel.AddGroup();
            UpdateElement(DefineShardsTab);
        }

        private void DeleteShardGroupButton_OnClick(object sender, RoutedEventArgs e)
        {
            DbShardGroupsModel.RemoveGroup((sender as Button)?.DataContext as DbShardGroup);
            UpdateElement(DefineShardsTab);
        }

        private void CheckConnectionStringButton_OnClick(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is DbShardGroupItem item)
            {
                if (item.ConnectionStringState == ConnectionStringState.Checking || item.ConnectionString.Trim() == "")
                {
                    return;
                }

                item.ConnectionStringState = ConnectionStringState.Checking;
                UpdateElement(DefineShardsTab);

                new Thread(() =>
                {
                    item.ConnectionStringState = item.Db.CheckConnectionString(item.ConnectionString)
                        ? ConnectionStringState.Valid
                        : ConnectionStringState.NotValid;

                    UpdateElement(DefineShardsTab);
                }).Start();
            }
        }
    }
}