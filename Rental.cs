namespace VehicleRental;

/// <summary>One booking associates exactly one customer with one vehicle.</summary>
public sealed class Rental
{
    public Guid Id { get; } = Guid.NewGuid();
    public Customer Customer { get; }
    public Vehicle Vehicle { get; }
    public int Days { get; }
    public DateTime StartDateUtc { get; }
    public DateTime? ReturnedAtUtc { get; private set; }
    public bool IsActive => ReturnedAtUtc is null;
    public decimal TotalFee { get; }

    internal Rental(Customer customer, Vehicle vehicle, int days, DateTime startDateUtc)
    {
        if (days <= 0) throw new ArgumentOutOfRangeException(nameof(days));
        Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        Vehicle = vehicle ?? throw new ArgumentNullException(nameof(vehicle));
        Days = days;
        StartDateUtc = startDateUtc;
        TotalFee = vehicle.CalculateRentalFee(days);
    }

    internal void Close(DateTime returnedAtUtc)
    {
        if (!IsActive) throw new InvalidOperationException("Rental has already been closed.");
        ReturnedAtUtc = returnedAtUtc;
    }
}
