namespace lab_KN_23;
/// <summary>
/// Представляє дані пасажира.
/// </summary>
public class Passenger
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Passport { get; set; }
    public int Age { get; set; }

    public string ToCsv() => $"{Id},{Name},{Passport},{Age}";

    public static Passenger FromCsv(string csvLine)
    {
        string[] v = csvLine.Split(',');
        return new Passenger { Id = int.Parse(v[0]), Name = v[1], Passport = v[2], Age = int.Parse(v[3]) };
    }
}