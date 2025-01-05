using DiveLogApplication.Core;
using DiveLogApplication.Models;
using DiveLogApplication.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;

namespace DiveLogApplication.ViewModels
{
    public class AddNewLicenseViewModel : ViewModel
    {
        private readonly bool _isEdit;
        private readonly DiveLicenseManager _diveLicenseManager;
        private readonly DiveLicense _diveLicense;

        private string _licenseLevel;
        private string _licenseProvider;
        private string _diveCentre;
        private string _location;
        private string _id;
        private bool _canSave;
        private DateTime _issuedDate;
        private Guid _uniqueId;

        public AddNewLicenseViewModel()
        {
            _diveLicenseManager = new DiveLicenseManager();
            IssuedDate = DateTime.Now;
            WireCommands();
        }

        public AddNewLicenseViewModel(DiveLicense diveLicense, bool isEdit)
        {
            _diveLicense = diveLicense;
            _isEdit = isEdit;
            _diveLicenseManager = new DiveLicenseManager();
            WireCommands();
            PopulateExistingItems(_diveLicense);
        }

        public string LicenseLevel
        {
            get => _licenseLevel;
            set
            {
                _licenseLevel = value;
                OnPropertyChanged();
                ValidateCanSave();
            }
        }

        public string LicenseProvider
        {
            get => _licenseProvider;
            set
            {
                _licenseProvider = value;
                OnPropertyChanged();
                ValidateCanSave();
            }
        }

        public string DiveCentre
        {
            get => _diveCentre;
            set
            {
                _diveCentre = value;
                OnPropertyChanged();
                ValidateCanSave();
            }
        }

        public string Location
        {
            get => _location;
            set
            {
                _location = value;
                OnPropertyChanged();
                ValidateCanSave();
            }
        }

        public DateTime IssuedDate
        {
            get => _issuedDate;
            set
            {
                _issuedDate = value;
                OnPropertyChanged();
                ValidateCanSave();
            }
        }

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
                ValidateCanSave();
            }
        }

        public Guid UniqueId
        {
            get => _uniqueId;
            set
            {
                _uniqueId = value;
                OnPropertyChanged();
            }
        }

        public bool CanSave
        {
            get => _canSave;
            set
            {
                _canSave = value;
                OnPropertyChanged();
            }
        }

        public DiveLicenseManager DiveLicenseManager { get; private set; }
        public RelayCommand LoadedCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand ClearCommand { get; set; }

        private void WireCommands()
        {
            LoadedCommand = new RelayCommand(
                param =>
                {
                    LicenseLevel = _diveLicense.LicenseLevel;
                    LicenseProvider = _diveLicense.LicenseProvider;
                    DiveCentre = _diveLicense.DiveCentre;
                    Location = _diveLicense.Location;
                    Id = _diveLicense.Id;
                    IssuedDate = _diveLicense.IssuedDate;
                },
                param => true);

            SaveCommand = new RelayCommand(
                param =>
                {
                    DiveLicense newLicenseData = new DiveLicense
                    {
                        UniqueId = _isEdit ? _diveLicense.UniqueId : GenerateRandomID.Generate(),
                        LicenseLevel = LicenseLevel,
                        LicenseProvider = LicenseProvider,
                        DiveCentre = DiveCentre,
                        Location = Location,
                        IssuedDate = IssuedDate,
                        Id = Id
                    };

                    _diveLicenseManager.WriteToFile(newLicenseData);

                    if (param is Window window)
                    {
                        window.Close();
                    }

                },
                param => CanSave);

            CancelCommand = new RelayCommand(
                param =>
                {
                    if (param is Window window)
                    {
                        window.Close();
                    }
                },
                param => true);

            ClearCommand = new RelayCommand(
                param =>
                {
                    LicenseLevel = string.Empty;
                    LicenseProvider = string.Empty;
                    DiveCentre = string.Empty;
                    Location = string.Empty;
                    IssuedDate = DateTime.Now;
                    Id = string.Empty;

                    CanSave = false;
                },
                param => true);
        }

        private void ValidateCanSave()
        {
            var validations = new List<Func<bool>>
            {
                () => !string.IsNullOrEmpty(LicenseLevel),
                () => !string.IsNullOrEmpty(LicenseProvider),
                () => !string.IsNullOrEmpty(DiveCentre),
                () => !string.IsNullOrEmpty(Location),
                () => !string.IsNullOrEmpty(Id)
            };

            CanSave = validations.TrueForAll(validate => validate());

        }

        private void PopulateExistingItems(DiveLicense diveLicese)
        {
            LicenseLevel = diveLicese.LicenseLevel;
            LicenseProvider = diveLicese.LicenseProvider;
            DiveCentre = diveLicese.DiveCentre;
            Location = diveLicese.Location;
            Id = diveLicese.Id;
            IssuedDate = diveLicese.IssuedDate;
        }
    }
}
