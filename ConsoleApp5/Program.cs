using System;

namespace lab3_KN_23
{
    public struct FlightManagement : IComparable<FlightManagement>
    {
        public string flightNumber;
        public string departure;
        public string arrival;
        public int seats;
        
        public override string ToString()
        {
            return $"Flight: {flightNumber} | From: {departure} | To: {arrival} | Seats: {seats}";
        }
        
        public int CompareTo(FlightManagement other)
        {
            return string.Compare(this.flightNumber, other.flightNumber, StringComparison.Ordinal);
        }
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

    struct TicketPriceInfo
    {
        public string route;
        public int age;
        public int ticketCount;
        public double totalPrice;
    }
    
    class Program
    {
        static List<FlightManagement> flights = new List<FlightManagement>();
        
        static PassengerRegistration passenger;
        static TicketBooking booking;
        static FlightStatusInfo flightStatus;
        static FlightSearchInfo flightSearch;
        
        public static void Main(string[] args)
        {
            flights.Add(new FlightManagement { flightNumber = "AB100", departure = "Kyiv", arrival = "London", seats = 150 });
            flights.Add(new FlightManagement { flightNumber = "BC200", departure = "Lviv", arrival = "Berlin", seats = 100 });
            flights.Add(new FlightManagement { flightNumber = "ZA999", departure = "Odesa", arrival = "Paris", seats = 200 });

            LoginSystem();
            RenderIntro();
            ShowMainMenu();
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
            double choice;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write(prompt + " ");
            
            // Обробка помилок: Перевірка на коректний тип даних (double)
            while (!double.TryParse(Console.ReadLine(), out choice))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid number! Try again.");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write(prompt + " ");
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
                Console.WriteLine("1. Flight Management (Add/Search/Sort)");
                Console.WriteLine("2. Passenger Registration");
                Console.WriteLine("3. Ticket Booking");
                Console.WriteLine("4. Flight Status");
                Console.WriteLine("5. Flight Search (Input Params)");
                Console.WriteLine("6. Ticket Price Calculation");
                Console.WriteLine("7. View All Stored Data");
                Console.WriteLine("0. Exit");
                Console.ResetColor();

                double choice = GetUserInput("Select a menu option (0-7): ");

                switch (choice)
                {
                    case 1: ShowFlightSubMenu(); break;
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
        
        private static void ShowFlightSubMenu()
        {
            while (true)
            {
                Console.WriteLine("\n=== FLIGHT MANAGEMENT ===");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1. Add New Flight");
                Console.WriteLine("2. Display All Flights");
                Console.WriteLine("3. Search Flight by Number (ID)");
                Console.WriteLine("4. Sort Flights (Built-in List.Sort)");
                Console.WriteLine("5. Sort Flights (Bubble Sort - Custom)");
                Console.WriteLine("0. Back to Main Menu");
                Console.ResetColor();

                double choice = GetUserInput("Select a flight menu option (0-5): ");

                switch (choice)
                {
                    case 1: AddNewFlight(); break;
                    case 2: DisplayFlights(); break;
                    case 3: SearchFlight(); break;
                    case 4: SortFlightsBuiltIn(); break;
                    case 5: SortFlightsBubble(); break;
                    case 0: return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice! Try again.");
                        Console.ResetColor();
                        break;
                }
            }
        }
        
        private static void AddNewFlight()
        {
            Console.WriteLine("\n=== ADD NEW FLIGHT ===");
            FlightManagement newFlight = new FlightManagement();

            Console.Write("Flight Number: ");
            newFlight.flightNumber = Console.ReadLine();

            Console.Write("Departure City: ");
            newFlight.departure = Console.ReadLine();

            Console.Write("Arrival City: ");
            newFlight.arrival = Console.ReadLine();

            int seats;
            Console.Write("Number of Seats: ");
            
            while (!int.TryParse(Console.ReadLine(), out seats) || seats <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid value! Number of seats must be a positive integer. Try again.");
                Console.ResetColor();
                Console.Write("Number of Seats: ");
            }
            newFlight.seats = seats;

            flights.Add(newFlight);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Flight saved successfully.");
            Console.ResetColor();
        }
        
        private static void SearchFlight()
        {
            Console.WriteLine("\n=== SEARCH FLIGHT ===");
            Console.Write("Enter Flight Number (ID) to search: ");
            string searchNumber = Console.ReadLine();
            
            FlightManagement foundFlight = new FlightManagement { flightNumber = null }; 
            bool isFound = false;

            try
            {
                foreach (var flight in flights)
                {
                    if (flight.flightNumber != null && flight.flightNumber.Equals(searchNumber, StringComparison.OrdinalIgnoreCase))
                    {
                        foundFlight = flight;
                        isFound = true;
                        break;
                    }
                }

                if (isFound) 
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Flight Found:");
                    Console.WriteLine(foundFlight.ToString());
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: Flight with number '{searchNumber}' not found.");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred during search: {ex.Message}");
                Console.ResetColor();
            }
        }
        
        private static void SortFlightsBuiltIn()
        {
            Console.WriteLine("\n=== SORTING FLIGHTS (Built-in List.Sort) ===");
            
            if (flights.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No flights to sort.");
                Console.ResetColor();
                return;
            }

            try
            {
                flights.Sort(); 
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Flights sorted successfully by Flight Number (ID):");
                Console.ResetColor();
                DisplayFlights();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error during sorting: {ex.Message}");
                Console.ResetColor();
            }
        }
        
        private static void SortFlightsBubble()
        {
            Console.WriteLine("\n=== SORTING FLIGHTS (Custom Bubble Sort) ===");

            if (flights.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No flights to sort.");
                Console.ResetColor();
                return;
            }

            try
            {
                BubbleSortFlights(flights); 
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Flights sorted successfully by Flight Number (ID) using Bubble Sort:");
                Console.ResetColor();
                DisplayFlights();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Result comparison: The results of List.Sort() and Bubble Sort are the same (both sort by Flight Number).");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error during Bubble Sort: {ex.Message}");
                Console.ResetColor();
            }
        }
        
