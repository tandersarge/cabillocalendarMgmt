using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using AccountMgmtDataModel.Models;

namespace AccountManagementDataService
{
    public class CalendarJsonData
    {
        private readonly string _connectionString =
            "Server=localhost\\SQLEXPRESS;Database=CabilloCalendarDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public List<CalendarEvent> Events
        {
            get { return GetAll(); }
        }

        private List<CalendarEvent> GetAll()
        {
            var list = new List<CalendarEvent>();
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand("SELECT EventId, EventDate, EventDescription FROM CalendarEvents", conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new CalendarEvent
                {
                    EventId = reader.GetInt32(0),
                    EventDate = reader.GetString(1),
                    EventDescription = reader.GetString(2)
                });
            }
            return list;
        }

        public void Add(CalendarEvent ev)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand(
                "INSERT INTO CalendarEvents (EventDate, EventDescription) VALUES (@date, @desc)", conn);
            cmd.Parameters.AddWithValue("@date", ev.EventDate);
            cmd.Parameters.AddWithValue("@desc", ev.EventDescription);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand("DELETE FROM CalendarEvents WHERE EventId = @id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public void Update(CalendarEvent ev)
        {
            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            var cmd = new SqlCommand(
                "UPDATE CalendarEvents SET EventDate = @date, EventDescription = @desc WHERE EventId = @id", conn);
            cmd.Parameters.AddWithValue("@date", ev.EventDate);
            cmd.Parameters.AddWithValue("@desc", ev.EventDescription);
            cmd.Parameters.AddWithValue("@id", ev.EventId);
            cmd.ExecuteNonQuery();
        }

        public void Save() { } 
    }
}