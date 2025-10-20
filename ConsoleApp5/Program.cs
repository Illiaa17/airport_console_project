namespace lab1_KN_23;

class Program
{
   static void Main(string[] args)
   {
      Console.ForegroundColor = ConsoleColor.Green;
      Console.WriteLine("Welcome to the airport");
      Console.ResetColor();
         
      Console.Write("Select a point A:");
      string cityA = Console.ReadLine();
         
      Console.Write("Select a point B:");
      string cityB = Console.ReadLine();
         
      Console.ForegroundColor = ConsoleColor.Yellow;
      Console.WriteLine($"Your flight: {cityA} - {cityB}");
      Console.ResetColor();
         
      Console.WriteLine();
      Console.WriteLine("Classes onboard:");
      Console.WriteLine("1. Economy class");
      Console.WriteLine("2. Comfort class");
      Console.WriteLine("3. Bussiness class");
      Console.Write("Select a flight class:");
      int classChoice = int.Parse(Console.ReadLine());
       
      Console.Write("Enter your age:");
      int agePassenger = int.Parse(Console.ReadLine());
         
      Console.Write("Select the number of tickets:");
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
      Console.WriteLine("Thank you for your purchases!");
      Console.ResetColor();
   }











}

