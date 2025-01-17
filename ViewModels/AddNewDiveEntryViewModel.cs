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

        public AddNewDiveEntryViewModel() : this(null) { }

        public AddNewDiveEntryViewModel(DiveEntry diveEntry)
        {
            _diveEntry = diveEntry ?? new DiveEntry();
            IsEditable = false;
            WireCommands();
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
            get => _diveEntry.Location;
            set
            {
                _diveEntry.Location = value;
                OnPropertyChanged();
            }
        }

        public string DiveSite
        {
            get => _diveEntry.DiveSite;
            set
            {
                _diveEntry.DiveSite = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartTime
        {
            get => _diveEntry.StartTime;
            set
            {
                _diveEntry.StartTime = value;
                OnPropertyChanged();
            }
        }

        public DateTime EndTime
        {
            get => _diveEntry.EndTime;
            set
            {
                _diveEntry.EndTime = value;
                OnPropertyChanged();
            }
        }

        public double Duration
        {
            get => _diveEntry.Duration;
            set
            {
                _diveEntry.Duration = value;
                OnPropertyChanged();
            }
        }

        public double MaxDepth
        {
            get => _diveEntry.MaxDepth;
            set
            {
                _diveEntry.MaxDepth = value;
                OnPropertyChanged();
            }
        }

        public double AverageDepth
        {
            get => _diveEntry.AverageDepth;
            set
            {
                _diveEntry.AverageDepth = value;
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
    }
}
