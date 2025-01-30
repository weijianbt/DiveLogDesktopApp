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

        private DiveLogViewModel _diveLogViewModel;
        private UserProfileViewModel _userProfileViewModel;
        private SettingsViewModel _settingsViewModel;

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

        public MainWindowViewModel()
        {
            _selectedContent = new Frame();
            LoadViewModels();
            LoadDiveLogData();
            LoadUserProfileData();
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

        public string DiverSinceMessage
        {
            get => _diverSinceMessage;
            set 
            { 
                _diverSinceMessage = value;
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

        public int TotalDiveLicenses
        {
            get => _totalDiveLicenses;
            set
            {
                _totalDiveLicenses = value;
                OnPropertyChanged();
            }
        }

        public double LongestDive
        {
            get => _longestDive;
            set 
            { 
                _longestDive = value; 
                OnPropertyChanged();
            }
        }

        public double DeepestDive
        {
            get => _deepestDive;
            set
            {
                _deepestDive = value;
                OnPropertyChanged();
            }
        }

        public string MostFrequentDiveSite
        {
            get => _mostFrequentDiveSite;
            set
            {
                _mostFrequentDiveSite = value;
                OnPropertyChanged();
            }
        }

        public DateTime LastDiveDate
        {
            get => _lastDiveDate;
            set 
            { 
                _lastDiveDate = value; 
                OnPropertyChanged();
            }
        }

        public double AverageDepth
        {
            get => _averageDepth;
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
 
        }

        private void LoadDiveLogData()
        {
            TotalDives = _diveLogViewModel.DiveLogList.Count;
            LongestDive = _diveLogViewModel.DiveLogList.Max(p => p.EndTime.Subtract(p.StartTime).TotalMinutes);
            DeepestDive = _diveLogViewModel.DiveLogList.Max(p => p.MaxDepth);
            MostFrequentDiveSite = _diveLogViewModel.DiveLogList
                .Where(p => !string.IsNullOrWhiteSpace(p.DiveSite)) // Filter out empty/null entries
                .GroupBy(p => p.DiveSite)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()
                ?.Key ?? "N/A"; // Fallback if no valid DiveSites exist

            AverageDepth = Math.Round(_diveLogViewModel.DiveLogList.Average(p => p.AverageDepth), 2);
            LastDiveDate = _diveLogViewModel.DiveLogList.Max(p => p.EndTime);
        }

        private void LoadUserProfileData()
        {

            List<DateTime> dateTimeList = new List<DateTime>();
            foreach(DiveLicense diveLicense in _userProfileViewModel.DiveLicenseList)
            {
                dateTimeList.Add(diveLicense.IssuedDate);
            }

            DateTime earliestLicense = dateTimeList.Min();

            WelcomeMessage = $"Welcome! You have a total of {TotalDives} dives.";
            DiverSinceMessage = $"Diver since {earliestLicense}";
            TotalDiveLicenses = _userProfileViewModel.DiveLicenseList.Count;
        }

    }
}
