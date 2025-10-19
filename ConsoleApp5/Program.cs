namespace lab1_KN_23;

class Program
{
   static void Main(string[] args)
   {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Welcome to the airport");
      Console.ResetColor();
         
      Console.Write("Виберіть точку А:");
      string cityA = Console.ReadLine();
         
      Console.Write("Виберіть точку B:");
      string cityB = Console.ReadLine();
         
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine($"Ваш рейс: {cityA} - {cityB}");
      Console.ResetColor();
         
      Console.WriteLine();
      Console.WriteLine("Класи польоту:");
      Console.WriteLine("1. Економ");
      Console.WriteLine("2. Середній");
      Console.WriteLine("3. Бізнес");
      Console.Write("Оберіть клас польоту:");
      int classChoice = int.Parse(Console.ReadLine());
       
      Console.Write("Вкажіть ваш вік:");
      int agePassenger = int.Parse(Console.ReadLine());
         
      Console.Write("Кількість білетів:");
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
      Console.WriteLine($"Загальна ціна: ${finalPrice}");
   }











}

