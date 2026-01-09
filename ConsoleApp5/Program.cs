using System;

namespace lab_KN_23
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
        public static List<Booking> ReadBookings() => ReadData(BookFile, Booking.FromCsv);
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
            catch { Console.WriteLine($"Помилка читання файлу {file}"); }
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
            // Встановлення кодування для коректного відображення кирилиці в консолі
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            FileManager.InitializeFiles();

            if (FileManager.ReadUsers().Count == 0)
                FileManager.AppendLine("users.csv", $"1,air@port.com,{Security.HashPassword("air12345")}");

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
                Console.WriteLine("\n=== АВТОРИЗАЦІЯ ===");
                Console.WriteLine("1. Увійти");
                Console.WriteLine("2. Зареєструватися");
                Console.WriteLine("0. Вихід");
                Console.Write("Оберіть опцію: ");
                string choice = Console.ReadLine();

                if (choice == "1") { if (Login()) break; }
                else if (choice == "2") Register();
                else if (choice == "0") Environment.Exit(0);
                else Console.WriteLine("Невірний вибір.");
            }
        }

        private static bool Login()
        {
            Console.Write("Логін: "); string email = Console.ReadLine();
            Console.Write("Пароль: "); string pass = Console.ReadLine();
            string hash = Security.HashPassword(pass);

            System.Collections.Generic.List<User> users = FileManager.ReadUsers();
            User foundUser = null;

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
                Console.WriteLine("Вхід успішний!");
                Console.ResetColor();
                return true;
            }
            Console.WriteLine("Невірні дані для входу.");
            return false;
        }

        private static void Register()
        {
            Console.Write("Новий логін: "); string email = Console.ReadLine();
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

            if (exists) { Console.WriteLine("Користувач вже існує."); return; }

            Console.Write("Новий пароль: "); string? pass = Console.ReadLine();

            int id = FileManager.GetNextId("users.csv");
            FileManager.AppendLine("users.csv", $"{id},{email},{Security.HashPassword(pass)}");
            Console.WriteLine("Реєстрація успішна!");
        }

        public static void RenderIntro()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n============================================");
            Console.WriteLine("===== ЛАСКАВО ПРОСИМО ДО АЕРОПОРТУ =====");
            Console.WriteLine("============================================");
            Console.ResetColor();
        }

        public static void ShowMainMenu()
        {
            while (true)
            {
                Console.WriteLine("\nГоловне меню:");
                Console.WriteLine("1. Управління рейсами");
                Console.WriteLine("2. Реєстрація пасажира");
                Console.WriteLine("3. Бронювання квитків");
                Console.WriteLine("4. Пошук рейсу");
                Console.WriteLine("5. Розрахунок вартості квитка");
                Console.WriteLine("6. Переглянути всі дані системи");
                Console.WriteLine("0. Вихід");

                Console.Write("Оберіть опцію (0-7): ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ShowFlightOperationsMenu(); break;
                    case "2": RegisterPassenger(); break;
                    case "3": BookTicket(); break;
                    case "4": SearchMenu(); break;
                    case "5": TicketPrice(); break;
                    case "6": ViewAllData(); break;
                    case "0": return;
                    default: Console.WriteLine("Невірний вибір!"); break;
                }
            }
        }
        
        private static void ShowFlightOperationsMenu()
        {
            while (true)
            {
                Console.WriteLine("\n----РЕЙСИ----");
                Console.WriteLine("1. Додати рейс");
                Console.WriteLine("2. Показати рейси");
                Console.WriteLine("0. Назад");
                Console.Write("Вибір: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        Flight f = new Flight();
                        Console.Write("Номер рейсу: "); f.FlightNumber = Console.ReadLine();
                        Console.Write("Звідки: "); f.Departure = Console.ReadLine();
                        Console.Write("Куди: "); f.Arrival = Console.ReadLine();
                        Console.WriteLine("Рейс додано.");
                        break;
                    case "2": 
                        // 1. Отримуємо список з файлу
                        var flights = FileManager.ReadFlights();

                        // 2. Якщо у файлі нічого немає, додаємо ваші рейси вручну
                        if (flights.Count == 0)
                        {
                            flights.Add(new Flight { Id = 1, FlightNumber = "AB100", Departure = "Kyiv", Arrival = "London", Seats = 150 });
                            flights.Add(new Flight { Id = 2, FlightNumber = "BC200", Departure = "Lviv", Arrival = "Berlin", Seats = 100 });
                            flights.Add(new Flight { Id = 3, FlightNumber = "ZA999", Departure = "Odesa", Arrival = "Paris", Seats = 200 });
                            flights.Add(new Flight { Id = 4, FlightNumber = "PS789", Departure = "Kyiv", Arrival = "Warsaw", Seats = 180 });
                            flights.Add(new Flight { Id = 5, FlightNumber = "TK124", Departure = "Istanbul", Arrival = "Kyiv", Seats = 160 });
                            flights.Add(new Flight { Id = 6, FlightNumber = "LH254", Departure = "Munich", Arrival = "Lviv", Seats = 120 });
                            flights.Add(new Flight { Id = 7, FlightNumber = "AF112", Departure = "Paris", Arrival = "Prague", Seats = 140 });

                            // (Опціонально) Зберігаємо їх у файл, щоб вони там були і наступного разу
                            FileManager.RewriteFlights(flights);
                        }

                        // 3. Відображаємо фінальний список
                        Console.WriteLine("\n--- ДОСТУПНІ РЕЙСИ ---");
                        RenderFlights(flights); 
                        break;
                       
                    case "0": return;
                }
            }
        }

        private static void RenderFlights(System.Collections.Generic.List<Flight> list)
        {
            Console.WriteLine($"\n{"ID",-3} {"Номер",-10} {"Звідки",-15} {"Куди",-15} {"Місць",-5}");
            foreach (var f in list)
                Console.WriteLine($"{f.Id,-3} {f.FlightNumber,-10} {f.Departure,-15} {f.Arrival,-15} {f.Seats,-5}");
        }

        private static void EditFlight()
        {
            Console.Write("ID рейсу для редагування: ");
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
                    Console.Write($"Новий пункт вильоту ({f.Departure}): "); string d = Console.ReadLine();
                    if (!string.IsNullOrEmpty(d)) f.Departure = d;
                    Console.Write($"Новий пункт прибуття ({f.Arrival}): "); string a = Console.ReadLine();
                    if (!string.IsNullOrEmpty(a)) f.Arrival = a;
                    FileManager.RewriteFlights(list);
                    Console.WriteLine("Оновлено.");
                }
                else Console.WriteLine("Рейс не знайдено.");
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

            Console.WriteLine($"Всього рейсів: {list.Count}, Сер. кількість місць: {avg:F1}, Макс: {max}");
        }
        
        private static void RegisterPassenger()
        {
            Console.WriteLine("\n--- НОВИЙ ПАСАЖИР ---");
            Passenger p = new Passenger();
            Console.Write("Ім'я: "); p.Name = Console.ReadLine();
            Console.Write("Паспорт: "); p.Passport = Console.ReadLine();
            Console.Write("Вік: "); int.TryParse(Console.ReadLine(), out int age); p.Age = age;

            p.Id = FileManager.GetNextId("passengers.csv");
            FileManager.AppendLine("passengers.csv", p.ToCsv());
            Console.WriteLine("Дані пасажира збережено!");
        }
        
        private static void BookTicket()
        {
            Console.WriteLine("\n--- НОВЕ БРОНЮВАННЯ ---");
            RenderFlights(FileManager.ReadFlights());

            Booking b = new Booking();
            Console.Write("Введіть номер рейсу зі списку: "); b.FlightNumber = Console.ReadLine();
            Console.Write("Ім'я пасажира: "); b.PassengerName = Console.ReadLine();

            b.Id = FileManager.GetNextId("bookings.csv");
            FileManager.AppendLine("bookings.csv", b.ToCsv());
            Console.WriteLine("Бронювання успішне!");
        }
        private static void SearchMenu()
        {
            Console.Write("Пошуковий запит (Місто/Номер): ");
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
            else Console.WriteLine("Рейсів не знайдено.");
        }
        
        private static void TicketPrice()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Розрахунок вартості квитка");
            Console.ResetColor();

            Console.Write("Оберіть точку А: ");
            string cityA = Console.ReadLine();

            Console.Write("Оберіть точку Б: ");
            string cityB = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Ваш рейс: {cityA} - {cityB}");
            Console.ResetColor();

            Console.WriteLine("\nКласи на борту:");
            Console.WriteLine("1. Економ клас");
            Console.WriteLine("2. Комфорт клас");
            Console.WriteLine("3. Бізнес клас");
            Console.Write("Оберіть клас перельоту: ");
            if (!int.TryParse(Console.ReadLine(), out int classChoice)) classChoice = 1;

            Console.Write("Введіть ваш вік: ");
            if (!int.TryParse(Console.ReadLine(), out int agePassenger)) agePassenger = 25;

            Console.Write("Оберіть кількість квитків: ");
            if (!int.TryParse(Console.ReadLine(), out int tickets)) tickets = 1;

            double basePrice = 100;

            double classMultiplier = 1;
            if (classChoice == 2) classMultiplier = 1.5;
            else if (classChoice == 3) classMultiplier = 2;

            double agePrice = 1;
            if (agePassenger < 5) agePrice = 0.5;
            else if (agePassenger >= 5 && agePassenger < 18) agePrice = 0.7;
            
            double finalPrice = basePrice * classMultiplier * agePrice * tickets;
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"Загальна вартість: ${finalPrice}");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Дякуємо за покупку!");
            Console.ResetColor();
        }
        
        private static void ViewAllData()
        {
            
            
                Console.WriteLine("\n=== ВСІ ДАНІ СИСТЕМИ ===");
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n[ПАСАЖИРИ]");
                Console.ResetColor();
                foreach (var p in FileManager.ReadPassengers())
                    Console.WriteLine($"ID: {p.Id}, Ім'я: {p.Name}, Паспорт: {p.Passport}, Вік: {p.Age}");

            
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n[БРОНЮВАННЯ]");
                Console.ResetColor();
                
            
            
        }
    }
}