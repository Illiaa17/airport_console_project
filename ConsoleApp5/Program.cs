using System;

namespace lab3_KN_23
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

    public class Flight
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public string Departure { get; set; }
        public string Arrival { get; set; }
        public int Seats { get; set; }

        public string ToCsv() => $"{Id},{FlightNumber},{Departure},{Arrival},{Seats}";

        public static Flight FromCsv(string csvLine)
        {
            string[] v = csvLine.Split(',');
            return new Flight { Id = int.Parse(v[0]), FlightNumber = v[1], Departure = v[2], Arrival = v[3], Seats = int.Parse(v[4]) };
        }
    }

    public class Passenger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Passport { get; set; }
        public int Age { get; set; }

        public string ToCsv() => $"{Id},{Name},{Passport},{Age}";

        public static Passenger FromCsv(string csvLine)
        {
            string[] v = csvLine.Split(',');
            return new Passenger { Id = int.Parse(v[0]), Name = v[1], Passport = v[2], Age = int.Parse(v[3]) };
        }
    }

    public class Booking
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public string PassengerName { get; set; }

        public string ToCsv() => $"{Id},{FlightNumber},{PassengerName}";

        public static Booking FromCsv(string csvLine)
        {
            string[] v = csvLine.Split(',');
            return new Booking { Id = int.Parse(v[0]), FlightNumber = v[1], PassengerName = v[2] };
        }
    }

    public class FlightStatus
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; }
        public string Status { get; set; }
        public string Time { get; set; }

        public string ToCsv() => $"{Id},{FlightNumber},{Status},{Time}";

        public static FlightStatus FromCsv(string csvLine)
        {
            string[] v = csvLine.Split(',');
            return new FlightStatus { Id = int.Parse(v[0]), FlightNumber = v[1], Status = v[2], Time = v[3] };
        }
    }
    
    public static class FileManager
    {
        private const string FlightsFile = "flights.csv";
        private const string UsersFile = "users.csv";
        private const string PassFile = "passengers.csv";
        private const string BookFile = "bookings.csv";
        private const string StatusFile = "statuses.csv";

        public static void InitializeFiles()
        {
            CreateIfNotExists(FlightsFile, "Id,FlightNumber,Departure,Arrival,Seats");
            CreateIfNotExists(UsersFile, "Id,Email,PasswordHash");
            CreateIfNotExists(PassFile, "Id,Name,Passport,Age");
            CreateIfNotExists(BookFile, "Id,FlightNumber,PassengerName");
            CreateIfNotExists(StatusFile, "Id,FlightNumber,Status,Time");
        }

        private static void CreateIfNotExists(string path, string header)
        {
            if (!System.IO.File.Exists(path)) 
                System.IO.File.WriteAllText(path, header + Environment.NewLine);
        }

        public static int GetNextId(string filePath)
        {
            try
            {
                var lines = System.IO.File.ReadAllLines(filePath);
                if (lines.Length <= 1) return 1;
                int maxId = 0;
                for (int i = 1; i < lines.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(lines[i])) continue;
                    var cols = lines[i].Split(',');
                    if (int.TryParse(cols[0], out int id))
                    {
                        if (id > maxId) maxId = id;
                    }
                }
                return maxId + 1;
            }
            catch { return 1; }
        }

        public static void AppendLine(string filePath, string line)
        {
            System.IO.File.AppendAllText(filePath, line + Environment.NewLine);
        }

        public static string[] ReadAllLines(string filePath)
        {
            if (!System.IO.File.Exists(filePath)) return new string[0];
            return System.IO.File.ReadAllLines(filePath);
        }

        public static void WriteAllText(string filePath, string content)
        {
            System.IO.File.WriteAllText(filePath, content);
        }
        
        public static System.Collections.Generic.List<Flight> ReadFlights() => ReadData(FlightsFile, Flight.FromCsv);

        public static System.Collections.Generic.List<User> ReadUsers() => ReadData(UsersFile, line => {
            var v = line.Split(',');
            return new User { Id = int.Parse(v[0]), Email = v[1].Trim(), PasswordHash = v[2].Trim() };
        });

        public static System.Collections.Generic.List<Passenger> ReadPassengers() => ReadData(PassFile, Passenger.FromCsv);
        public static System.Collections.Generic.List<Booking> ReadBookings() => ReadData(BookFile, Booking.FromCsv);
        public static System.Collections.Generic.List<FlightStatus> ReadStatuses() => ReadData(StatusFile, FlightStatus.FromCsv);

        private static System.Collections.Generic.List<T> ReadData<T>(string file, Func<string, T> mapper)
        {
            var list = new System.Collections.Generic.List<T>();
            try
            {
                var lines = ReadAllLines(file);
                for (int i = 1; i < lines.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(lines[i]))
                    {
                        try { list.Add(mapper(lines[i])); } catch { }
                    }
                }
            }
            catch { Console.WriteLine($"Error reading {file}"); }
            return list;
        }

        public static void RewriteFlights(System.Collections.Generic.List<Flight> list)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder("Id,FlightNumber,Departure,Arrival,Seats\n");
            foreach (var x in list)
            {
                sb.AppendLine(x.ToCsv());
            }
            WriteAllText(FlightsFile, sb.ToString());
        }
    }
    
    public static class Security
    {
        public static string HashPassword(string password)
        {
            using (System.Security.Cryptography.SHA256 sha = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
    
    class Program
    {
        static User currentUser = null;

        public static void Main(string[] args)
        {
            FileManager.InitializeFiles();

            // Default Admin check
            if (FileManager.ReadUsers().Count == 0)
                FileManager.AppendLine("users.csv", $"1,admin,{Security.HashPassword("admin123")}");

            AuthSystem();

            if (currentUser != null)
            {
                RenderIntro();
                ShowMainMenu();
            }
        }

        private static void AuthSystem()
        {
            while (true)
            {
                Console.WriteLine("\n=== AUTHENTICATION ===");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("0. Exit");
                Console.Write("Select option: ");
                string choice = Console.ReadLine();

                if (choice == "1") { if (Login()) break; }
                else if (choice == "2") Register();
                else if (choice == "0") Environment.Exit(0);
                else Console.WriteLine("Invalid choice.");
            }
        }

        private static bool Login()
        {
            Console.Write("Login: "); string email = Console.ReadLine();
            Console.Write("Password: "); string pass = Console.ReadLine();
            string hash = Security.HashPassword(pass);

            System.Collections.Generic.List<User> users = FileManager.ReadUsers();
            User foundUser = null;

            // Manual Search
            foreach (var u in users)
            {
                if (u.Email == email && u.PasswordHash == hash)
                {
                    foundUser = u;
                    break;
                }
            }

            if (foundUser != null)
            {
                currentUser = foundUser;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Login successful!");
                Console.ResetColor();
                return true;
            }
            Console.WriteLine("Invalid credentials.");
            return false;
        }

        private static void Register()
        {
            Console.Write("New Login: "); string email = Console.ReadLine();
            System.Collections.Generic.List<User> users = FileManager.ReadUsers();
            
            bool exists = false;
            foreach (var u in users)
            {
                if (u.Email == email)
                {
                    exists = true;
                    break;
                }
            }

            if (exists) { Console.WriteLine("User exists."); return; }

            Console.Write("New Password: "); string pass = Console.ReadLine();

            int id = FileManager.GetNextId("users.csv");
            FileManager.AppendLine("users.csv", $"{id},{email},{Security.HashPassword(pass)}");
            Console.WriteLine("Registered!");
        }

        public static void RenderIntro()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n============================================");
            Console.WriteLine("========== WELCOME TO THE AIRPORT ==========");
            Console.WriteLine("============================================");
            Console.ResetColor();
        }

        public static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\nMain Menu:");
                Console.WriteLine("1. Flight Management");
                Console.WriteLine("2. Passenger Registration");
                Console.WriteLine("3. Ticket Booking");
                Console.WriteLine("4. Flight Status");
                Console.WriteLine("5. Flight Search");
                Console.WriteLine("6. Ticket Price Calculation");
                Console.WriteLine("7. View All Stored Data");
                Console.WriteLine("0. Exit");

                Console.Write("Select option (0-7): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowFlightOperationsMenu(); break;
                    case "2": RegisterPassenger(); break;
                    case "3": BookTicket(); break;
                    case "4": AddFlightStatus(); break;
                    case "5": SearchMenu(); break;
                    case "6": CalcPrice(); break;
                    case "7": ViewAllData(); break;
                    case "0": return;
                    default: Console.WriteLine("Invalid choice!"); break;
                }
            }
        }
        
        private static void ShowFlightOperationsMenu()
        {
            while (true)
            {
                Console.WriteLine("\n--- FLIGHT MANAGEMENT ---");
                Console.WriteLine("1. Add Flight");
                Console.WriteLine("2. Show Flights");
                Console.WriteLine("3. Delete Flight");
                Console.WriteLine("4. Edit Flight");
                Console.WriteLine("5. Statistics");
                Console.WriteLine("0. Back");
                Console.Write("Choice: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Flight f = new Flight();
                        Console.Write("Number: "); f.FlightNumber = Console.ReadLine();
                        Console.Write("Departure: "); f.Departure = Console.ReadLine();
                        Console.Write("Arrival: "); f.Arrival = Console.ReadLine();
                        Console.Write("Seats: "); int.TryParse(Console.ReadLine(), out int s); f.Seats = s;
                        f.Id = FileManager.GetNextId("flights.csv");
                        FileManager.AppendLine("flights.csv", f.ToCsv());
                        break;
                    case "2": RenderFlights(FileManager.ReadFlights()); break;
                    case "3":
                        Console.Write("ID to delete: ");
                        if (int.TryParse(Console.ReadLine(), out int delId))
                        {
                            var list = FileManager.ReadFlights();
                            bool removed = false;
                            for (int i = list.Count - 1; i >= 0; i--)
                            {
                                if (list[i].Id == delId)
                                {
                                    list.RemoveAt(i);
                                    removed = true;
                                }
                            }
                            
                            if (removed)
                            {
                                FileManager.RewriteFlights(list);
                                Console.WriteLine("Deleted.");
                            }
                            else Console.WriteLine("ID not found.");
                        }
                        break;
                    case "4": EditFlight(); break;
                    case "5": ShowStats(); break;
                    case "0": return;
                }
            }
        }

        private static void RenderFlights(System.Collections.Generic.List<Flight> list)
        {
            Console.WriteLine($"\n{"ID",-3} {"Number",-10} {"Departure",-15} {"Arrival",-15} {"Seats",-5}");
            foreach (var f in list)
                Console.WriteLine($"{f.Id,-3} {f.FlightNumber,-10} {f.Departure,-15} {f.Arrival,-15} {f.Seats,-5}");
        }

        private static void EditFlight()
        {
            Console.Write("ID to edit: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var list = FileManager.ReadFlights();
                Flight f = null;
                foreach (var item in list)
                {
                    if (item.Id == id)
                    {
                        f = item;
                        break;
                    }
                }

                if (f != null)
                {
                    Console.Write($"New Departure ({f.Departure}): "); string d = Console.ReadLine();
                    if (!string.IsNullOrEmpty(d)) f.Departure = d;
                    Console.Write($"New Arrival ({f.Arrival}): "); string a = Console.ReadLine();
                    if (!string.IsNullOrEmpty(a)) f.Arrival = a;
                    FileManager.RewriteFlights(list);
                    Console.WriteLine("Updated.");
                }
                else Console.WriteLine("Flight not found.");
            }
        }

        private static void ShowStats()
        {
            var list = FileManager.ReadFlights();
            if (list.Count == 0) return;

            double sum = 0;
            int max = 0;

            foreach (var f in list)
            {
                sum += f.Seats;
                if (f.Seats > max) max = f.Seats;
            }

            double avg = sum / list.Count;

            Console.WriteLine($"Total: {list.Count}, Avg Seats: {avg:F1}, Max: {max}");
        }
        
        private static void RegisterPassenger()
        {
            Console.WriteLine("\n--- NEW PASSENGER ---");
            Passenger p = new Passenger();
            Console.Write("Name: "); p.Name = Console.ReadLine();
            Console.Write("Passport: "); p.Passport = Console.ReadLine();
            Console.Write("Age: "); int.TryParse(Console.ReadLine(), out int age); p.Age = age;

            p.Id = FileManager.GetNextId("passengers.csv");
            FileManager.AppendLine("passengers.csv", p.ToCsv());
            Console.WriteLine("Passenger Saved to CSV!");
        }
        
        private static void BookTicket()
        {
            Console.WriteLine("\n--- NEW BOOKING ---");
            RenderFlights(FileManager.ReadFlights());

            Booking b = new Booking();
            Console.Write("Enter Flight Number from list: "); b.FlightNumber = Console.ReadLine();
            Console.Write("Passenger Name: "); b.PassengerName = Console.ReadLine();

            b.Id = FileManager.GetNextId("bookings.csv");
            FileManager.AppendLine("bookings.csv", b.ToCsv());
            Console.WriteLine("Booking Saved to CSV!");
        }
        
        private static void AddFlightStatus()
        {
            Console.WriteLine("\n--- UPDATE STATUS ---");
            FlightStatus fs = new FlightStatus();
            Console.Write("Flight Number: "); fs.FlightNumber = Console.ReadLine();
            Console.Write("Status (e.g. Delayed): "); fs.Status = Console.ReadLine();
            Console.Write("Time: "); fs.Time = Console.ReadLine();

            fs.Id = FileManager.GetNextId("statuses.csv");
            FileManager.AppendLine("statuses.csv", fs.ToCsv());
            Console.WriteLine("Status Saved!");
        }
        
        private static void SearchMenu()
        {
            Console.Write("Search query (City/Number): ");
            string q = Console.ReadLine().ToLower();
            var allFlights = FileManager.ReadFlights();
            var results = new System.Collections.Generic.List<Flight>();

            foreach (var f in allFlights)
            {
                if (f.FlightNumber.ToLower().Contains(q) ||
                    f.Departure.ToLower().Contains(q) ||
                    f.Arrival.ToLower().Contains(q))
                {
                    results.Add(f);
                }
            }

            if (results.Count > 0) RenderFlights(results);
            else Console.WriteLine("No flights found.");
        }
        
        private static void CalcPrice()
        {
            Console.WriteLine("\n--- PRICE CALCULATOR ---");
            Console.Write("Enter Base Price: ");
            if (double.TryParse(Console.ReadLine(), out double basePrice))
            {
                double tax = basePrice * 0.2; 
                Console.WriteLine($"Base: {basePrice}, Tax: {tax}, TOTAL: {basePrice + tax}$");
            }
        }
        
        private static void ViewAllData()
        {
            Console.WriteLine("\n=== ALL SYSTEM DATA ===");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[FLIGHTS]");
            Console.ResetColor();
            RenderFlights(FileManager.ReadFlights());

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[PASSENGERS]");
            Console.ResetColor();
            foreach (var p in FileManager.ReadPassengers())
                Console.WriteLine($"ID: {p.Id}, Name: {p.Name}, Passport: {p.Passport}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[BOOKINGS]");
            Console.ResetColor();
            foreach (var b in FileManager.ReadBookings())
                Console.WriteLine($"ID: {b.Id}, Flight: {b.FlightNumber}, Pass: {b.PassengerName}");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n[STATUSES]");
            Console.ResetColor();
            foreach (var s in FileManager.ReadStatuses())
                Console.WriteLine($"Flight: {s.FlightNumber} -> {s.Status} at {s.Time}");
        }
    }
}