namespace VehicleRental;

/// <summary>Scooter rentals have a fixed 5 PLN safety-kit fee per booking.</summary>
public sealed class Scooter : Vehicle
{
    public int BatteryRangeKm { get; }
    public Scooter(string brand, string model, decimal dailyRate, int batteryRangeKm)
        : base(brand, model, dailyRate)
    {
        if (batteryRangeKm <= 0) throw new ArgumentOutOfRangeException(nameof(batteryRangeKm));
        BatteryRangeKm = batteryRangeKm;
    }
    public override string VehicleType => "Scooter";
    public override decimal CalculateRentalFee(int days)
    {
        ValidateDays(days);
        return DailyRate * days + 5m;
    }
}
