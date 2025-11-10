using System;

namespace lab2_KN_23
{
    class Program
    {
        public static void Main(string[] args)
        {
            RenderIntro();
            ShowMainMenu();
        }

        public static void RenderIntro()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("=========================================");
            Console.WriteLine("========= WELCOME TO THE AIRPORT! =======");
            Console.WriteLine("=========================================");
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
                Console.WriteLine("You did not enter a valid number. Try again.");
                Console.ResetColor();
                return GetUserInput(prompt);
            }

            Console.ResetColor();
            return choice;
        }

        public static void ShowMainMenu()
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
            Console.WriteLine("0. Exit");
            Console.ResetColor();

            double choice = GetUserInput("Select a menu option (0-6): ");

            switch (choice)
            {
                case 1:
                    ShowFlightMenu();
                    break;
                case 2:
                    ShowPassengerMenu();
                    break;
                case 3:
                    ShowBookingMenu();
                    break;
                case 4:
                    ShowStatusMenu();
                    break;
                case 5:
                    ShowSearchMenu();
                    break;
                case 6:
                    TicketPrice(); 
                    break;
                case 0:
                    Console.WriteLine("Exiting the program. Goodbye!");
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice. Please try again.");
                    Console.ResetColor();
                    ShowMainMenu();
                    break;
            }
        }

        private static void ShowFlightMenu()
        {
            Console.WriteLine("\n=== Flight Management ===");
            Console.WriteLine("1. Add Flight");
            Console.WriteLine("2. View All Flights");
            Console.WriteLine("3. Edit Flight");
            Console.WriteLine("4. Delete Flight");
            Console.WriteLine("5. Return to Main Menu");

            double choice = GetUserInput("Select an action (1-5): ");

            if (choice >= 1 && choice <= 4)
            {
                Console.WriteLine("Function under development... Returning to flight menu.");
                ShowFlightMenu();
            }
            else
            {
                ShowMainMenu();
            }
        }

        private static void ShowPassengerMenu()
        {
            Console.WriteLine("\nPassenger management function under development...");
            ShowMainMenu();
        }

        private static void ShowBookingMenu()
        {
            Console.WriteLine("\nTicket booking function under development...");
            ShowMainMenu();
        }

        private static void ShowStatusMenu()
        {
            Console.WriteLine("\nFlight status check function under development...");
            ShowMainMenu();
        }

        private static void ShowSearchMenu()
        {
            Console.WriteLine("\nFlight search function under development...");
            ShowMainMenu();
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
    }
}
