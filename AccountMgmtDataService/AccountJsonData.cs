using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using AccountMgmtDataModel.Models;

namespace AccountManagementDataService
{
    public class CalendarJsonData
    {
        private readonly string _jsonFileName;
        private readonly object _lockObject = new object();

        public List<CalendarEvent> Events { get; private set; } = new List<CalendarEvent>();

        public CalendarJsonData()
        {
            _jsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "events.json");
            Load();
        }

        public void Load()
        {
            if (!File.Exists(_jsonFileName))
            {
                Events = new List<CalendarEvent>();
                return;
            }

            string json = File.ReadAllText(_jsonFileName);
            Events = JsonSerializer.Deserialize<List<CalendarEvent>>(json) ?? new List<CalendarEvent>();
        }

        public void Save()
        {
            lock (_lockObject)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(Events, options);
                File.WriteAllText(_jsonFileName, json);
            }
        }

        public void Add(CalendarEvent ev)
        {
            lock (_lockObject)
            {
                Events.Add(ev);
                Save();
            }
        }
    }
}