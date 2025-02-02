using DiveLogApplication.Core;
using DiveLogApplication.Utilities;
using DiveLogApplication.ViewModels;
using DiveLogApplication.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiveLogApplication.Models
{
    public class DiveLogAppData : ViewModel
    {
        public DiveLogAppData()
        {
            SettingsViewModel = new SettingsViewModel();
            IniFile = new IniFile(SettingsViewModel.DiveLogSettingsFilePath);
            DiveLicenseManager = new DiveLicenseManager();
            DiveLogManager = new DiveLogManager();

            // load app data into memory
            InitializeData();
        }

        public SettingsViewModel SettingsViewModel { get; set; }
        public IniFile IniFile { get; set; }
        public DiveLicenseManager DiveLicenseManager { get; set; }
        public DiveLogManager DiveLogManager { get; set; }

        // dive license & user profile properties
        public string ProfilePicturePath { get; set; }
        public int TotalDiveLicenses { get; set; }
        public ObservableCollection<DiveLicense> DiveLicenseList { get; set; }

        // dive log properties
        public string WelcomeMessage { get; set; }
        public string DiverSinceMessage {  get; set; }
        public string MostFrequentDiveSite {  get; set; }
        public int TotalDives { get; set; }
        public double LongestDive {  get; set; }
        public double DeepestDive {  get; set; }
        public double AverageDepth { get; set; }
        public DateTime LastDiveDate { get; set; }
        public ObservableCollection<DiveEntry> DiveLogList { get; set; }

        public void InitializeData()
        {
            LoadAppSettingsFromFile();
            LoadDiveLogFromFile();
            LoadDiveLicenseFromFile();
            LoadSummaryData();
        }

        public void AddLicense(DiveLicense diveLicense, DiveLicense existingLicense = null, bool isEdit = false)
        {
            if (isEdit)
            {
                DiveLicenseList.Remove(existingLicense);
                DiveLicenseList.Add(diveLicense);
            }
            else
            {
                DiveLicenseList.Add(diveLicense);
            }
        }

        public void DeleteLicense(DiveLicense diveLicense)
        {
            DiveLicenseList.Remove(diveLicense);
            DiveLicenseManager.DeleteFromFile(diveLicense);
        }

        public void AddDiveLog(DiveEntry diveEntry, int listIndex = 0, bool isNewEntry = false)
        {
            if (!isNewEntry)
            {
                DiveLogList.RemoveAt(listIndex);
                DiveLogList.Insert(listIndex, diveEntry);
            }
            else
            {
                DiveLogList.Add(diveEntry);
            }

            SortDiveEntriesDescending();
        }

        public void DeleteDiveLog(DiveEntry diveEntry)
        {
            DiveLogList.Remove(diveEntry);
            DiveLogManager.Delete(diveEntry);
        }

        private void LoadAppSettingsFromFile()
        {
            ProfilePicturePath = IniFile.Read(nameof(ProfilePicturePath), "General");
        }

        private void LoadDiveLogFromFile()
        {
            DiveLogManager.Load();
            DiveLogList = DiveLogManager.DiveLogList;
        }

        private void LoadDiveLicenseFromFile()
        {
            DiveLicenseList = DiveLicenseManager.LoadData();
        }

        private void LoadSummaryData()
        {
            LoadDiveLicenseSummary();
            LoadDiveLogSummary();
        }

        private void LoadDiveLicenseSummary()
        {
            List<DateTime> dateTimeList = new List<DateTime>();
            foreach (DiveLicense diveLicense in DiveLicenseList)
            {
                dateTimeList.Add(diveLicense.IssuedDate);
            }

            if (dateTimeList.Count > 0)
            {
                DateTime earliestLicense = dateTimeList.Min();
                DiverSinceMessage = $"Diver since {earliestLicense}";
            }

            WelcomeMessage = $"Welcome! You have a total of {TotalDives} dives.";
            
            TotalDiveLicenses = DiveLicenseList.Count;
        }

        private void LoadDiveLogSummary()
        {
            var diveLoglist = DiveLogList;

            TotalDives = diveLoglist.Count;
            LongestDive = diveLoglist.Max(p => p.EndTime.Subtract(p.StartTime).TotalMinutes);
            DeepestDive = diveLoglist.Max(p => p.MaxDepth);
            MostFrequentDiveSite = diveLoglist
                .Where(p => !string.IsNullOrWhiteSpace(p.DiveSite)) // Filter out empty/null entries
                .GroupBy(p => p.DiveSite)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()
                ?.Key ?? "N/A"; // Fallback if no valid DiveSites exist

            AverageDepth = Math.Round(diveLoglist.Average(p => p.AverageDepth), 2);
            LastDiveDate = diveLoglist.Max(p => p.EndTime);
        }

        private void SortDiveEntriesDescending()
        {
            var sortedList = DiveLogList.OrderByDescending(o => o.DiveLogIndex).ToList();

            DiveLogList.Clear();
            foreach (var item in sortedList)
            {
                DiveLogList.Add(item);
            }
        }
    }
}
