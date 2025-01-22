using DiveLogApplication.Properties;
using DiveLogApplication.ViewModels;
using System;
using System.Collections.Generic;
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

        public DiveLogManager()
        {
            _settings = new SettingsViewModel();
            _diveLogDirectory = Path.Combine(_settings.DiveLicenseDirectory, "DiveLogEntry.xml");
        }

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

        public void Load(DiveEntry diveEntry)
        {

        }

        private void CreateFile()
        {
            _diveLogFile = new XDocument(new XElement("DiveLogs"));
            _diveLogFile.Save(_diveLogDirectory);
        }

    }
}
