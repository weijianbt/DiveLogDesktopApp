using DiveLogApplication.Core;
using System;
using System.Windows.Controls;

namespace DiveLogApplication.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private Frame _selectedContent;

        public MainWindowViewModel()
        {
            _selectedContent = new Frame();
            WireCommands();
        }

        public RelayCommand NavigateMainPageCommand { get; private set; }
        public RelayCommand NavigateUserProfileCommand { get; private set; }
        public RelayCommand NavigateDiveLogCommand { get; private set; }
        public RelayCommand NavigateSettingsCommand { get; private set; }

        public Frame SelectedContent
        {
            get => _selectedContent;
            set
            {
                _selectedContent = value;
                OnPropertyChanged();
            }
        }

        private void WireCommands()
        {
            NavigateMainPageCommand = new RelayCommand(
                param =>
                {
                    SelectedContent.Navigate(new Uri("Views/MainPageView.xaml", UriKind.Relative));
                },
                param => true
                );

            NavigateUserProfileCommand = new RelayCommand(
                param =>
                {
                    //var userProfileView = new UserProfileView();
                    //var userProfileViewModel = new UserProfileViewModel
                    //{
                    //    Name = "Yee Cheng"
                    //};

                    //userProfileView.DataContext = userProfileViewModel;
                    //SelectedContent.Navigate(userProfileView);
                    SelectedContent.Navigate(new Uri("Views/UserProfileView.xaml", UriKind.Relative));
                },
                param => true
                );

            NavigateDiveLogCommand = new RelayCommand(
                param =>
                {
                    SelectedContent.Navigate(new Uri("Views/DiveLogView.xaml", UriKind.Relative));
                },
                param => true
                );

            NavigateSettingsCommand = new RelayCommand(
                param =>
                {
                    SelectedContent.Navigate(new Uri("Views/SettingsView.xaml", UriKind.Relative));
                },
                param => true);
        }

    }
}
