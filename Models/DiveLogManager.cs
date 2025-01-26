using DiveLogApplication.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
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
            _diveLogDirectory = _settings.DiveLogFullFilePath;
        }

        public ObservableCollection<DiveEntry> DiveLogList => _diveLogList;

        public bool IsEdit { get; set; }

        public bool IsSameIndex { get; set; }

        public bool WriteToFile(DiveEntry data)
        {
            if (!File.Exists(_diveLogDirectory))
            {
                CreateFile();
            }

            _diveLogFile = XDocument.Load(_diveLogDirectory);

            XElement result = null;
            if ((IsEdit && !IsSameIndex) || !IsEdit)
            {
                result = _diveLogFile.Descendants("DiveLog").FirstOrDefault(d => (uint)d.Element("DiveLogIndex") == data.DiveLogIndex);
            }

            if (result != null)
            {
                MessageBox.Show("Dive entry number already exists. Please key in another index."
                    , "Index exist", MessageBoxButton.OK);

                return false;
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

            _diveLogFile = SortElements(_diveLogFile);
            _diveLogFile.Save(_diveLogDirectory);

            return true;
        }

        public void Delete(DiveEntry diveEntry)
        {
            if (!File.Exists(_diveLogDirectory))
            {
                return;
            }

            _diveLogList = new ObservableCollection<DiveEntry>();
            _diveLogFile = XDocument.Load(_diveLogDirectory);

            var diveLogs = _diveLogFile.Root.Elements("DiveLog");

            if (!diveLogs.Any())
            {
                return;
            }

            string idToFind = diveEntry.DiveLogIndex.ToString();

            _diveLogFile.Descendants("DiveLog").FirstOrDefault(d => (string)d.Element("DiveLogIndex") == idToFind).Remove();
            _diveLogFile.Save(_diveLogDirectory);

        }

        public void Load()
        {
            if (!File.Exists(_diveLogDirectory))
            {
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

                if (DateTime.TryParse(diveLog.Element(nameof(DiveEntry.EndTime))?.Value, out DateTime endTime))
                {
                    newDiveEntry.EndTime = endTime;
                }

                if (double.TryParse(diveLog.Element(nameof(DiveEntry.Duration))?.Value, out double duration))
                {
                    newDiveEntry.Duration = duration;
                }

                if (double.TryParse(diveLog.Element(nameof(DiveEntry.MaxDepth))?.Value, out double maxDepth))
                {
                    newDiveEntry.MaxDepth = maxDepth;
                }

                if (double.TryParse(diveLog.Element(nameof(DiveEntry.AverageDepth))?.Value, out double averageDepth))
                {
                    newDiveEntry.AverageDepth = diveLogIndex;
                }

                _diveLogList.Add(newDiveEntry);
            }

            _diveLogList.OrderByDescending(p => p.DiveLogIndex);
        }

        private void CreateFile()
        {
            _diveLogFile = new XDocument(new XElement("DiveLogs"));
            _diveLogFile.Save(_diveLogDirectory);
        }

        private XDocument SortElements(XDocument doc)
        {
            var sortedElements = doc.Root.Elements("DiveLog")
                                            .OrderByDescending(e => (int)e.Element("DiveLogIndex"))
                                            .ToList();

            doc.Root.RemoveNodes();
            doc.Root.Add(sortedElements);

            return doc;
        }
    }
}
