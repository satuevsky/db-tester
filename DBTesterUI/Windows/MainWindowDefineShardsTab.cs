using System.Threading;
using System.Windows;
using System.Windows.Controls;
using DBTesterUI.Models.Config;

namespace DBTesterUI.Windows
{
    public partial class MainWindow
    {

        private void AddShardGroupButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShardGroupsModel.AddGroup();
        }

        private void DeleteShardGroupButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShardGroupsModel.RemoveGroup((sender as Button)?.DataContext as DbShardGroup);
        }

        private void CheckConnectionStringButton_OnClick(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is DbShardGroupItem item)
            {
                if (item.ConnectionStringState == ConnectionStringState.Checking || item.ConnectionString == null || item.ConnectionString.Trim() == "")
                {
                    return;
                }

                item.ConnectionStringState = ConnectionStringState.Checking;

                new Thread(() =>
                {
                    item.ConnectionStringState = item.Db.CheckConnectionString(item.ConnectionString)
                        ? ConnectionStringState.Valid
                        : ConnectionStringState.NotValid;
                    
                }).Start();
            }
        }
    }
}