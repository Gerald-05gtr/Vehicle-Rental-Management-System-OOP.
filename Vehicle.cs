namespace VehicleRental;

/// <summary>Shared contract and protected availability state for all rentable vehicles.</summary>
public abstract class Vehicle
{
    public Guid Id { get; }
    public string Brand { get; }
    public string Model { get; }
    public decimal DailyRate { get; }
    public bool IsAvailable { get; private set; } = true;

    protected Vehicle(string brand, string model, decimal dailyRate)
    {
        if (string.IsNullOrWhiteSpace(brand)) throw new ArgumentException("Brand is required.", nameof(brand));
        if (string.IsNullOrWhiteSpace(model)) throw new ArgumentException("Model is required.", nameof(model));
        if (dailyRate <= 0) throw new ArgumentOutOfRangeException(nameof(dailyRate));
        Id = Guid.NewGuid();
        Brand = brand.Trim();
        Model = model.Trim();
        DailyRate = dailyRate;
    }

    public abstract string VehicleType { get; }
    public abstract decimal CalculateRentalFee(int days);

    public void Rent()
    {
        if (!IsAvailable) throw new InvalidOperationException("Vehicle is already rented.");
        IsAvailable = false;
    }

    public void ReturnVehicle()
    {
        if (IsAvailable) throw new InvalidOperationException("Vehicle is not rented.");
        IsAvailable = true;
    }

    protected static void ValidateDays(int days)
    {
        if (days <= 0) throw new ArgumentOutOfRangeException(nameof(days), "Days must be positive.");
    }

    public override string ToString() => $"{VehicleType}: {Brand} {Model} | {DailyRate:0.00} PLN/day | {(IsAvailable ? "Available" : "Rented")}";
}
