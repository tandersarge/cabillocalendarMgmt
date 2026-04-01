namespace CabilloCalendar.Models
{

    public class CalendarEvent
    {
        public int EventId { get; set; }
        public string EventDate { get; set; }
        public string EventDescription { get; set; }
        public object Date { get; set; }
        public object Description { get; set; }
    }
}