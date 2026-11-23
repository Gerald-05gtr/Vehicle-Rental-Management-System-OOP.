namespace VehicleRental;

/// <summary>Bike rentals receive a 10% discount for bookings of seven days or more.</summary>
public sealed class Bike : Vehicle
{
    public bool HasElectricAssist { get; }
    public Bike(string brand, string model, decimal dailyRate, bool hasElectricAssist)
        : base(brand, model, dailyRate) => HasElectricAssist = hasElectricAssist;
    public override string VehicleType => "Bike";
    public override decimal CalculateRentalFee(int days)
    {
        ValidateDays(days);
        decimal fee = DailyRate * days;
        return days >= 7 ? fee * 0.90m : fee;
    }
}
