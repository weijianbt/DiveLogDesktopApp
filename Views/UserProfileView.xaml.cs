using DiveLogApplication.Models;
using DiveLogApplication.ViewModels;
using System.Windows.Controls;

namespace DiveLogApplication.Views
{
    /// <summary>
    /// Interaction logic for UserProfileView.xaml
    /// </summary>
    public partial class UserProfileView : Page
    {
        public UserProfileView()
        {
            InitializeComponent();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataContext is UserProfileViewModel vm)
            {
                if (e.AddedItems.Count > 0)
                {
                    vm.SelectedDiveLicense = (DiveLicense)e.AddedItems[0];
                }
            }
        }
    }
}
