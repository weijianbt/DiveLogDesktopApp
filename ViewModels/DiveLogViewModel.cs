using DiveLogApplication.Core;
using DiveLogApplication.Models;
using DiveLogApplication.Views;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Reflection;
using System.Windows;

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
                        int index = DiveLogList.IndexOf(selectedDiveEntry);
                        var vm = new AddNewDiveEntryViewModel(selectedDiveEntry, isPopulatingFromExisting: true, actionSource: ActionSource.DoubleClickFromList);

                        var dialog = new AddNewDiveEntry
                        {
                            DataContext = vm
                        };

                        dialog.ShowDialog();
                        UpdateUI(vm.DiveEntry, index: index, isNewEntry: false);
                    }
                },
                param => param is DiveEntry);

            NewEntryCommand = new RelayCommand(
                param =>
                {
                    var vm = new AddNewDiveEntryViewModel(
                        new DiveEntry(), 
                        isPopulatingFromExisting: false, 
                        actionSource: ActionSource.ClickFromButtonCommand, 
                        isNewEntry: true);

                    var dialog = new AddNewDiveEntry
                    {
                        DataContext = vm
                    };

                    dialog.ShowDialog();

                    UpdateUI(vm.DiveEntry, isNewEntry: true);
                },
                param => true);

            EditEntryCommand = new RelayCommand(
                param =>
                {
                    if (SelectedDiveEntry != null)
                    {
                        int index = DiveLogList.IndexOf(SelectedDiveEntry);
                        var vm = new AddNewDiveEntryViewModel(SelectedDiveEntry, isPopulatingFromExisting: true, actionSource: ActionSource.ClickFromButtonCommand);

                        var dialog = new AddNewDiveEntry
                        {
                            DataContext = vm
                        };

                        dialog.ShowDialog();
                        UpdateUI(vm.DiveEntry, index: index, isNewEntry: false);
                    }
                },
                param => SelectedDiveEntry != null);

            DuplicateEntryCommand = new RelayCommand(
                param =>
                {
                    if (SelectedDiveEntry != null)
                    {
                        DiveEntry newDiveEntry = SelectedDiveEntry.Clone();
                        AddNewDiveEntryViewModel vm = new AddNewDiveEntryViewModel(newDiveEntry, isPopulatingFromExisting: true, actionSource: ActionSource.ClickFromButtonCommand);
                        var dialog = new AddNewDiveEntry
                        {
                            DataContext = vm
                        };
                        dialog.ShowDialog();
                        UpdateUI(vm.DiveEntry, isNewEntry: true);
                    }
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

        private void UpdateUI(DiveEntry diveEntry, int index = 0, bool isNewEntry = true)
        {
            if (!isNewEntry)
            {
                DiveLogList.RemoveAt(index);
                DiveLogList.Insert(index, diveEntry);
            }
            else
            {
                DiveLogList.Add(diveEntry);
            }
        }
    }
}
