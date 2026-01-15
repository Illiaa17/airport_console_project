namespace lab_KN_23;
/// <summary>
/// Модель бронювання квитка.
/// </summary>
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