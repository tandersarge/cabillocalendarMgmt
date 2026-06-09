using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AppModel = AccountMgmtDataModel.Models.CalendarEvent;
using MyDataService = AccountManagementDataService;

namespace CabilloCalendar
{
    public class CalendarBL
    {
        private CalendarDBData _dbData = new CalendarDBData();

        public bool AddEvent(string date, string description)
        {
            var newEvent = new AppModel
            {
                EventDate = date,
                EventDescription = description
            };
            _dbData.Add(newEvent);
            return true;
        }

        public string ViewEvents()
        {
            List<AppModel> events = _dbData.GetEvents();
            if (events == null || events.Count == 0) return "No events found. Please add an event first...";
            StringBuilder sb = new StringBuilder();
            foreach (var ev in events)
            {
                sb.AppendLine($"[{ev.EventId}] {ev.EventDate} - {ev.EventDescription}");
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
        private MyDataService.CalendarJsonData _jsonData = new MyDataService.CalendarJsonData();

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