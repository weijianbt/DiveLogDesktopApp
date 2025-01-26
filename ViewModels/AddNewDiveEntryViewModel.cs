using DiveLogApplication.Core;
using DiveLogApplication.Models;
using System;
using System.Windows;

namespace DiveLogApplication.ViewModels
{
    public class AddNewDiveEntryViewModel : ViewModel
    {
        private readonly DiveEntry _diveEntry;
        private readonly bool _isNewEntry;
        private readonly uint _originalDiveLogIndex;
        
        private bool _isEditable;
        private uint _diveLogIndex;
        private string _location;
        private string _diveSite;
        private DateTime _startTime;
        private DateTime _endTime;
        private double _duration;
        private double _maxDepth;
        private double _averageDepth;

        private readonly DiveLogManager _diveLogManager = new DiveLogManager();

        public AddNewDiveEntryViewModel() : this(null, true, ActionSource.DoubleClickFromList) { }

        public AddNewDiveEntryViewModel(DiveEntry diveEntry, bool isPopulatingFromExisting = true, ActionSource actionSource = ActionSource.DoubleClickFromList, bool isNewEntry = false)
        {
            _diveEntry = diveEntry ?? new DiveEntry();
            _isNewEntry = isNewEntry;
            IsEditable = (isPopulatingFromExisting && (actionSource != ActionSource.DoubleClickFromList)) || _isNewEntry;
            _originalDiveLogIndex = _diveEntry.DiveLogIndex;
            EntryType = _isNewEntry ? "Add New Entry" : "Edit Entry";
            WireCommands();

            if (isPopulatingFromExisting)
            {
                PopulateExistingFields(_diveEntry);
            }
        }

        public string EntryType { get; set; }

        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                _isEditable = value;
                OnPropertyChanged();
            }
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

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
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

        public DiveEntry DiveEntry => _diveEntry;
        public RelayCommand EditEntryCommand { get; private set; }
        public RelayCommand SaveEntryCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        private void WireCommands()
        {
            EditEntryCommand = new RelayCommand(
                param =>
                {
                    IsEditable = true;
                },
                param => !_isNewEntry);

            SaveEntryCommand = new RelayCommand(
                param =>
                {
                    // Populate the _diveEntry fields
                    _diveEntry.DiveLogIndex = DiveLogIndex;
                    _diveEntry.Location = Location;
                    _diveEntry.DiveSite = DiveSite;
                    _diveEntry.StartTime = StartTime;
                    _diveEntry.EndTime = EndTime;
                    _diveEntry.Duration = Duration;
                    _diveEntry.MaxDepth = MaxDepth;
                    _diveEntry.AverageDepth = AverageDepth;

                    _diveLogManager.IsEdit = _isNewEntry == false;
                    _diveLogManager.IsSameIndex = _originalDiveLogIndex == DiveLogIndex;
                    bool canClose = _diveLogManager.WriteToFile(DiveEntry);

                    if (param is Window window && canClose)
                    {
                        window.DialogResult = true;
                        window.Close();
                    }
                },
                param => IsEditable);

            CancelCommand = new RelayCommand(
                param =>
                {
                    if (param is Window window)
                    {
                        window.Close();
                    }
                },
                param => true);

        }

        private void PopulateExistingFields(DiveEntry diveEntry)
        {
            DiveLogIndex = diveEntry.DiveLogIndex;
            DiveSite = diveEntry.DiveSite;
            Location = diveEntry.Location;
            Duration = diveEntry.Duration;
            MaxDepth = diveEntry.MaxDepth;
            AverageDepth = diveEntry.AverageDepth;
            StartTime = diveEntry.StartTime;
            EndTime = diveEntry.EndTime;
        }
    }
}
