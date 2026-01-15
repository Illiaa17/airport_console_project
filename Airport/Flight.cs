namespace lab_KN_23;

/// <summary>
/// Представляє інформацію про авіарейс.
/// </summary>
public class Flight
{
    public int Id { get; set; }
    public string FlightNumber { get; set; }
    public string? Departure { get; set; }
    public string Arrival { get; set; }
    public int Seats { get; set; }
        
    /// <summary>
    /// Конвертує об'єкт рейсу в рядок CSV.
    /// </summary>
    public string ToCsv() => $"{Id},{FlightNumber},{Departure},{Arrival},{Seats}";

    /// <summary>
    /// Створює об'єкт рейсу з рядка CSV.
    /// </summary>
    public static Flight FromCsv(string csvLine)
    {
        string[] v = csvLine.Split(',');
        return new Flight { Id = int.Parse(v[0]), FlightNumber = v[1], Departure = v[2], Arrival = v[3], Seats = int.Parse(v[4]) };
    }
}