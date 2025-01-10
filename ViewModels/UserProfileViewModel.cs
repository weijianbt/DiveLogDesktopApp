using DiveLogApplication.Core;
using DiveLogApplication.Models;
using DiveLogApplication.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace DiveLogApplication.ViewModels
{
    public class UserProfileViewModel : ViewModel
    {
        private readonly DiveLicenseManager _diveLicenseManager;

        private string _profilePicturePath;
        private ObservableCollection<DiveLicense> _diveLicenseList;
        private DiveLicense _selectedDiveLicense;
        private SettingsViewModel _settings;

        public UserProfileViewModel()
        {
            _settings = new SettingsViewModel();
            _diveLicenseManager = new DiveLicenseManager();
            WireCommands();

            LoadLicenseCommand.Execute(null);
        }

        public string ProfilePicturePath
        {
            get => _profilePicturePath;
            set
            {
                _profilePicturePath = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<DiveLicense> DiveLicenseList
        {
            get => _diveLicenseList;
            set
            {
                _diveLicenseList = value;
                OnPropertyChanged();
            }
        }

        public DiveLicense SelectedDiveLicense
        {
            get => _selectedDiveLicense;
            set
            {
                _selectedDiveLicense = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SelectProfilePictureCommand { get; private set; }
        public RelayCommand AddNewLicenseCommand { get; private set; }
        public RelayCommand DeleteLicenseCommand { get; private set; }
        public RelayCommand EditLicenseCommand { get; private set; }
        public RelayCommand LoadLicenseCommand { get; private set; }

        private void WireCommands()
        {
            LoadLicenseCommand = new RelayCommand(
                param =>
                {
                    DiveLicenseList = _diveLicenseManager.LoadData();

                },
                param => true
                );

            SelectProfilePictureCommand = new RelayCommand(
                param =>
                {
                    var fileDialog = new Microsoft.Win32.OpenFileDialog
                    {
                        Filter = "Image files (*.jpg;*.jpeg;*.png;*.bmp;) | *.jpg;*.jpeg;*.png;*.bmp;"
                    };
                    bool? result = fileDialog.ShowDialog();

                    if (result == true)
                    {
                        ProfilePicturePath = fileDialog.FileName;
                    }
                },
                param => true);

            AddNewLicenseCommand = new RelayCommand(
                param =>
                {
                    var dialog = new AddNewLicenseView();
                    dialog.ShowDialog();

                    LoadLicenseCommand.Execute(null);
                },
                param => true);

            DeleteLicenseCommand = new RelayCommand(
                param =>
                {
                    if (_selectedDiveLicense != null)
                    {
                        var isDelete = MessageBox.Show("Are you sure you want to delete license?", "Delete License", MessageBoxButton.OKCancel);
                        if (isDelete == MessageBoxResult.OK)
                        {
                            _diveLicenseManager.DeleteFromFile(_selectedDiveLicense);
                            LoadLicenseCommand.Execute(null);
                        }
                    }
                },
                param => _selectedDiveLicense != null);

            EditLicenseCommand = new RelayCommand(
            param =>
            {
                if (_selectedDiveLicense != null)
                {
                    var dialog = new AddNewLicenseView
                    {
                        DataContext = new AddNewLicenseViewModel(_selectedDiveLicense, true)
                    };
                    dialog.ShowDialog();

                    LoadLicenseCommand.Execute(null);
                }
            },
            param => _selectedDiveLicense != null);
        }
    }
}
