using DiveLogApplication.Properties;
using DiveLogApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;

namespace DiveLogApplication.Models
{
    public class DiveLogManager
    {

        private string _diveLogDirectory;
        private string _diveLogFileName;
        private XDocument _diveLogFile = new XDocument();
        private SettingsViewModel _settings;
        private ObservableCollection<DiveEntry> _diveLogList;

        public DiveLogManager()
        {
            _settings = new SettingsViewModel();
            _diveLogDirectory = Path.Combine(_settings.DiveLicenseDirectory, "DiveLogEntry.xml");
        }

        public ObservableCollection<DiveEntry> DiveLogList => _diveLogList;

        public bool WriteToFile(DiveEntry data)
        {
            if (!File.Exists(_diveLogDirectory))
            {
                CreateFile();
            }

            _diveLogFile = XDocument.Load(_diveLogDirectory);

            XElement result = _diveLogFile.Descendants("DiveLog").FirstOrDefault(d => (uint)d.Element("DiveLogIndex") == data.DiveLogIndex);

            if (result != null)
            {
                if(MessageBox.Show("Dive entry number already exists. Do you want to replace existing entry index number?" +
                    "\nSelect YES to replace existing entry. Select NO to key in index again", "Index exist", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    result.Element("DiveLogIndex")?.SetValue(data.DiveLogIndex);
                    result.Element("Location")?.SetValue(data.Location);
                    result.Element("DiveSite")?.SetValue(data.DiveSite);
                    result.Element("StartTime")?.SetValue(data.StartTime);
                    result.Element("EndTime")?.SetValue(data.EndTime);
                    result.Element("Duration")?.SetValue(data.Duration);
                    result.Element("MaxDepth")?.SetValue(data.MaxDepth);
                    result.Element("AverageDepth")?.SetValue(data.AverageDepth);
                }
                else
                {
                    return false;
                }

            }
            else
            {
                var newElement = new XElement("DiveLog",
                                new XElement("DiveLogIndex", data.DiveLogIndex),
                                new XElement("Location", data.Location),
                                new XElement("DiveSite", data.DiveSite),
                                new XElement("StartTime", data.StartTime),
                                new XElement("EndTime", data.EndTime),
                                new XElement("Duration", data.Duration),
                                new XElement("MaxDepth", data.MaxDepth),
                                new XElement("AverageDepth", data.AverageDepth));

                _diveLogFile.Root.Add(newElement);
            }

            _diveLogFile.Save(_diveLogDirectory);

            return true;
        }

        public void Delete(DiveEntry diveEntry)
        {

        }

        public void Load()
        {
            if (!File.Exists(_diveLogDirectory))
            {
                MessageBox.Show(@"Dive log directory do not exists. Directory: {_diveLogDirectory}");
                return;
            }

            _diveLogList = new ObservableCollection<DiveEntry>();
            _diveLogFile = XDocument.Load(_diveLogDirectory);

            var diveLogs = _diveLogFile.Root.Elements("DiveLog");

            if (!diveLogs.Any())
            {
                return;
            }


            foreach (var diveLog in diveLogs)
            {

                DiveEntry newDiveEntry = new DiveEntry();

                if (uint.TryParse(diveLog.Element(nameof(DiveEntry.DiveLogIndex))?.Value, out uint diveLogIndex))
                {
                    newDiveEntry.DiveLogIndex = diveLogIndex;
                }

                newDiveEntry.Location = diveLog.Element(nameof(DiveEntry.Location))?.Value;
                newDiveEntry.DiveSite = diveLog.Element(nameof(DiveEntry.DiveSite))?.Value;

                if (DateTime.TryParse(diveLog.Element(nameof(DiveEntry.StartTime))?.Value, out DateTime startTime))
                {
                    newDiveEntry.StartTime = startTime;
                }

                if(DateTime.TryParse(diveLog.Element(nameof(DiveEntry.EndTime))?.Value, out DateTime endTime))
                {
                    newDiveEntry.EndTime = endTime;
                }
                
                if(double.TryParse(diveLog.Element(nameof(DiveEntry.Duration))?.Value, out double duration))
                {
                    newDiveEntry.Duration = duration;                    
                }

                if(double.TryParse(diveLog.Element(nameof(DiveEntry.MaxDepth))?.Value, out double maxDepth))
                {
                    newDiveEntry.MaxDepth = maxDepth;                    
                }                
                
                if(double.TryParse(diveLog.Element(nameof(DiveEntry.AverageDepth))?.Value, out double averageDepth))
                {
                    newDiveEntry.AverageDepth = diveLogIndex;                    
                }

                _diveLogList.Add(newDiveEntry);
            }
        }

        private void CreateFile()
        {
            _diveLogFile = new XDocument(new XElement("DiveLogs"));
            _diveLogFile.Save(_diveLogDirectory);
        }

    }
}
