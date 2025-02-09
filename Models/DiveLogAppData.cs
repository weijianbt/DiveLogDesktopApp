﻿using DiveLogApplication.Core;
using DiveLogApplication.Utilities;
using DiveLogApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

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
        public string DiverSinceMessage { get; set; }
        public string MostFrequentDiveSite { get; set; }
        public int TotalDives { get; set; }
        public double LongestDive { get; set; }
        public double DeepestDive { get; set; }
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

        public void AddLicense(DiveLicense diveLicense, int listIndex = 0, bool isEdit = false)
        {
            if (isEdit)
            {
                DiveLicenseList.RemoveAt(listIndex);
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

        public void AddDiveLog(DiveEntry diveEntry, int listIndex = 0, bool isEdit = false)
        {
            if (isEdit)
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
            if (DiveLogList.Count == 0)
            {
                return;
            }

            var diveLoglist = DiveLogList;

            TotalDives = diveLoglist.Count;
            DeepestDive = diveLoglist.Max(p => p.MaxDepth);
            MostFrequentDiveSite = diveLoglist
                .Where(p => !string.IsNullOrWhiteSpace(p.DiveSite)) // Filter out empty/null entries
                .GroupBy(p => p.DiveSite)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()
                ?.Key ?? "N/A"; // Fallback if no valid DiveSites exist

            AverageDepth = Math.Round(diveLoglist.Average(p => p.AverageDepth), 2);
            LongestDive = diveLoglist.Max(p => p.Duration);
            LastDiveDate = ParseDiveDates(diveLoglist);
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

        private DateTime ParseDiveDates(ObservableCollection<DiveEntry> diveLogList)
        {
            // set default date to current time
            DateTime latestDiveDate = DateTime.Now;

            for (int i = 0; i < diveLogList.Count; i++)
            {
                if (i == 0)
                {
                    DateTime.TryParseExact(diveLogList[i].EndTime, "dd/MM/yyyy hh:mm tt", new CultureInfo("en-MY"), DateTimeStyles.None, out DateTime parsedDiveEndTime);
                    latestDiveDate = parsedDiveEndTime;
                }

                // parse the start and end time, then calculate the duration
                DateTime.TryParseExact(diveLogList[i].EndTime, "dd/MM/yyyy hh:mm tt", new CultureInfo("en-MY"), DateTimeStyles.None, out DateTime endTime);

                // get latest dive
                if (endTime > latestDiveDate)
                {
                    latestDiveDate = endTime;
                }
            }

            return latestDiveDate;
        }
    }
}
