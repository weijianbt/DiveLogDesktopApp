using DiveLogApplication.Core;
using DiveLogApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace DiveLogApplication.ViewModels
{
    public class AddNewDiveEntryViewModel : ViewModel
    {
        private readonly DiveEntry _diveEntry;
        private bool _isEditable;
        private bool _isNewEntry;

        private string _location;
        private string _diveSite;
        private DateTime _startTime;
        private DateTime _endTime;
        private double _duration;
        private double _maxDepth;
        private double _averageDepth;

        public AddNewDiveEntryViewModel() : this(null, true, ActionSource.DoubleClickFromList) { }

        public AddNewDiveEntryViewModel(DiveEntry diveEntry, bool isNewEntry = false, ActionSource actionSource = ActionSource.DoubleClickFromList)
        {
            _diveEntry = diveEntry ?? new DiveEntry();
            _isNewEntry = isNewEntry;
            IsEditable = !isNewEntry && (actionSource != ActionSource.DoubleClickFromList);

            WireCommands();

            if (!_isNewEntry) 
            { 
                PopulateExistingFields(_diveEntry); 
            }
        }

        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                _isEditable = value;
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
                param => true);

            SaveEntryCommand = new RelayCommand(
                param =>
                {
                    // todo
                },
                param => true);

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
