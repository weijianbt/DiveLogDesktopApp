using DiveLogApplication.Core;
using DiveLogApplication.Models;
using DiveLogApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

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
        private DateTime _startDate = DateTime.Now;
        private DateTime _endDate = DateTime.Now;
        private double _duration;
        private double _maxDepth;
        private double _averageDepth;

        private readonly DiveLogManager _diveLogManager = new DiveLogManager();

        private DayOrNight _startingDayOrNight;
        private DayOrNight _endingDayOrNight;
        private int _startingHour = 1;
        private int _endingHour = 1;
        private int _startingMinute = 1;        
        private int _endingMinute = 1;
        private bool _hasValidationError = true;
        private bool _validStartAndEndTime;
        private string _processedStartDateTime;
        private string _processedEndDateTime;

        private List<string> _validationErrorMessage;

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

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }        
        
        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }

        public int StartingHour
        {
            get => _startingHour;
            set
            {
                _startingHour = value;
                OnPropertyChanged();
            }
        }

        public int StartingMinute
        {
            get => _startingMinute;
            set
            {
                _startingMinute = value;
                OnPropertyChanged();
            }
        }

        public DayOrNight StartingDayOrNight
        {
            get => _startingDayOrNight;
            set
            {
                _startingDayOrNight = value;
                OnPropertyChanged();
            }
        }

        public int EndingHour
        {
            get => _endingHour;
            set
            {
                _endingHour = value;
                OnPropertyChanged();
            }
        }

        public int EndingMinute
        {
            get => _endingMinute;
            set
            {
                _endingMinute = value;
                OnPropertyChanged();
            }
        }

        public DayOrNight EndingDayOrNight
        {
            get => _endingDayOrNight;
            set
            {
                _endingDayOrNight = value;
                OnPropertyChanged();
            }
        }

        public bool HasValidationError
        {
            get => _hasValidationError;
            set
            {
                _hasValidationError = value;
                OnPropertyChanged();
            }
        }

        public DiveEntry DiveEntry => _diveEntry;
        public DiveEntry NewDiveEntry { get; set; }
        public RelayCommand EditEntryCommand { get; private set; }
        public RelayCommand SaveEntryCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand HourIncreaseCommand { get; set; }
        public RelayCommand HourDecreaseCommand { get; set; }
        public RelayCommand MinuteIncreaseCommand { get; set; }
        public RelayCommand MinuteDecreaseCommand { get; set; }
        public RelayCommand OKCommand { get; set; }

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
                    CheckStartVsEndtime();

                    int indexToFind = (int)_diveEntry.DiveLogIndex;

                    // Populate the _diveEntry fields
                    _diveEntry.DiveLogIndex = DiveLogIndex;
                    _diveEntry.Location = Location;
                    _diveEntry.DiveSite = DiveSite;
                    _diveEntry.StartTime = _processedStartDateTime;
                    _diveEntry.EndTime = _processedEndDateTime;
                    _diveEntry.Duration = Duration;
                    _diveEntry.MaxDepth = MaxDepth;
                    _diveEntry.AverageDepth = AverageDepth;

                    _diveLogManager.IsEdit = _isNewEntry == false;
                    _diveLogManager.IsSameIndex = _originalDiveLogIndex == DiveLogIndex;

                    if (ValidateCanSave())
                    {
                        bool canClose = _diveLogManager.WriteToFile(DiveEntry, indexToFind);

                        if (param is Window window && canClose)
                        {
                            NewDiveEntry = _diveEntry;
                            window.DialogResult = true;
                            window.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Join("\n", _validationErrorMessage), "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                    
                },
                param => IsEditable && !HasValidationError);

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

            DateTime.TryParseExact(diveEntry.StartTime, "dd/MM/yyyy hh:mm tt", new CultureInfo("en-MY"), DateTimeStyles.None, out DateTime startDiveDateTime);
            StartDate = startDiveDateTime.Date;
            StartingDayOrNight = startDiveDateTime.Hour < 12 ? DayOrNight.AM : DayOrNight.PM;
            StartingHour = startDiveDateTime.Hour;
            StartingMinute = startDiveDateTime.Minute;

            if (StartingDayOrNight == DayOrNight.PM)
            {
                StartingHour -= 12;
            }

            DateTime.TryParseExact(diveEntry.EndTime, "dd/MM/yyyy hh:mm tt", new CultureInfo("en-MY"), DateTimeStyles.None, out DateTime endDiveDateTime);
            EndDate = endDiveDateTime;
            EndingDayOrNight = endDiveDateTime.Hour < 12 ? DayOrNight.AM : DayOrNight.PM;
            EndingHour = endDiveDateTime.Hour;
            EndingMinute = endDiveDateTime.Minute;

            if (EndingDayOrNight == DayOrNight.PM)
            {
                EndingHour -= 12;
            }
        }

        private bool ValidateCanSave()
        {
            _validationErrorMessage = new List<string>();

            if (DiveLogIndex <= 0)
                _validationErrorMessage.Add("Dive Log Index must be greater than zero.");

            if (string.IsNullOrEmpty(Location))
                _validationErrorMessage.Add("Location cannot be empty.");

            if (string.IsNullOrEmpty(DiveSite))
                _validationErrorMessage.Add("Dive Site cannot be empty.");

            if (Duration <= 0)
                _validationErrorMessage.Add("Duration must be greater than zero.");

            if (MaxDepth <= 0)
                _validationErrorMessage.Add("Max Depth must be greater than zero.");

            if (AverageDepth < 0)
                _validationErrorMessage.Add("Average Depth cannot be negative.");

            if (!_validStartAndEndTime)
                _validationErrorMessage.Add("Start time must be before end time.");

            // If there are errors, show them in a message box
            if (_validationErrorMessage.Count > 0)
            {
                return false;
            }

            return true;

        }

        private void CheckStartVsEndtime()
        {
            int processedStartingHour = StartingHour;
            int processedEndingHour = EndingHour;
            double previousDuration = Duration;

            if (StartingDayOrNight == DayOrNight.PM)
            {
                processedStartingHour += 12;
            }

            if (EndingDayOrNight == DayOrNight.PM)
            {
                processedEndingHour += 12;
            }

            DateTime startDateTime = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day, processedStartingHour, StartingMinute, 0);
            DateTime endDateTime = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, processedEndingHour, EndingMinute, 0);

            _processedStartDateTime = startDateTime.ToString("dd/MM/yyyy hh:mm tt");
            _processedEndDateTime = endDateTime.ToString("dd/MM/yyyy hh:mm tt");

            _validStartAndEndTime = startDateTime < endDateTime;
            
            if (_validStartAndEndTime)
            {
                Duration = endDateTime.Subtract(startDateTime).TotalMinutes;
            }
            else
            {
                Duration = previousDuration;
            }
        }
    }
}