        private static void BubbleSortFlights(List<FlightManagement> list)
        {
            int n = list.Count;
            bool swapped;
            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (list[j].CompareTo(list[j + 1]) > 0)
                    {
                        FlightManagement temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                        swapped = true;
                    }
                }
                if (!swapped)
                    break;
            }
        }
        
        private static void DisplayFlights()
        {
            Console.WriteLine("\n--- ALL FLIGHTS ---");
            if (flights.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("The flight list is empty.");
                Console.ResetColor();
                return;
            }
            
            int index = 0;
            foreach (var flight in flights)
            {
                Console.WriteLine($"[{index++}] {flight}");
            }
        }

        private static void RegisterPassenger()
        {
            Console.WriteLine("\n=== PASSENGER REGISTRATION ===");

            Console.Write("Passenger Name: ");
            passenger.name = Console.ReadLine();

            Console.Write("Passport Number: ");
            passenger.passport = Console.ReadLine();

            Console.Write("Age: ");
            while (!int.TryParse(Console.ReadLine(), out passenger.age) || passenger.age < 0 || passenger.age > 120)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid age! Age must be a number between 0 and 120. Try again.");
                Console.ResetColor();
                Console.Write("Age: ");
                continue;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Passenger saved!");
            Console.ResetColor();
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

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Booking saved!");
            Console.ResetColor();
        }

        private static void ShowStatusMenu()
        {
            Console.WriteLine("\n=== FLIGHT STATUS ===");

            Console.Write("Flight Number: ");
            flightStatus.flightNumber = Console.ReadLine();

            Console.Write("Status (On Time / Delayed / Boarding): ");
            flightStatus.status = Console.ReadLine();

            Console.Write("Time: ");
            flightStatus.time = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Status saved!");
            Console.ResetColor();
        }

