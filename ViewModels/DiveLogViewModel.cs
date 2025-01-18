using DiveLogApplication.Core;
using DiveLogApplication.Models;
using DiveLogApplication.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
//using System.Windows.Forms;

namespace DiveLogApplication.ViewModels
{
    public class DiveLogViewModel : ViewModel
    {
        private string _diveSite;
        private string _location;
        private DateTime _startTime;
        private DateTime _endTime;
        private double _duration;
        private double _maxDepth;
        private double _averageDepth;
        private ObservableCollection<DiveEntry> _diveLogList;
        private DiveEntry _selectedDiveEntry;

        public DiveLogViewModel()
        {
            DiveLogList = new ObservableCollection<DiveEntry>()
            {
                new DiveEntry()
                {
                    DiveSite = "Tioman",
                    Location = "Tioman Mersing",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Duration = 12,
                    MaxDepth = 11.1,
                    AverageDepth = 9
                },

                new DiveEntry()
                {
                    DiveSite = "Tenggol",
                    Location = "Terrengganu",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    Duration = 45,
                    MaxDepth = 18,
                    AverageDepth = 10
                }
            };

            WireCommands();

        }

        public string DiveSite
        {
            get => _diveSite;
            set
            {
                _diveSite = value;
                OnPropertyChanged();
            }
        }

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged();
            }
        }

        public DateTime EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                OnPropertyChanged();
            }
        }

        public double Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                OnPropertyChanged();
            }
        }

        public double MaxDepth
        {
            get => _maxDepth;
            set
            {
                _maxDepth = value;
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

        public ObservableCollection<DiveEntry> DiveLogList
        {
            get => _diveLogList;
            set
            {
                _diveLogList = value;
                OnPropertyChanged();
            }
        }

        public DiveEntry SelectedDiveEntry
        {
            get => _selectedDiveEntry;
            set
            {
                _selectedDiveEntry = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand ViewEntryCommand { get; set; }
        public RelayCommand NewEntryCommand { get; set; }
        public RelayCommand EditEntryCommand { get; set; }
        public RelayCommand DuplicateEntryCommand { get; set; }
        public RelayCommand DeleteEntryCommand { get; set; }

        private void WireCommands()
        {
            ViewEntryCommand = new RelayCommand(
                param =>
                {
                    if (param is DiveEntry selectedDiveEntry)
                    {
                        var vm = new AddNewDiveEntryViewModel(selectedDiveEntry, isNewEntry:false, actionSource: ActionSource.DoubleClickFromList);

                        var dialog = new AddNewDiveEntry
                        {
                            DataContext = vm
                        };

                        dialog.ShowDialog();
                    }
                },
                param => param is DiveEntry);

            NewEntryCommand = new RelayCommand(
                param =>
                {
                    var vm = new AddNewDiveEntryViewModel(new DiveEntry(), isNewEntry:true, actionSource: ActionSource.ClickFromButtonCommand);
                    var dialog = new AddNewDiveEntry
                    {
                        DataContext = vm
                    };
                    dialog.ShowDialog();
                },
                param => true);

            EditEntryCommand = new RelayCommand(
                param =>
                {
                    if (SelectedDiveEntry != null)
                    {
                        var vm = new AddNewDiveEntryViewModel(SelectedDiveEntry, isNewEntry: false, actionSource: ActionSource.ClickFromButtonCommand);

                        var dialog = new AddNewDiveEntry
                        {
                            DataContext = vm
                        };

                        dialog.ShowDialog();
                    }
                },
                param => SelectedDiveEntry != null);

            DuplicateEntryCommand = new RelayCommand(
                param =>
                {

                },
                param => SelectedDiveEntry != null);

            DeleteEntryCommand = new RelayCommand(
                param =>
                {
                    if (MessageBox.Show(
                        "Are you sure you want to delete the dive log entry?", 
                        "Delete dive log", 
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        DiveLogList.Remove(SelectedDiveEntry);
                    }
                },
                param => SelectedDiveEntry != null);
        }
    }
}
