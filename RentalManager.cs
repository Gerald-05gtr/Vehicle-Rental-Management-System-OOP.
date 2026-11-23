namespace VehicleRental;

/// <summary>Coordinates registrations, bookings, and returns; business state remains encapsulated.</summary>
public sealed class RentalManager
{
    private readonly List<Vehicle> _vehicles = new();
    private readonly List<Customer> _customers = new();
    private readonly List<Rental> _rentals = new();

    public IReadOnlyList<Vehicle> Vehicles => _vehicles.AsReadOnly();
    public IReadOnlyList<Customer> Customers => _customers.AsReadOnly();
    public IReadOnlyList<Rental> Rentals => _rentals.AsReadOnly();

    public void RegisterVehicle(Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
        if (_vehicles.Any(v => v.Id == vehicle.Id)) throw new InvalidOperationException("Vehicle already registered.");
        _vehicles.Add(vehicle);
    }

    public void RegisterCustomer(Customer customer)
    {
        ArgumentNullException.ThrowIfNull(customer);
        if (_customers.Any(c => c.Id == customer.Id)) throw new InvalidOperationException("Customer already registered.");
        _customers.Add(customer);
    }

    public Rental RentVehicle(Guid customerId, Guid vehicleId, int days)
    {
        if (days <= 0) throw new ArgumentOutOfRangeException(nameof(days));
        Customer customer = _customers.SingleOrDefault(c => c.Id == customerId)
            ?? throw new KeyNotFoundException("Customer is not registered.");
        Vehicle vehicle = _vehicles.SingleOrDefault(v => v.Id == vehicleId)
            ?? throw new KeyNotFoundException("Vehicle is not registered.");
        if (!vehicle.IsAvailable) throw new InvalidOperationException("Vehicle is already rented.");

        // Calculate and construct before altering vehicle availability.
        Rental rental = new(customer, vehicle, days, DateTime.UtcNow);
        vehicle.Rent();
        _rentals.Add(rental);
        customer.RecordRental(rental);
        return rental;
    }

    public void ReturnVehicle(Guid rentalId)
    {
        Rental rental = _rentals.SingleOrDefault(r => r.Id == rentalId)
            ?? throw new KeyNotFoundException("Rental does not exist.");
        if (!rental.IsActive) throw new InvalidOperationException("Rental already returned.");
        rental.Vehicle.ReturnVehicle();
        rental.Close(DateTime.UtcNow);
    }

    public IEnumerable<Vehicle> GetAvailableVehicles() => _vehicles.Where(v => v.IsAvailable);
}
