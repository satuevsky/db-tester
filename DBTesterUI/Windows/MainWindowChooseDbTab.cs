using System.Windows;

namespace DBTesterUI.Windows
{
    public partial class MainWindow
    {
        private void DbListCheckbox_OnChecked(object sender, RoutedEventArgs e)
        {
            UpdateElement(ChooseDbsTab);
        }
    }
}