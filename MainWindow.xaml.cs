using DiveLogApplication.ViewModels;
using System.Windows;

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
    }
}
