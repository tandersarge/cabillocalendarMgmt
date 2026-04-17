using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AppModel = AccountMgmtDataModel.Models.CalendarEvent;
using DataEntity = AccountMgmtDataModel.Models.CalendarEvent;
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
            int nextId = (_jsonData.Events != null && _jsonData.Events.Count > 0)
                         ? _jsonData.Events.Max(e => e.EventId) + 1
                         : 1;

            newEvent.EventId = nextId;

            _jsonData.Add(newEvent);
            _jsonData.Save(); 
        }

        internal List<AppModel> GetEvents()
        {
            return _jsonData.Events;
        }

        internal void Delete(int id)
        {
            var eventToRemove = _jsonData.Events.FirstOrDefault(e => e.EventId == id);
            if (eventToRemove != null)
            {
                _jsonData.Events.Remove(eventToRemove);
                _jsonData.Save();
            }
        }

        internal void Update(AppModel updatedEvent)
        {
            var existingEvent = _jsonData.Events.FirstOrDefault(e => e.EventId == updatedEvent.EventId);
            if (existingEvent != null)
            {
                existingEvent.EventDate = updatedEvent.EventDate;
                existingEvent.EventDescription = updatedEvent.EventDescription;
                _jsonData.Save();
            }
        }
    }
}