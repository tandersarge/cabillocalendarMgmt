using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AccountDataService;

namespace AccountManagementDataService
{
    public class AccountJsonData
    {
        private readonly string _jsonFileName;
        private readonly object _lockObject = new object();

        // Must be PUBLIC to fix CS1061
        public List<CalendarEvent> Events { get; private set; } = new List<CalendarEvent>();

        public AccountJsonData()
        {
            _jsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "events.json");
            PopulateJsonFile();
        }

        private void PopulateJsonFile()
        {
            lock (_lockObject)
            {
                if (File.Exists(_jsonFileName))
                {
                    RetrieveDataFromJsonFile();
                }
                else
                {
                    Events = new List<CalendarEvent>();
                    SaveDataToJsonFile();
                }
            }
        }

        public void SaveDataToJsonFile()
        {
            lock (_lockObject)
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(Events, options);
                    File.WriteAllText(_jsonFileName, jsonString);
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error saving to JSON: {ex.Message}");
                }
            }
        }

        private void RetrieveDataFromJsonFile()
        {
            lock (_lockObject)
            {
                try
                {
                    string jsonString = File.ReadAllText(_jsonFileName);
                    Events = JsonSerializer.Deserialize<List<CalendarEvent>>(jsonString) ?? new List<CalendarEvent>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading JSON: {ex.Message}");
                    Events = new List<CalendarEvent>();
                }
            }
        }

        // Must be PUBLIC to fix CS1061
        public void Add(CalendarEvent newEvent)
        {
            lock (_lockObject)
            {
                Events.Add(newEvent);
                SaveDataToJsonFile();
            }
        }
    }
}