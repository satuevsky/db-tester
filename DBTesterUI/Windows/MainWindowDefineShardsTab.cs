using System.Threading;
using System.Windows;
using System.Windows.Controls;
using DBTesterUI.Models.Config;

namespace DBTesterUI.Windows
{
    public partial class MainWindow
    {
        private void MachinesCountUpDown_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateElement(DefineShardsTab);
        }

        private void AddShardGroupButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShardGroupsModel.AddGroup();
            UpdateElement(DefineShardsTab);
        }

        private void DeleteShardGroupButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShardGroupsModel.RemoveGroup((sender as Button)?.DataContext as DbShardGroup);
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