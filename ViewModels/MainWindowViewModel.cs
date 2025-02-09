using DiveLogApplication.Core;
using DiveLogApplication.Models;
using DiveLogApplication.Views;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
        private string _currentDatetime;

        public MainWindowViewModel()
        {
            _selectedContent = new Frame();
            _diveLogAppData = new DiveLogAppData();

            WireCommands();
            StartTimer();
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

        public string CurrentDateTime
        {
            get { return _currentDatetime; }
            set
            {
                _currentDatetime = value;
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
                    _mainPageView = new MainPageView();
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
                        DiveLogViewModel vm = new DiveLogViewModel(_diveLogAppData);
                        _diveLogView = new DiveLogView
                        {
                            DataContext = vm
                        };
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

        private void StartTimer()
        {
            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        if (App.Current == null || App.Current.Dispatcher.HasShutdownStarted)
                            break; // Stop the loop if the app is shutting down

                        string currentTime = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");

                        await App.Current.Dispatcher.InvokeAsync(() =>
                        {
                            CurrentDateTime = currentTime;
                        });

                        await Task.Delay(1000); // Wait 1 second before next update
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions gracefully
                    Console.WriteLine($"Timer stopped: {ex.Message}");
                }

            });
        }

    }
}
