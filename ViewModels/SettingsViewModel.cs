using DiveLogApplication.Core;
using DiveLogApplication.Utilities;
using System;
using System.IO;
using System.Windows.Forms;

namespace DiveLogApplication.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        private const string _general = "General";
        private const string _diveLogFileName = "DiveLog.xml";
        private const string _diveLicenseFileName = "DiveLicense.xml";
        private const string _diveLogSettingsFile = "DiveLogSettings.ini";
        private readonly string _defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private string _diveLogSettingsFilePath;
        private string _diveLicenseDirectory;
        private string _diveLogDirectory;
        private readonly IniFile _iniFile;

        private string _previousDiveLicenseDirectory;
        private string _previousDiveLogDirectory;
        private string _previousUsername;
        private string _username;

        public SettingsViewModel()
        {
            // Default path for settings file is in user's document folder
            DiveLogSettingsFilePath = Path.Combine(_defaultDirectory, _diveLogSettingsFile);
            _iniFile = new IniFile(_diveLogSettingsFilePath);
            LoadedCommand();
            WireCommands();
        }

        public string DiveLogFullFilePath { get; set; }

        public string DiveLicenseFullFilePath { get; set; }

        public string ProfilePicturePath { get; set; }

        public string DiveLogSettingsFilePath
        {
            get { return _diveLogSettingsFilePath; }
            set { _diveLogSettingsFilePath = value; }
        }

        public string DiveLicenseDirectory
        {
            get => _diveLicenseDirectory;
            set
            {
                _diveLicenseDirectory = value;
                OnPropertyChanged();
            }
        }

        public string DiveLogDirectory
        {
            get => _diveLogDirectory;
            set
            {
                _diveLogDirectory = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand SelectDirectoryCommand { get; private set; }

        private void WireCommands()
        {
            SelectDirectoryCommand = new RelayCommand(
                param =>
                {
                    if (param is string propertyName)
                    {
                        using (var folderPicker = new FolderBrowserDialog())
                        {
                            folderPicker.Description = "Select a folder";
                            folderPicker.ShowNewFolderButton = true;

                            if (folderPicker.ShowDialog() == DialogResult.OK)
                            {
                                var property = GetType().GetProperty(propertyName);
                                property.SetValue(this, folderPicker.SelectedPath);
                            }
                        };
                    }
                },
                param => true);

            SaveCommand = new RelayCommand(
                param =>
                {
                    if (_previousDiveLogDirectory != DiveLogDirectory)
                    {
                        _iniFile.MoveFile(Path.Combine(_previousDiveLogDirectory, _diveLogFileName), Path.Combine(DiveLogDirectory, _diveLogFileName));
                    }

                    if (_previousDiveLicenseDirectory != DiveLicenseDirectory)
                    {
                        _iniFile.MoveFile(Path.Combine(_previousDiveLicenseDirectory, _diveLicenseFileName), Path.Combine(DiveLicenseDirectory, _diveLicenseFileName));
                    }

                    _iniFile.Write(nameof(Username), Username, _general);
                    _iniFile.Write(nameof(DiveLicenseDirectory), DiveLicenseDirectory, _general);
                    _iniFile.Write(nameof(DiveLogDirectory), DiveLogDirectory, _general);
                },
                param => true);
        }

        private void LoadedCommand()
        {
            PopulateExistingItems();
            RecordPreviousValues();
            GenerateFullFilePath();
        }

        private void GenerateFullFilePath()
        {
            DiveLicenseFullFilePath = Path.Combine(DiveLicenseDirectory, _diveLicenseFileName);
            DiveLogFullFilePath = Path.Combine(DiveLogDirectory, _diveLogFileName);
        }

        private void PopulateExistingItems()
        {
            DiveLicenseDirectory = _iniFile.Read(nameof(DiveLicenseDirectory), _general);
            DiveLogDirectory = _iniFile.Read(nameof(DiveLogDirectory), _general);
            Username = _iniFile.Read(nameof(Username), _general);
        }

        private void RecordPreviousValues()
        {
            _previousDiveLicenseDirectory = DiveLicenseDirectory;
            _previousDiveLogDirectory = DiveLogDirectory;
            _previousUsername = Username;
        }
    }
}
