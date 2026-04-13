using AccountDataService;

namespace CabilloCalendar
{
    internal class Program
    {
        private static CalendarBL calendarBL = new CalendarBL();

        static void Main(string[] args)
        {
            bool manageOption = true;

            Console.WriteLine("--- CABILLO CALENDAR MANAGEMENT SYSTEM ---");


            while (manageOption)
            {
                Console.WriteLine("\nOptions:\n1 = (Add Event), 2 = (View events), 3 = (Update Event), 4 = (Delete Event), 5 = (Exit)");
                Console.Write("Select an option (1, 2, 3, 4, 5): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddEvent();
                        break;
                    case "2":
                        ViewEvents();
                        break;
                    case "3":
                        UpdateEvent();
                        break;
                    case "4":
                        DeleteEvent();
                        break;
                    case "5":
                        manageOption = false;
                        Console.WriteLine("Goodbye!");
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        static void AddEvent()
        {
            Console.Write("Enter a date (MM/DD/YYYY) \n(for example: 12/1/2005): ");
            string inputDate = Console.ReadLine();
            Console.Write("Enter your event for this day: ");
            string inputEvent = Console.ReadLine();

            if (calendarBL.AddEvent(inputDate, inputEvent))
            {
                Console.WriteLine("Your event has been successfully added!\n");
            }
            else
            {
                Console.WriteLine("Error: Could not add event. Please check your input or calendar capacity.\n");
            }
        }

        static void ViewEvents()
        {
            Console.WriteLine("\n--- YOUR SCHEDULES/EVENTS ---");
            string result = calendarBL.ViewEvents();
            Console.WriteLine(result);
        }

        static void UpdateEvent()
        {
            Console.Write("Enter the ID of event to update: ");
            if (int.TryParse(Console.ReadLine(), out int id) && id > 0)
            {
                Console.Write("Enter new date (MM/DD/YYYY): ");
                string newDate = Console.ReadLine();
                Console.Write("Enter new event: ");
                string newEvent = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(newDate) && !string.IsNullOrWhiteSpace(newEvent))
                {
                    if (calendarBL.UpdateEvent(id, newDate, newEvent))
                    {
                        Console.WriteLine("Event updated successfully!\n");
                    }
                    else
                    {
                        Console.WriteLine("Error: Could not find an event with that ID.\n");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Date and Event cannot be empty.\n");
                }
            }
        }

        static void DeleteEvent()
        {
            Console.Write("Enter the number of the event to delete: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0)
            {
                if (calendarBL.DeleteEvent(index))
                {
                    Console.WriteLine("Event deleted successfully!\n");
                }
                else
                {
                    Console.WriteLine("Invalid event number.\n");
                }
            }
            else
            {
                Console.WriteLine("Invalid event number.\n");
            }
        }
    }
}