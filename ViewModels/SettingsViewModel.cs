using DiveLogApplication.Core;
using DiveLogApplication.Utilities;
using System;
using System.CodeDom;
using System.IO;
using System.Windows.Forms;

namespace DiveLogApplication.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        private const string _general = "General";
        private readonly string _diveLogSettingsFile = "DiveLogSettings.ini";
        private readonly string _defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private readonly string _diveLogSettingsFilePath;
        private string _diveLicenseDirectory;
        private string _diveLogDirectory;
        private readonly IniFile _iniFile;

        public SettingsViewModel()
        {
            // Default path for settings file is in user's document folder
            _diveLogSettingsFilePath = Path.Combine(_defaultDirectory, _diveLogSettingsFile);
            _iniFile = new IniFile(_diveLogSettingsFilePath);
            WireCommands();
            PopulateExistingItems();

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
                    _iniFile.Write(nameof(DiveLicenseDirectory), DiveLicenseDirectory, _general);
                    _iniFile.Write(nameof(DiveLogDirectory), DiveLogDirectory, _general);
                },
                param => true);
        }

        private void PopulateExistingItems()
        {
            DiveLicenseDirectory = _iniFile.Read(nameof(DiveLicenseDirectory), _general);
            DiveLogDirectory = _iniFile.Read(nameof(DiveLogDirectory), _general);
        }
    }
}
