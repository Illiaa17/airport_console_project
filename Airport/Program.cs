// Copyright (c) AirportManager. All rights reserved.
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace lab_KN_23

{
    /// <summary>
    /// Клас для роботи з файловою системою та збереження даних.
    /// </summary>
    public static class FileManager
    {
        private const string FlightsFile = "flights.csv";
        private const string UsersFile = "users.csv";
        private const string PassFile = "passengers.csv";
        private const string BookFile = "bookings.csv";
        private const string AddFlightsFile = "addflights.csv";
       
        /// <summary>
        /// Створює необхідні CSV файли, якщо вони відсутні.
        /// </summary>
        public static void InitializeFiles()
        {
            CreateIfNotExists(FlightsFile, "Id,FlightNumber,Departure,Arrival,Seats");
            CreateIfNotExists(UsersFile, "Id,Email,PasswordHash");
            CreateIfNotExists(PassFile, "Id,Name,Passport,Age");
            CreateIfNotExists(BookFile, "Id,FlightNumber,PassengerName");
            CreateIfNotExists(AddFlightsFile, "Id,FlightNumber,Departure,Arrival,Seats");
        }
        
        private static void CreateIfNotExists(string path, string header)
        {
            if (!System.IO.File.Exists(path)) 
                File.WriteAllText(path, header + Environment.NewLine);
        }

        /// <summary>
        /// Знаходить наступний доступний ID у файлі.
        /// </summary>
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
        
        /// <summary>
        /// Читає список рейсів з файлу.
        /// </summary>
        public static System.Collections.Generic.List<Flight> ReadFlights() => ReadData(FlightsFile, Flight.FromCsv);
        /// <summary>
        /// Читає список доданих рейсів з файлу.
        /// </summary>
        public static System.Collections.Generic.List<Flight> ReadAddedFlights() => ReadData(AddFlightsFile, Flight.FromCsv);
        /// <summary>
        /// Читає список користувачів.
        /// </summary>
        public static System.Collections.Generic.List<User> ReadUsers() => ReadData(UsersFile, line => {
            var v = line.Split(',');
            return new User { Id = int.Parse(v[0]), Email = v[1].Trim(), PasswordHash = v[2].Trim() };
        });

        public static System.Collections.Generic.List<Passenger> ReadPassengers() => ReadData(PassFile, Passenger.FromCsv);
        public static List<Booking> ReadBookings() => ReadData(BookFile, Booking.FromCsv);
        
        
        /// <summary>
        /// Універсальний метод для читання даних з CSV.
        /// </summary>
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

        /// <summary>
        /// Перезаписує файл рейсів актуальними даними.
        /// </summary>
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
    
    /// <summary>
    /// Забезпечує безпеку та хешування паролів.
    /// </summary>
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
    
    /// <summary>
    /// Головний клас програми для керування аеропортом.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Поточний авторизований користувач.
        /// </summary>
        static User currentUser = null;

        /// <summary>
        /// Точка входу в програму.
        /// </summary>
        public static void Main(string[] args)
        {
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

        /// <summary>
        /// Система авторизації та реєстрації.
        /// </summary>
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

        /// <summary>
        /// Метод для входу в систему.
        /// </summary>
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

        /// <summary>
        /// Реєстрація нового користувача.
        /// </summary>
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

        /// <summary>
        /// Відображає вітальне повідомлення.
        /// </summary>
        public static void RenderIntro()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("\n============================================");
            Console.WriteLine("===== ЛАСКАВО ПРОСИМО ДО АЕРОПОРТУ =====");
            Console.WriteLine("============================================");
            Console.ResetColor();
        }

        /// <summary>
        /// Головне меню управління.
        /// </summary>
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
        
        /// <summary>
        /// Меню операцій над рейсами.
        /// </summary>
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
                        Console.Write("Кількість місць: "); 
                        int.TryParse(Console.ReadLine(), out int seats); f.Seats = seats;
                        f.Id = FileManager.GetNextId("addflights.csv");
                        FileManager.AppendLine("addflights.csv", f.ToCsv());
                        
                        Console.WriteLine("Рейс додано.");
                        break;
                    case "2": 
                        
                        var systemFlights = FileManager.ReadFlights();
                        if (systemFlights.Count == 0)
                            
                        {
                            systemFlights.Add(new Flight { Id = 1, FlightNumber = "AB100", Departure = "Kyiv", Arrival = "London", Seats = 150 });
                            systemFlights.Add(new Flight { Id = 2, FlightNumber = "BC200", Departure = "Lviv", Arrival = "Berlin", Seats = 100 });
                            systemFlights.Add(new Flight { Id = 3, FlightNumber = "ZA999", Departure = "Odesa", Arrival = "Paris", Seats = 200 });
                            systemFlights.Add(new Flight { Id = 4, FlightNumber = "PS789", Departure = "Kyiv", Arrival = "Warsaw", Seats = 180 });
                            systemFlights.Add(new Flight { Id = 5, FlightNumber = "TK124", Departure = "Istanbul", Arrival = "Kyiv", Seats = 160 });
                            systemFlights.Add(new Flight { Id = 6, FlightNumber = "LH254", Departure = "Munich", Arrival = "Lviv", Seats = 120 });
                            systemFlights.Add(new Flight { Id = 7, FlightNumber = "AF112", Departure = "Paris", Arrival = "Prague", Seats = 140 });
                            
                            FileManager.RewriteFlights(systemFlights);
                        }
                        Console.WriteLine("\n=== СИСТЕМНІ РЕЙСИ ===");
                        RenderFlights(systemFlights);

                        // 2. Обробка доданих рейсів
                        var userFlights = FileManager.ReadAddedFlights();
                
                        Console.WriteLine("\n=== ДОДАНІ ВАМИ РЕЙСИ ===");
                        if (userFlights.Count > 0)
                        {
                            RenderFlights(userFlights);
                        }
                        else
                        {
                            Console.WriteLine("Додані рейси відсутні.");
                        }
                        break; 

                    case "0": return; 
                    default: Console.WriteLine("Невірний вибір."); break;
                        
                    
                }
            }
        }

        /// <summary>
        /// Виводить список рейсів у табличному форматі.
        /// </summary>
        private static void RenderFlights(System.Collections.Generic.List<Flight> list)
        {
            Console.WriteLine($"\n{"ID",-3} {"Номер",-10} {"Звідки",-15} {"Куди",-15} {"Місць",-5}");
            foreach (var f in list)
                Console.WriteLine($"{f.Id,-3} {f.FlightNumber,-10} {f.Departure,-15} {f.Arrival,-15} {f.Seats,-5}");
        }
        
        /// <summary>
        /// Реєстрація нового пасажира.
        /// </summary>
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
        
        /// <summary>
        /// Створення нового бронювання.
        /// </summary>
        private static void BookTicket()
        {
            Console.WriteLine("\n--- НОВЕ БРОНЮВАННЯ ---");
            
            var flights = FileManager.ReadFlights();
            
            if (flights.Count == 0)
            {
                flights.Add(new Flight { Id = 1, FlightNumber = "AB100", Departure = "Kyiv", Arrival = "London", Seats = 150 });
                flights.Add(new Flight { Id = 2, FlightNumber = "BC200", Departure = "Lviv", Arrival = "Berlin", Seats = 100 });
                flights.Add(new Flight { Id = 3, FlightNumber = "ZA999", Departure = "Odesa", Arrival = "Paris", Seats = 200 });
                flights.Add(new Flight { Id = 4, FlightNumber = "PS789", Departure = "Kyiv", Arrival = "Warsaw", Seats = 180 });
                flights.Add(new Flight { Id = 5, FlightNumber = "TK124", Departure = "Istanbul", Arrival = "Kyiv", Seats = 160 });
                flights.Add(new Flight { Id = 6, FlightNumber = "LH254", Departure = "Munich", Arrival = "Lviv", Seats = 120 });
                flights.Add(new Flight { Id = 7, FlightNumber = "AF112", Departure = "Paris", Arrival = "Prague", Seats = 140 });

                FileManager.RewriteFlights(flights);
            }
            
            RenderFlights(flights);

            Booking b = new Booking();
            Console.Write("\nВведіть номер рейсу зі списку: "); 
            b.FlightNumber = Console.ReadLine();
    
            Console.Write("Ім'я пасажира: "); 
            b.PassengerName = Console.ReadLine();

            b.Id = FileManager.GetNextId("bookings.csv");
            FileManager.AppendLine("bookings.csv", b.ToCsv());
    
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Бронювання успішне!");
            Console.ResetColor();
        }
        
        /// <summary>
        /// Пошук рейсів за містом або номером.
        /// </summary>
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
        
        /// <summary>
        /// Калькулятор вартості квитка з урахуванням класу та віку.
        /// </summary>
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
        
        /// <summary>
        /// Перегляд усіх збережених даних у системі.
        /// </summary>
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