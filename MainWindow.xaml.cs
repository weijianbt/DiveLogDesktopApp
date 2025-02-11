using DiveLogApplication.ViewModels;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace DiveLogApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm)
            {
                vm.NavigateMainPageCommand.Execute(null);
            }
        }

        private void ToggleButton_Clicked(object sender, RoutedEventArgs e)
        {
            int childAmount = VisualTreeHelper.GetChildrenCount((sender as ToggleButton).Parent);

            ToggleButton tb;
            for (int i = 0; i < childAmount; i++)
            {
                tb = null;
                tb = VisualTreeHelper.GetChild((sender as ToggleButton).Parent, i) as ToggleButton;

                if (tb != null)
                    tb.IsChecked = false;
            }

            (sender as ToggleButton).IsChecked = true;
        }
    }
}
