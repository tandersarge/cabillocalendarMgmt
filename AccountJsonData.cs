using System.Globalization;
using System.Text.Json;
using static AccountMgmtDataService.AccountDataService;

namespace AccountManagementDataService
{
    public class AccountJsonData
    {
        private readonly string _jsonFileName;
        private List<CalendarBL> _calendars = new List<CalendarBL>();
        private readonly object _lockObject = new object();

        public List<CalendarBL> Calendars
        {
            get { return _calendars; }
            private set { _calendars = value; }
        }

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

                if (Calendars.Count <= 0)
                {
                    Calendars.Add(new CalendarBL());
                    SaveDataToJsonFile();
                }
            }
        }

        private void SaveDataToJsonFile()
        {
            lock (_lockObject)
            {
                try
                {
                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string jsonString = JsonSerializer.Serialize(Calendars, options);
                    File.WriteAllText(_jsonFileName, jsonString);
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error saving calendars to JSON file: {ex.Message}");
                    throw;
                }
            }
        }

        private void RetrieveDataFromJsonFile()
        {
            lock (_lockObject)
            {
                try
                {
                    if (!File.Exists(_jsonFileName))
                    {
                        Calendars = new List<CalendarBL>();
                        return;
                    }

                    string jsonString = File.ReadAllText(_jsonFileName);
                    var deserializedCalendars = JsonSerializer.Deserialize<List<CalendarBL>>(jsonString);
                    Calendars = deserializedCalendars ?? new List<CalendarBL>();
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Error deserializing JSON: {ex.Message}");
                    Calendars = new List<CalendarBL>();
                }
                catch (IOException ex)
                {
                    Console.WriteLine($"Error reading calendars file: {ex.Message}");
                    Calendars = new List<CalendarBL>();
                }
            }
        }

        public void Add(CalendarBL calendar)
        {
            if (calendar == null)
                throw new ArgumentNullException(nameof(calendar), "Calendar object cannot be null.");

            lock (_lockObject)
            {
                Calendars.Add(calendar);
                SaveDataToJsonFile();
            }
        }

        public List<CalendarBL> GetCalendars()
        {
            lock (_lockObject)
            {
                RetrieveDataFromJsonFile();
                return new List<CalendarBL>(Calendars);
            }
        }

        public void DeleteCalendar(int index)
        {
            if (index < 0 || index >= Calendars.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Calendar index is out of range.");

            lock (_lockObject)
            {
                Calendars.RemoveAt(index);
                SaveDataToJsonFile();
            }
        }

        public CalendarBL GetCalendarAt(int index)
        {
            if (index < 0 || index >= Calendars.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Calendar index is out of range.");

            lock (_lockObject)
            {
                RetrieveDataFromJsonFile();
                return Calendars[index];
            }
        }
    }

    internal class CalendarBL
    {
    }
}