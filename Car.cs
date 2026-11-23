namespace VehicleRental;

/// <summary>Car rentals include a fixed 20 PLN cleaning charge per booking.</summary>
public sealed class Car : Vehicle
{
    public int Seats { get; }
    public Car(string brand, string model, decimal dailyRate, int seats) : base(brand, model, dailyRate)
    {
        if (seats < 2) throw new ArgumentOutOfRangeException(nameof(seats));
        Seats = seats;
    }
    public override string VehicleType => "Car";
    public override decimal CalculateRentalFee(int days)
    {
        ValidateDays(days);
        return DailyRate * days + 20m;
    }
}
