using System;

[Serializable]
public class Bus
{
    public Guid Id { get; set; }
    public int Seats { get; set; }
    public int Year { get; set; }
    public string Model { get; set; }

    public static Bus CreateNew()
    {
        var rnd = new Random(Guid.NewGuid().GetHashCode());
        return new Bus
        {
            Id = Guid.NewGuid(),
            Seats = rnd.Next(20, 60),
            Year = rnd.Next(2000, 2025),
            Model = "Model" + rnd.Next(1, 100)
        };
    }
}
