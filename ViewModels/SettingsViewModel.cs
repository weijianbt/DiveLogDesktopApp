using DiveLogApplication.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiveLogApplication.Utilities;
using System.IO;

namespace DiveLogApplication.ViewModels
{
	public class SettingsViewModel : ViewModel
    {
        private const string _general = "General";
        private readonly string _diveLogSettingsFile = "DiveLogSettings.ini";
        private readonly string _defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private readonly string _diveLogSettingsFilePath;
        private string _diveLicenseDirectory;
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

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand SelectDirectoryCommand { get; private set; }

        private void WireCommands()
        {
            SelectDirectoryCommand = new RelayCommand(
                param =>
                {
                    using (var folderPicker = new FolderBrowserDialog())
                    {
                        folderPicker.Description = "Select a folder";
                        folderPicker.ShowNewFolderButton = true;

                        if (folderPicker.ShowDialog() == DialogResult.OK)
                        {
                            DiveLicenseDirectory = folderPicker.SelectedPath;
                        }
                    };
                },
                param => true);

            SaveCommand = new RelayCommand(
                param =>
                {
                    _iniFile.Write(nameof(DiveLicenseDirectory), DiveLicenseDirectory, _general);
                },
                param => true);
        }

        private void PopulateExistingItems()
        {
            DiveLicenseDirectory = _iniFile.Read(nameof(DiveLicenseDirectory), _general);
        }
    }
}
