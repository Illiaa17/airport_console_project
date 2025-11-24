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

        struct TicketPriceInfo
        {
            public string route;
            public int age;
            public int ticketCount;
            public double totalPrice;
        }
        

        static FlightManagement flight;
        static PassengerRegistration passenger;
        static TicketBooking booking;
        static FlightStatusInfo flightStatus;
        
        
        public static void Main(string[] args)
        {
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
                    case 1: ShowFlightMenu(); break;
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
        

        private static void ShowFlightMenu()
        {
            Console.WriteLine("\n=== ADD NEW FLIGHT ===");

            Console.Write("Flight Number: ");
            flight.flightNumber = Console.ReadLine();

            Console.Write("Departure City: ");
            flight.departure = Console.ReadLine();

            Console.Write("Arrival City: ");
            flight.arrival = Console.ReadLine();

            Console.Write("Number of Seats: ");
            bool ok = int.TryParse(Console.ReadLine(), out flight.seats);

            if (!ok)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid value! Flight not saved.");
                Console.ResetColor();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Flight saved successfully.");
            Console.ResetColor();
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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid age! Try again.");
                Console.ResetColor();
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
            Console.WriteLine("\nFlight search function is under development...");
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
            Console.Write("Select a flight class: ");
            int classChoice = int.Parse(Console.ReadLine());

            Console.Write("Enter your age: ");
            int agePassenger = int.Parse(Console.ReadLine());

            Console.Write("Select the number of tickets: ");
            int tickets = int.Parse(Console.ReadLine());

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

            ShowMainMenu();
        }
        

        private static void ShowAllData()
        {
            Console.WriteLine("\n=== STORED DATA ===");

            Console.WriteLine($"Flight: {flight.flightNumber}, {flight.departure} -> {flight.arrival}, Seats: {flight.seats}");
            Console.WriteLine($"Passenger: {passenger.name}, Passport: {passenger.passport}, Age: {passenger.age}");
            Console.WriteLine($"Booking: {booking.bookingID}, Passenger: {booking.passengerName}, Flight: {booking.flightNumber}");
            Console.WriteLine($"Status: {flightStatus.flightNumber}, {flightStatus.status}, Time: {flightStatus.time}");
        }
    }
}
