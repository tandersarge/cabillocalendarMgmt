using System;
using System.Collections.Generic;
using System.Text;
using AccountMgmtDataModel.Models; 

namespace AccountManagementDataService
{
   
    public class CalendarBL
    {
        private CalendarDBData _dbData = new CalendarDBData();

        public bool AddEvent(string date, string evDescription)
        {
            if (string.IsNullOrWhiteSpace(date) || string.IsNullOrWhiteSpace(evDescription)) return false;

            int nextId = (_dbData.GetEvents().Count > 0) ? _dbData.GetEvents().Max(e => e.EventId) + 1 : 1;

            var newEvent = new CalendarEvent
            {
                EventId = nextId,
                EventDate = date,
                EventDescription = evDescription
            };

            _dbData.Add(newEvent);
            return true;
        }

        public string ViewEvents()
        {
            var events = _dbData.GetEvents();
            if (events == null || !events.Any()) return "No events found.";

            StringBuilder sb = new StringBuilder();
            foreach (var ev in events)
            {
                sb.AppendLine($"{ev.EventId}. ({ev.EventDate}): {ev.EventDescription}");
            }
            return sb.ToString();
        }
    }

    internal class CalendarDBData
    {
        private CalendarJsonData _jsonData = new CalendarJsonData();

        internal void Add(CalendarEvent newEvent)
        {
            _jsonData.Add(newEvent);
        }
        
        internal List<CalendarEvent> GetEvents()
        {
            return _jsonData.Events;
        }
    }
}