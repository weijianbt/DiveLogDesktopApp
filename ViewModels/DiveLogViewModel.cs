﻿using DiveLogApplication.Core;
using DiveLogApplication.Models;
using DiveLogApplication.Utilities;
using DiveLogApplication.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace DiveLogApplication.ViewModels
{
    public class DiveLogViewModel : ViewModel
    {
        private uint _diveLogIndex;
        private string _diveSite;
        private string _location;
        private DateTime _startTime;
        private DateTime _endTime;
        private double _duration;
        private double _maxDepth;
        private double _averageDepth;
        private ObservableCollection<DiveEntry> _diveLogList;
        private DiveEntry _selectedDiveEntry;
        private List<uint> _diveLogIndexList = new List<uint>();
        private readonly DiveLogAppData _diveLogAppData;

        public DiveLogViewModel()
        {
        }

        public DiveLogViewModel(DiveLogAppData diveLogAppData)
        {
            _diveLogAppData = diveLogAppData;

            WireCommands();
            LoadedCommand.Execute(null);
        }

        public uint DiveLogIndex
        {
            get => _diveLogIndex;
            set
            {
                _diveLogIndex = value;
                OnPropertyChanged();
            }
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

        public RelayCommand LoadedCommand { get; set; }
        public RelayCommand ViewEntryCommand { get; set; }
        public RelayCommand NewEntryCommand { get; set; }
        public RelayCommand EditEntryCommand { get; set; }
        public RelayCommand DuplicateEntryCommand { get; set; }
        public RelayCommand DeleteEntryCommand { get; set; }

        private void WireCommands()
        {
            LoadedCommand = new RelayCommand(
                param =>
                {
                    DiveLogList = _diveLogAppData.DiveLogList;
                },
                param => true);

            ViewEntryCommand = new RelayCommand(
                param =>
                {
                    if (param is DiveEntry selectedDiveEntry)
                    {
                        int index = _diveLogAppData.DiveLogList.IndexOf(selectedDiveEntry);
                        var vm = new AddNewDiveEntryViewModel(selectedDiveEntry, isPopulatingFromExisting: true, actionSource: ActionSource.DoubleClickFromList);

                        var dialog = new AddNewDiveEntry
                        {
                            DataContext = vm
                        };

                        var result = DialogHelper.ShowCenteredDialog(Application.Current.MainWindow, dialog);

                        if (result == true)
                        {
                            _diveLogAppData.AddDiveLog(vm.NewDiveEntry, listIndex: index, isEdit: true);
                        }
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
                        DataContext = vm,
                    };

                    var result = DialogHelper.ShowCenteredDialog(Application.Current.MainWindow, dialog);

                    if (result == true)
                    {
                        _diveLogAppData.AddDiveLog(vm.NewDiveEntry, isEdit: false);
                    }

                },
                param => true);

            EditEntryCommand = new RelayCommand(
                param =>
                {
                    if (SelectedDiveEntry != null)
                    {
                        int listIndex = _diveLogAppData.DiveLogList.IndexOf(SelectedDiveEntry);
                        var vm = new AddNewDiveEntryViewModel(SelectedDiveEntry, isPopulatingFromExisting: true, actionSource: ActionSource.ClickFromButtonCommand);

                        var dialog = new AddNewDiveEntry
                        {
                            DataContext = vm
                        };

                        var result = DialogHelper.ShowCenteredDialog(Application.Current.MainWindow, dialog);

                        if (result == true)
                        {
                            _diveLogAppData.AddDiveLog(vm.NewDiveEntry, listIndex: listIndex, isEdit: true);
                        }
                    }
                },
                param => SelectedDiveEntry != null);

            DuplicateEntryCommand = new RelayCommand(
                param =>
                {
                    if (SelectedDiveEntry != null)
                    {
                        DiveEntry newDiveEntry = SelectedDiveEntry.Clone();
                        AddNewDiveEntryViewModel vm = new AddNewDiveEntryViewModel(newDiveEntry, isPopulatingFromExisting: true, actionSource: ActionSource.ClickFromButtonCommand, isNewEntry: true);
                        var dialog = new AddNewDiveEntry
                        {
                            DataContext = vm
                        };

                        var result = DialogHelper.ShowCenteredDialog(Application.Current.MainWindow, dialog);

                        if (result == true)
                        {
                            _diveLogAppData.AddDiveLog(vm.NewDiveEntry, isEdit: false);
                        }
                    }
                },
                param => SelectedDiveEntry != null);

            DeleteEntryCommand = new RelayCommand(
                param =>
                {
                    if (MessageBox.Show(
                        "Delete dive log?\nThis is not reversible.",
                        "Delete dive log",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning) == MessageBoxResult.OK)
                    {
                        _diveLogAppData.DeleteDiveLog(SelectedDiveEntry);
                    }
                },
                param => SelectedDiveEntry != null);
        }
    }
}