using System.Timers;
using System.Windows;
using DBTesterUI.Models.Config;

namespace DBTesterUI.Windows
{
    public partial class MainWindow
    {
        public void ShowChooseDbsTab()
        {
            TabControl1.SelectedIndex = 0;
        }

        public void ShowDefineShardsTab(bool resetModel = true)
        {
            if (resetModel)
            {
                DefineShardsTab.DataContext = new DbShardGroupsModel(ItemsModel);
            }

            TabControl1.SelectedIndex = 1;
        }

        public void ShowDefineDataTab()
        {
            TabControl1.SelectedIndex = 2;
        }

        public void ShowTestTab()
        {
            TabControl1.SelectedIndex = 3;
            TestTab.DataContext = new DbTestModel(ShardGroupsModel, DataModel);
            TestModel.Start();

            var timer = new Timer(50);
            timer.Elapsed += (sender, args) => { TestGraphic.InvalidatePlot(); };
            timer.Start();
        }

        private void Next1Button_OnClick(object sender, RoutedEventArgs e)
        {
            ShowDefineShardsTab();
        }

        private void Back2_OnClick(object sender, RoutedEventArgs e)
        {
            ShowChooseDbsTab();
        }

        private void Next2Button_OnClick(object sender, RoutedEventArgs e)
        {
            ShowTestTab();
        }
    }
}