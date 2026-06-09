using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AccountMgmtDataModel.Models;
using AppModel = AccountMgmtDataModel.Models.CalendarEvent;

namespace AccountManagementDataService
{
    public class CalendarBL
    {
        private CalendarDBData _dbData = new CalendarDBData();

        public bool AddEvent(string date, string evDescription)
        {
            if (string.IsNullOrWhiteSpace(date) || string.IsNullOrWhiteSpace(evDescription)) return false;
            var newEvent = new AppModel
            {
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

        public bool DeleteEvent(int id)
        {
            _dbData.Delete(id);
            return true;
        }

        public bool UpdateEvent(int id, string date, string description)
        {
            var updatedEvent = new AppModel
            {
                EventId = id,
                EventDate = date,
                EventDescription = description
            };
            _dbData.Update(updatedEvent);
            return true;
        }
    }

    internal class CalendarDBData
    {
        private CalendarJsonData _jsonData = new CalendarJsonData();

        internal void Add(AppModel newEvent)
        {
            _jsonData.Add(newEvent);
        }

        internal List<AppModel> GetEvents()
        {
            return _jsonData.Events;
        }

        internal void Delete(int id)
        {
            _jsonData.Delete(id);
        }

        internal void Update(AppModel updatedEvent)
        {
            _jsonData.Update(updatedEvent);
        }
    }
}