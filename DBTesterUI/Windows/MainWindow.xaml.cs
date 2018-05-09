using System.Windows;
using System.Windows.Controls;

namespace DBTesterUI.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex = 1;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            TabControl.SelectedIndex = 0;
        }

        private void DbListCheckbox_OnChecked(object sender, RoutedEventArgs e)
        {
            Next1Button.GetBindingExpression(IsEnabledProperty)?.UpdateTarget();
        }
    }
}
