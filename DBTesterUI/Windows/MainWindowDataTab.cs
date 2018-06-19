using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DBTesterUI.Models.Config.DataModel;

namespace DBTesterUI.Windows
{
    public partial class MainWindow
    {
        private void DeleteColumnButton_OnClick(object sender, RoutedEventArgs e)
        {
            var row = (sender as Button).DataContext as DbDataColumn;
            DataModel.Columns.Remove(row);
        }

        private void DataTabNextButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowTestTab();
        }

        private void Back3_OnClick(object sender, RoutedEventArgs e)
        {
            ShowDefineShardsTab();
        }
    }
}