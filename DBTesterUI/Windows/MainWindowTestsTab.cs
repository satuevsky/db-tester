using System.Windows.Controls;
using DBTesterUI.Models.TestModel;

namespace DBTesterUI.Windows
{
    public partial class MainWindow
    {
        private void TestsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(TestModel == null) return;
            TestModel.SelectedTest = ((ListView)sender).SelectedItem as DbTestItem;
        }
    }
}