using System.Windows.Controls;

namespace DBTesterUI.Windows
{
    public partial class MainWindow
    {
        private void TestsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(TestModel == null) return;
            var index = ((ListView) sender).SelectedIndex;
            TestModel.SelectedTest = TestModel?.Tests[index];
        }
    }
}