using DiveLogApplication.Core;
using DiveLogApplication.Models;
using System;
using System.Collections.Generic;

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
        private List<DiveEntry> _diveLogList;

        public DiveLogViewModel()
        {
            DiveLogList = new List<DiveEntry>
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

        public List<DiveEntry> DiveLogList
        {
            get => _diveLogList;
            set
            {
                _diveLogList = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand OpenDetailsCommand { get; set; }

        private void WireCommands()
        {
            OpenDetailsCommand = new RelayCommand(
                param =>
                {
                    if (param is DiveEntry selectedDiveEntry)
                    {

                    }
                },
                param => true);
        }
    }
}
