using DiveLogApplication.Core;
using DiveLogApplication.Models;
using DiveLogApplication.Utilities;
using DiveLogApplication.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace DiveLogApplication.ViewModels
{
    public class UserProfileViewModel : ViewModel
    {
        private string _profilePicturePath;
        private ObservableCollection<DiveLicense> _diveLicenseList;
        private DiveLicense _selectedDiveLicense;
        private readonly DiveLogAppData _diveLogAppData;

        public UserProfileViewModel()
        {

        }

        public UserProfileViewModel(DiveLogAppData diveLogAppData)
        {
            _diveLogAppData = diveLogAppData;
            WireCommands();
            LoadData();
        }

        private void LoadData()
        {
            DiveLicenseList = _diveLogAppData.DiveLicenseList;
            ProfilePicturePath = _diveLogAppData.ProfilePicturePath;
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
                    DiveLicenseList = _diveLogAppData.DiveLicenseList;
                    ProfilePicturePath = _diveLogAppData.ProfilePicturePath;
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
                        _diveLogAppData.IniFile.Write(nameof(ProfilePicturePath), ProfilePicturePath, "General");
                    }
                },
                param => true);

            AddNewLicenseCommand = new RelayCommand(
                param =>
                {
                    var dialog = new AddNewLicenseView();
                    var vm = new AddNewLicenseViewModel();
                    vm.EntryType = "Add New License";
                    dialog.DataContext = vm;

                    var result = DialogHelper.ShowCenteredDialog(Application.Current.MainWindow, dialog);

                    if (result == true)
                    {
                        var newLicense = vm.NewDiveLicense;
                        _diveLogAppData.AddLicense(newLicense);
                        LoadLicenseCommand.Execute(null);
                    }
                },
                param => true);

            DeleteLicenseCommand = new RelayCommand(
                param =>
                {
                    if (_selectedDiveLicense != null)
                    {
                        var isDelete = MessageBox.Show("Delete license?\nThis is not reversible.", "Delete License", MessageBoxButton.OKCancel);
                        if (isDelete == MessageBoxResult.OK)
                        {
                            _diveLogAppData.DeleteLicense(_selectedDiveLicense);
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
                        var dialog = new AddNewLicenseView();
                        var vm = new AddNewLicenseViewModel(_selectedDiveLicense, true);
                        vm.EntryType = "Edit License";
                        dialog.DataContext = vm;

                        var result = DialogHelper.ShowCenteredDialog(Application.Current.MainWindow, dialog);

                        if (result == true)
                        {
                            var newLicense = vm.NewDiveLicense;
                            var licenseListIndex = _diveLogAppData.DiveLicenseList.IndexOf(_selectedDiveLicense);

                            _diveLogAppData.AddLicense(newLicense, listIndex: licenseListIndex, isEdit: true);
                            LoadLicenseCommand.Execute(null);
                        }
                    }
                },
                param => _selectedDiveLicense != null);
        }
    }
}
