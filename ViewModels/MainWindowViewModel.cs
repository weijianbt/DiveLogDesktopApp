using DiveLogApplication.Core;
using DiveLogApplication.Models;
using DiveLogApplication.Views;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // dive license parameters
        private string _welcomeMessage;
        private string _diverSinceMessage;
        private int _totalDives;
        private int _totalDiveLicenses;

        // dive log parameters
        private double _longestDive;
        private double _deepestDive;
        private string _mostFrequentDiveSite;
        private DateTime _lastDiveDate;
        private double _averageDepth;

        private DiveLogAppData _diveLogAppData;

        public MainWindowViewModel()
        {
            _selectedContent = new Frame();
            _diveLogAppData = new DiveLogAppData();

            WireCommands();
        }

        public string WelcomeMessage
        {
            get { return _diveLogAppData.WelcomeMessage; }
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged();
            }
        }

        public string DiverSinceMessage
        {
            get { return _diveLogAppData.DiverSinceMessage; }
            set 
            { 
                _diverSinceMessage = value;
                OnPropertyChanged();
            }
        }

        public int TotalDives
        {
            get { return _diveLogAppData.TotalDives; }
            set
            {
                _totalDives = value;
                OnPropertyChanged();
            }
        }

        public int TotalDiveLicenses
        {
            get { return _diveLogAppData.TotalDiveLicenses; }
            set
            {
                _totalDiveLicenses = value;
                OnPropertyChanged();
            }
        }

        public double LongestDive
        {
            get { return _diveLogAppData.LongestDive; }
            set 
            { 
                _longestDive = value; 
                OnPropertyChanged();
            }
        }

        public double DeepestDive
        {
            get { return _diveLogAppData.DeepestDive; }
            set
            {
                _deepestDive = value;
                OnPropertyChanged();
            }
        }

        public string MostFrequentDiveSite
        {
            get { return _diveLogAppData.MostFrequentDiveSite; }
            set
            {
                _mostFrequentDiveSite = value;
                OnPropertyChanged();
            }
        }

        public DateTime LastDiveDate
        {
            get { return _diveLogAppData.LastDiveDate; }
            set 
            { 
                _lastDiveDate = value; 
                OnPropertyChanged();
            }
        }

        public double AverageDepth
        {
            get { return _diveLogAppData.AverageDepth; }
            set
            {
                _averageDepth = value;
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

                        UserProfileViewModel vm = new UserProfileViewModel(_diveLogAppData);
                        _userProfileView = new UserProfileView
                        {
                            DataContext = vm
                        };
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
