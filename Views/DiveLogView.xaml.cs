using DiveLogApplication.Models;
using DiveLogApplication.ViewModels;
using System.Windows.Controls;

namespace DiveLogApplication.Views
{
    /// <summary>
    /// Interaction logic for DiveLogView.xaml
    /// </summary>
    public partial class DiveLogView : Page
    {
        public DiveLogView()
        {
            InitializeComponent();
        }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (DataContext is DiveLogViewModel vm &&
                sender is ListBox lb &&
                lb.SelectedValue is DiveEntry selectedItem)
            {
                vm.OpenDetailsCommand.Execute(selectedItem);
            }
        }
    }
}
