using DiveLogApplication.Core;
using DiveLogApplication.Views;
using System;
using System.Windows.Controls;

namespace DiveLogApplication.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private Frame _selectedContent;
        private MainPageView _mainPageView;
        private UserProfileView _userProfileView;
        private DiveLogView _diveLogView;
        private SettingsView _settingsView;


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
                    if (_mainPageView == null)
                    {
                        _mainPageView = new MainPageView();
                    }

                    SelectedContent.Content = _mainPageView;
                },
                param => true
                );

            NavigateUserProfileCommand = new RelayCommand(
                param =>
                {
                    if (_userProfileView == null)
                    {
                        _userProfileView = new UserProfileView();
                    }

                    SelectedContent.Content = _userProfileView;
                },
                param => true
                );

            NavigateDiveLogCommand = new RelayCommand(
                param =>
                {
                    if (_diveLogView == null)
                    {
                        _diveLogView = new DiveLogView();
                    }

                    SelectedContent.Content = _diveLogView;
                },
                param => true
                );

            NavigateSettingsCommand = new RelayCommand(
                param =>
                {
                    if (_settingsView == null)
                    {
                        _settingsView = new SettingsView();
                    }

                    SelectedContent.Content = _settingsView;
                },
                param => true);
        }

    }
}
