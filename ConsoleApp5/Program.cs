using System;

namespace lab3_KN_23
{
    class Program
    {

        struct FlightManagement
        {
            public string flightNumber;
            public string departure;
            public string arrival;
            public int seats;
        }

        struct PassengerRegistration
        {
            public string name;
            public string passport;
            public int age;
        }

        struct TicketBooking
        {
            public string bookingID;
            public string flightNumber;
            public string passengerName;
        }

        struct FlightStatusInfo
        {
            public string flightNumber;
            public string status;
            public string time;
        }

        struct FlightSearchInfo
        {
            public string departure;
            public string arrival;
            public string date;
            public bool directFlight;
        }

        
        static List<FlightManagement> flights = new List<FlightManagement>();

        static PassengerRegistration passenger;
        static TicketBooking booking;
        static FlightStatusInfo flightStatus;
        static FlightSearchInfo flightSearch;

        public static void Main(string[] args)
        {
            InitializeData(); 
            LoginSystem();
            RenderIntro();
            ShowMainMenu();
        }

        private static void InitializeData()
        {
            flights.Add(new FlightManagement { flightNumber = "PS101", departure = "Kyiv", arrival = "London", seats = 150 });
            flights.Add(new FlightManagement { flightNumber = "LH404", departure = "Berlin", arrival = "New York", seats = 200 });
            flights.Add(new FlightManagement { flightNumber = "FR303", departure = "Kyiv", arrival = "Warsaw", seats = 180 });
            flights.Add(new FlightManagement { flightNumber = "TK202", departure = "Istanbul", arrival = "Kyiv", seats = 160 });
            flights.Add(new FlightManagement { flightNumber = "BA505", departure = "London", arrival = "Paris", seats = 140 });
        }

        private static void LoginSystem()
        {
            string correctLogin = "airport";
            string correctPass = "12345678";
            int attempts = 0;

            do
            {
                Console.Write("\nEnter login: ");
                string login = Console.ReadLine();
                Console.Write("Enter password: ");
                string pass = Console.ReadLine();

                if (login == correctLogin && pass == correctPass)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Login successful!\n");
                    Console.ResetColor();
                    return;
                }
                else
                {
                    attempts++;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect login or password.");
                    Console.ResetColor();
                    if (attempts == 3)
                    {
                        Console.WriteLine("Access denied. Program will close.");
                        Environment.Exit(0);
                    }
                }
            } while (true);
        }

