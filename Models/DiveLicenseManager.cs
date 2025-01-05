using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace DiveLogApplication.Models
{
    public class DiveLicenseManager
    {
        private readonly string _licenseDirectory;

        private bool _retriedWrite;
        private bool _retriedLoad;
        private ObservableCollection<DiveLicense> _diveLicenseList;
        private XDocument _diveLicenseFile = new XDocument();

        public DiveLicenseManager()
        {
            _licenseDirectory = @"C:\Users\twjbr\Documents\DiveLogs\DiveLicense.xml";
        }

        public void WriteToFile(DiveLicense data)
        {
            if (!File.Exists(_licenseDirectory))
            {
                CreateFile();
            }

            try
            {
                _diveLicenseFile = XDocument.Load(_licenseDirectory);

                string idToFind = data.UniqueId.ToString();

                XElement result = _diveLicenseFile.Descendants("DiveLicense").FirstOrDefault(d => (string)d.Element("UniqueId") == idToFind);

                if (result != null)
                {
                    result.Element("LicenseLevel")?.SetValue(data.LicenseLevel);
                    result.Element("LicenseProvider")?.SetValue(data.LicenseProvider);
                    result.Element("DiveCentre")?.SetValue(data.DiveCentre);
                    result.Element("Location")?.SetValue(data.Location);
                    result.Element("IssuedDate")?.SetValue(data.IssuedDate.ToString("dd/MM/yyyy"));
                    result.Element("Id")?.SetValue(data.Id);
                }
                else
                {
                    var newElement = new XElement("DiveLicense",
                        new XElement("UniqueId", data.UniqueId),
                        new XElement("LicenseLevel", data.LicenseLevel),
                        new XElement("LicenseProvider", data.LicenseProvider),
                        new XElement("DiveCentre", data.DiveCentre),
                        new XElement("Location", data.Location),
                        new XElement("IssuedDate", data.IssuedDate.ToString("dd/MM/yyyy")),
                        new XElement("Id", data.Id));

                    _diveLicenseFile.Root.Add(newElement);
                }

                _diveLicenseFile.Save(_licenseDirectory);

            }
            catch (XmlException e)
            {
                if (!_retriedWrite)
                {
                    CreateFile();
                    WriteToFile(data);
                    _retriedWrite = true;
                }

                MessageBox.Show($"Unable to write to dive license file.\n{e.Message}");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occured when trying to save dive license.\n{ex.Message}");
            }
        }

        public ObservableCollection<DiveLicense> LoadData()
        {
            try
            {
                XDocument doc = XDocument.Load(_licenseDirectory);

                var licenses = doc.Root.Elements("DiveLicense");
                _diveLicenseList = new ObservableCollection<DiveLicense>();

                foreach (var license in licenses)
                {
                    DateTime.TryParse(license.Element(nameof(DiveLicense.IssuedDate))?.Value, out DateTime issuedDate);
                    Guid.TryParse(license.Element(nameof(DiveLicense.UniqueId))?.Value, out Guid uniqueId);

                    DiveLicense newLicense = new DiveLicense()
                    {
                        UniqueId = uniqueId,
                        IssuedDate = issuedDate,
                        LicenseLevel = license.Element(nameof(DiveLicense.LicenseLevel))?.Value,
                        LicenseProvider = license.Element(nameof(DiveLicense.LicenseProvider))?.Value,
                        DiveCentre = license.Element(nameof(DiveLicense.DiveCentre))?.Value,
                        Location = license.Element(nameof(DiveLicense.Location))?.Value,
                        Id = license.Element(nameof(DiveLicense.Id))?.Value,
                    };

                    _diveLicenseList.Add(newLicense);
                }
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show($"Dive license file not found @ {_licenseDirectory}");
            }
            catch (XmlException e)
            {
                if (!_retriedLoad)
                {
                    CreateFile();
                    LoadData();
                    _retriedLoad = true;
                }

                MessageBox.Show($"Unable to load dive license file @ {_licenseDirectory}\n{e.Message}");
            }
            catch (Exception e)
            {
                MessageBox.Show($"An unexpected error occured while trying to load dive license file @ {_licenseDirectory}\n{e.Message}");
            }

            return _diveLicenseList;
        }

        public void DeleteFromFile(DiveLicense data)
        {
            string idToFind = data.UniqueId.ToString();
            XDocument _diveLicenseFile = XDocument.Load(_licenseDirectory);

            _diveLicenseFile.Descendants("DiveLicense").FirstOrDefault(d => (string)d.Element("UniqueId") == idToFind).Remove();
            _diveLicenseFile.Save(_licenseDirectory);
        }

        private void CreateFile()
        {
            _diveLicenseFile = new XDocument(new XElement("DiveLicenses"));
            _diveLicenseFile.Save(_licenseDirectory);
        }
    }
}
