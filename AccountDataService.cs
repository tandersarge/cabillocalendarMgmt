using System.Text;

namespace AccountDataService
{
    public class CalendarBL
    {
        private CalendarDBData _dbData = new CalendarDBData();

        public bool AddEvent(string date, string evDescription)
        {
            if (string.IsNullOrWhiteSpace(date) || string.IsNullOrWhiteSpace(evDescription))
            {
                return false;
            }

            var newEvent = new CalendarEvent
            {
                EventDate = date,
                EventDescription = evDescription
            };

            _dbData.Add(newEvent);
            return true;
        }

        public string ViewEvents()
        {
            List<CalendarEvent> events = _dbData.GetEvents();

            if (events.Count == 0)
            {
                return "Your calendar is currently empty.";
            }

            StringBuilder sb = new StringBuilder();
            foreach (var ev in events)
            {
                sb.AppendLine($"{ev.EventId}. ({ev.EventDate}) : {ev.EventDescription}");
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
            var updatedEvent = new CalendarEvent
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
        private List<CalendarEvent> _events = new List<CalendarEvent>();

        internal void Add(CalendarEvent newEvent)
        {
            _events.Add(newEvent);
        }

        internal void Delete(int id)
        {
            var eventToDelete = _events.FirstOrDefault(e => e.EventId == id);
            if (eventToDelete != null)
            {
                _events.Remove(eventToDelete);
            }
        }

        internal List<CalendarEvent> GetEvents()
        {
            return _events;
        }

        internal void Update(CalendarEvent updatedEvent)
        {
            var existingEvent = _events.FirstOrDefault(e => e.EventId == updatedEvent.EventId);

            if (existingEvent != null)
            {
                existingEvent.EventDate = updatedEvent.EventDate;
                existingEvent.EventDescription = updatedEvent.EventDescription;
            }
        }
    }
}