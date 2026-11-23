namespace VehicleRental;

/// <summary>Customer identity and read-only rental history.</summary>
public sealed class Customer
{
    private readonly List<Rental> _rentalHistory = new();
    public Guid Id { get; } = Guid.NewGuid();
    public string FullName { get; }
    public string Email { get; }
    public IReadOnlyList<Rental> RentalHistory => _rentalHistory.AsReadOnly();

    public Customer(string fullName, string email)
    {
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Name is required.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Valid email is required.", nameof(email));
        FullName = fullName.Trim();
        Email = email.Trim();
    }

    internal void RecordRental(Rental rental) => _rentalHistory.Add(rental);
}