        public static void RenderIntro()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("============================================");
            Console.WriteLine("========== WELCOME TO THE AIRPORT ==========");
            Console.WriteLine("============================================");
            Console.ResetColor();
        }

        public static double GetUserInput(string prompt = "Enter a number: ")
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(prompt + " ");
            bool isNumber = double.TryParse(Console.ReadLine(), out double choice);

            if (!isNumber)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid number! Try again.");
                Console.ResetColor();
                return GetUserInput(prompt);
            }
            Console.ResetColor();
            return choice;
        }

        public static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Flight Management");
                Console.WriteLine("2. Passenger Registration");
                Console.WriteLine("3. Ticket Booking");
                Console.WriteLine("4. Flight Status");
                Console.WriteLine("5. Flight Search");
                Console.WriteLine("6. Ticket Price Calculation");
                Console.WriteLine("7. View All Stored Data");
                Console.WriteLine("0. Exit");
                Console.ResetColor();

                double choice = GetUserInput("Select a menu option (0-7): ");

                switch (choice)
                {
                    case 1: ShowFlightOperationsMenu(); break;
                    case 2: RegisterPassenger(); break;
                    case 3: ShowBookingMenu(); break;
                    case 4: ShowStatusMenu(); break;
                    case 5: ShowSearchMenu(); break;
                    case 6: TicketPrice(); break;
                    case 7: ShowAllData(); break;
                    case 0:
                        Console.WriteLine("Exiting program. Goodbye!");
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice! Try again.");
                        Console.ResetColor();
                        break;
                }
            }
        }
        
        private static void ShowFlightOperationsMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- FLIGHT MANAGEMENT OPERATIONS ---");
                Console.WriteLine("1. Add new flight");
                Console.WriteLine("2. Show all flights");
                Console.WriteLine("3. Search flight");
                Console.WriteLine("4. Delete flight");
                Console.WriteLine("5. Sort flights");
                Console.WriteLine("6. Statistics");
                Console.WriteLine("0. Back to Main Menu");

                double choice = GetUserInput("Select option: ");

                switch (choice)
                {
                    case 1: AddFlightsLoop(); break;
                    case 2: RenderFlightsTable(flights); break;
                    case 3: SearchFlight(); break;
                    case 4: DeleteFlight(); break;
                    case 5: SortFlightsMenu(); break;
                    case 6: ShowStatistics(); break;
                    case 0: return;
                    default: Console.WriteLine("Invalid option."); break;
                }
            }
        }
        
        private static void AddFlightsLoop()
        {
            Console.Write("How many flights do you want to add? ");
            if (int.TryParse(Console.ReadLine(), out int count) && count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine($"\nAdding flight #{i + 1}:");
                    FlightManagement newFlight = new FlightManagement();

                    Console.Write("Flight Number: ");
                    newFlight.flightNumber = Console.ReadLine();
                    Console.Write("Departure City: ");
                    newFlight.departure = Console.ReadLine();
                    Console.Write("Arrival City: ");
                    newFlight.arrival = Console.ReadLine();
                    Console.Write("Number of Seats: ");
                    while (!int.TryParse(Console.ReadLine(), out newFlight.seats) || newFlight.seats <= 0)
                    {
                        Console.WriteLine("Invalid input. Please enter a positive integer for seats.");
                    }

                    flights.Add(newFlight);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Flights added successfully.");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Invalid number.");
            }
        }
        
        private static void RenderFlightsTable(List<FlightManagement> listToPrint)
        {
            if (listToPrint.Count == 0)
            {
                Console.WriteLine("No flights available.");
                return;
            }

            Console.WriteLine("\n-------------------------------------------------------------");
            Console.WriteLine("| {0,-10} | {1,-15} | {2,-15} | {3,-5} |", "Flight #", "Departure", "Arrival", "Seats");
            Console.WriteLine("-------------------------------------------------------------");

            foreach (var f in listToPrint)
            {
                Console.WriteLine("| {0,-10} | {1,-15} | {2,-15} | {3,-5} |", f.flightNumber, f.departure, f.arrival, f.seats);
            }
            Console.WriteLine("-------------------------------------------------------------");
        }
        
        private static void SearchFlight()
        {
            Console.Write("Enter parameter to search (Flight Number or City): ");
            string query = Console.ReadLine().ToLower();
            
            List<FlightManagement> results = new List<FlightManagement>();
            
            foreach (var f in flights)
            {
                if (f.flightNumber.ToLower().Contains(query) || 
                    f.departure.ToLower().Contains(query) || 
                    f.arrival.ToLower().Contains(query))
                {
                    results.Add(f);
                }
            }

            if (results.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Found {results.Count} flights:");
                Console.ResetColor();
                RenderFlightsTable(results);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No flights found.");
                Console.ResetColor();
            }
        }
        
        private static void DeleteFlight()
        {
            Console.WriteLine("1. Delete by Index");
            Console.WriteLine("2. Delete by Flight Number");
            double choice = GetUserInput("Choice: ");

            if (choice == 1)
            {
                RenderFlightsTable(flights);
                Console.Write("Enter index (row number starting from 0): ");
                if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < flights.Count)
                {
                    flights.RemoveAt(index);
                    Console.WriteLine("Deleted.");
                }
                else
                {
                    Console.WriteLine("Invalid index.");
                }
            }
            else if (choice == 2)
            {
                Console.Write("Enter Flight Number to delete: ");
                string num = Console.ReadLine();
                bool deleted = false;
                
                for (int i = flights.Count - 1; i >= 0; i--)
                {
                    if (flights[i].flightNumber == num)
                    {
                        flights.RemoveAt(i);
                        deleted = true;
                    }
                }

                if (deleted) Console.WriteLine("Deleted successfully.");
                else Console.WriteLine("Flight not found.");
            }
        }
        
        private static void SortFlightsMenu()
        {
            Console.WriteLine("1. Sort by Seats (Ascending) - Built-in List.Sort");
            Console.WriteLine("2. Sort by Seats (Ascending) - Custom Bubble Sort");
            double choice = GetUserInput("Choice: ");

            if (choice == 1)
            {
                flights.Sort((x, y) => x.seats.CompareTo(y.seats));
                Console.WriteLine("Sorted using Built-in Sort.");
                RenderFlightsTable(flights);
            }
            else if (choice == 2)
            {
                BubbleSortFlights();
                Console.WriteLine("Sorted using Bubble Sort.");
                RenderFlightsTable(flights);
            }
        }

        private static void BubbleSortFlights()
        {
            for (int i = 0; i < flights.Count - 1; i++)
            {
                for (int j = 0; j < flights.Count - i - 1; j++)
                {
                    if (flights[j].seats > flights[j + 1].seats)
                    {
                        FlightManagement temp = flights[j];
                        flights[j] = flights[j + 1];
                        flights[j + 1] = temp;
                    }
                }
            }
        }
        
        private static void ShowStatistics()
        {
            if (flights.Count == 0)
            {
                Console.WriteLine("No data for statistics.");
                return;
            }

            int count = flights.Count;
            int totalSeats = 0;
            int minSeats = flights[0].seats; 
            int maxSeats = flights[0].seats;
            
            foreach (var f in flights)
            {
                totalSeats += f.seats;
                
                if (f.seats < minSeats) minSeats = f.seats;
                
                if (f.seats > maxSeats) maxSeats = f.seats;
            }
            
            double avgSeats = (double)totalSeats / count;

            Console.WriteLine("\n=== FLIGHT STATISTICS ===");
            Console.WriteLine($"Total Flights stored: {count}");
            Console.WriteLine($"Total Seats (All flights): {totalSeats}");
            Console.WriteLine($"Min Seats on a flight: {minSeats}");
            Console.WriteLine($"Max Seats on a flight: {maxSeats}");
            Console.WriteLine($"Average Seats: {avgSeats:F2}");
        }
        

        private static void RegisterPassenger()
        {
            Console.WriteLine("\n=== PASSENGER REGISTRATION ===");
            Console.Write("Passenger Name: ");
            passenger.name = Console.ReadLine();
            Console.Write("Passport Number: ");
            passenger.passport = Console.ReadLine();
            Console.Write("Age: ");
            while (!int.TryParse(Console.ReadLine(), out passenger.age))
            {
                Console.WriteLine("Invalid age!");
            }
            Console.WriteLine("Passenger saved!");
        }

        private static void ShowBookingMenu()
        {
            Console.WriteLine("\n=== TICKET BOOKING ===");
            Console.Write("Booking ID: ");
            booking.bookingID = Console.ReadLine();
            Console.Write("Flight Number: ");
            booking.flightNumber = Console.ReadLine();
            Console.Write("Passenger Name: ");
            booking.passengerName = Console.ReadLine();
            Console.WriteLine("Booking saved!");
        }

        private static void ShowStatusMenu()
        {
            Console.WriteLine("\n=== FLIGHT STATUS ===");
            Console.Write("Flight Number: ");
            flightStatus.flightNumber = Console.ReadLine();
            Console.Write("Status: ");
            flightStatus.status = Console.ReadLine();
            Console.Write("Time: ");
            flightStatus.time = Console.ReadLine();
            Console.WriteLine("Status saved!");
        }

        private static void ShowSearchMenu()
        {
            Console.WriteLine("\n=== FLIGHT SEARCH PARAMETERS ===");
            Console.Write("Departure City: ");
            flightSearch.departure = Console.ReadLine();
            Console.Write("Arrival City: ");
            flightSearch.arrival = Console.ReadLine();
            Console.Write("Date: ");
            flightSearch.date = Console.ReadLine();
            Console.WriteLine("Search parameters saved.");
        }

        private static void TicketPrice()
        {
            Console.WriteLine("\n--- Quick Ticket Price Calc ---");
            Console.Write("Calculation executed... Total: $150 ");
        }

        private static void ShowAllData()
        {
            Console.WriteLine("\n=== ALL STORED DATA ===");
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--- Flight Management (List) ---");
            Console.ResetColor();
            RenderFlightsTable(flights);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Last Registered Passenger ---");
            Console.ResetColor();
            Console.WriteLine($"Name: {passenger.name}, Passport: {passenger.passport}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Last Booking ---");
            Console.ResetColor();
            Console.WriteLine($"ID: {booking.bookingID}, Flight: {booking.flightNumber}");
        }
    }
}