        private static void ShowSearchMenu()
        {
            Console.WriteLine("\n=== FLIGHT SEARCH (INPUT PARAMS) ===");

            Console.Write("Departure City: ");
            flightSearch.departure = Console.ReadLine();

            Console.Write("Arrival City: ");
            flightSearch.arrival = Console.ReadLine();

            Console.Write("Date (e.g., DD.MM.YYYY): ");
            flightSearch.date = Console.ReadLine();

            Console.Write("Direct Flight Only (Y/N): ");
            string directInput = Console.ReadLine().ToUpper();
            flightSearch.directFlight = (directInput == "Y");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Search parameters saved. Starting search...");
            Console.WriteLine($"(Departure: {flightSearch.departure}, Arrival: {flightSearch.arrival}, Date: {flightSearch.date}, Direct: {flightSearch.directFlight})");
            Console.ResetColor();
        }

        private static void TicketPrice()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Welcome to the airport");
            Console.ResetColor();

            Console.Write("Select point A: ");
            string cityA = Console.ReadLine();

            Console.Write("Select point B: ");
            string cityB = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Your flight: {cityA} - {cityB}");
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("Classes onboard:");
            Console.WriteLine("1. Economy class");
            Console.WriteLine("2. Comfort class");
            Console.WriteLine("3. Business class");
            
            int classChoice;
            Console.Write("Select a flight class: ");
            while (!int.TryParse(Console.ReadLine(), out classChoice) || classChoice < 1 || classChoice > 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid choice! Select 1, 2, or 3.");
                Console.ResetColor();
                Console.Write("Select a flight class: ");
            }

            int agePassenger;
            Console.Write("Enter your age: ");
            while (!int.TryParse(Console.ReadLine(), out agePassenger) || agePassenger < 0 || agePassenger > 120)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid age! Age must be a number between 0 and 120. Try again.");
                Console.ResetColor();
                Console.Write("Enter your age: ");
            }

            int tickets;
            Console.Write("Select the number of tickets: ");
            while (!int.TryParse(Console.ReadLine(), out tickets) || tickets < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid ticket count! Must be 1 or more.");
                Console.ResetColor();
                Console.Write("Select the number of tickets: ");
            }


            double basePrice = 100;

            double classMultiplier = 1;
            if (classChoice == 1)
                classMultiplier = 1;
            else if (classChoice == 2)
                classMultiplier = 1.5;
            else if (classChoice == 3)
                classMultiplier = 2;

            double agePrice = 1;
            if (agePassenger < 5)
                agePrice = 0.5;
            else if (agePassenger >= 5 && agePassenger < 18)
                agePrice = 0.7;

            double finalPrice = basePrice * classMultiplier * agePrice * tickets;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Total price: ${finalPrice}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Thank you for your purchase!");
            Console.ResetColor();
        }

        private static void ShowAllData()
        {
            Console.WriteLine("\n=== STORED DATA ===");
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("--- Flight Management (All Flights) ---");
            Console.ResetColor();
            DisplayFlights();
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Last Passenger Registration ---");
            Console.ResetColor();
            Console.WriteLine($"Passenger: {passenger.name}, Passport: {passenger.passport}, Age: {passenger.age}");
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Last Ticket Booking ---");
            Console.ResetColor();
            Console.WriteLine($"Booking: {booking.bookingID}, Passenger: {booking.passengerName}, Flight: {booking.flightNumber}");
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Last Flight Status ---");
            Console.ResetColor();
            Console.WriteLine($"Status: {flightStatus.flightNumber}, {flightStatus.status}, Time: {flightStatus.time}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n--- Last Flight Search Params ---");
            Console.ResetColor();
            Console.WriteLine($"Search: {flightSearch.departure} -> {flightSearch.arrival}, Date: {flightSearch.date}, Direct Flight: {flightSearch.directFlight}");
        }
    }
}