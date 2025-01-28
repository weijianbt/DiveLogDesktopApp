using DiveLogApplication.Core;
using DiveLogApplication.Views;
using System;
using System.Windows;
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

        private DiveLogViewModel _diveLogViewModel;
        private UserProfileViewModel _userProfileViewModel;
        private SettingsViewModel _settingsViewModel;

        private string _welcomeMessage;
        private int _totalDives;

        public MainWindowViewModel()
        {
            _selectedContent = new Frame();
            LoadViewModels();
            LoadUserProfileData();
            LoadDiveLogData();
            WireCommands();
        }

        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged();
            }
        }

        public int TotalDives
        {
            get => _totalDives;
            set
            {
                _totalDives = value;
                OnPropertyChanged();
            }
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

        private void LoadViewModels()
        {
            _diveLogViewModel = new DiveLogViewModel();
            _diveLogViewModel.LoadedCommand.Execute(null);

            _userProfileViewModel = new UserProfileViewModel();
            _userProfileViewModel.LoadLicenseCommand.Execute(null);

            TotalDives = _diveLogViewModel.DiveLogList.Count;
            WelcomeMessage = $"Welcome! You have a total of {TotalDives} dives.";
        }

        private void LoadDiveLogData()
        {
            throw new NotImplementedException();
        }

        private void LoadUserProfileData()
        {
            throw new NotImplementedException();
        }

    }
}
