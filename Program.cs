using System.Globalization;
using VehicleRental;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

if (args.Contains("--test", StringComparer.OrdinalIgnoreCase))
{
    RunSelfTests();
    return;
}

var manager = new RentalManager();
Vehicle car = new Car("Toyota", "Corolla", 120m, 5);
Vehicle bike = new Bike("Giant", "Escape 3", 30m, false);
Vehicle scooter = new Scooter("Xiaomi", "Mi Electric", 45m, 30);
Customer customer = new("Anna Kowalska", "anna@example.com");

foreach (Vehicle vehicle in new[] { car, bike, scooter }) manager.RegisterVehicle(vehicle);
manager.RegisterCustomer(customer);

Console.WriteLine("=== VEHICLE RENTAL MANAGEMENT SYSTEM ===");
Console.WriteLine("Available vehicles and polymorphic 3-day fees:");
foreach (Vehicle vehicle in manager.GetAvailableVehicles())
    Console.WriteLine($"  {vehicle} | 3-day fee: {vehicle.CalculateRentalFee(3):0.00} PLN");

Rental rental = manager.RentVehicle(customer.Id, car.Id, 3);
Console.WriteLine($"\nCreated rental {rental.Id} for {customer.FullName}");
Console.WriteLine($"Total fee: {rental.TotalFee:0.00} PLN | Car available: {car.IsAvailable}");

try { manager.RentVehicle(customer.Id, car.Id, 1); }
catch (InvalidOperationException ex) { Console.WriteLine($"Expected duplicate-rental rejection: {ex.Message}"); }

manager.ReturnVehicle(rental.Id);
Console.WriteLine($"Returned rental | Car available: {car.IsAvailable} | Rental active: {rental.IsActive}");
Console.WriteLine($"Customer rental history count: {customer.RentalHistory.Count}");
Console.WriteLine("Demo completed successfully.");

static void RunSelfTests()
{
    int passed = 0;
    void Check(bool condition, string label)
    {
        if (!condition) throw new Exception($"FAILED: {label}");
        Console.WriteLine($"PASS: {label}");
        passed++;
    }
    static bool Throws<T>(Action action) where T : Exception
    {
        try { action(); return false; }
        catch (T) { return true; }
    }

    var manager = new RentalManager();
    Vehicle car = new Car("Toyota", "Corolla", 120m, 5);
    Vehicle bike = new Bike("Giant", "Escape", 30m, false);
    Vehicle scooter = new Scooter("Xiaomi", "Mi", 45m, 30);
    Customer customer = new("Test Customer", "test@example.com");
    manager.RegisterVehicle(car);
    manager.RegisterVehicle(bike);
    manager.RegisterVehicle(scooter);
    manager.RegisterCustomer(customer);
    Check(car.CalculateRentalFee(3) == 380m, "Car overrides pricing (360 + 20)");
    Check(bike.CalculateRentalFee(7) == 189m, "Bike applies 7-day discount");
    Check(scooter.CalculateRentalFee(3) == 140m, "Scooter includes safety kit");
    Check(Throws<ArgumentOutOfRangeException>(() => car.CalculateRentalFee(0)), "Invalid days rejected");
    Check(Throws<ArgumentException>(() => new Customer("", "test@example.com")), "Invalid customer rejected");
    Check(Throws<InvalidOperationException>(() => manager.RegisterVehicle(car)), "Duplicate registration rejected");
    Rental rental = manager.RentVehicle(customer.Id, car.Id, 3);
    Check(!car.IsAvailable && rental.IsActive, "Renting changes availability");
    Check(customer.RentalHistory.Count == 1, "Rental saved in customer history");
    Check(Throws<InvalidOperationException>(() => manager.RentVehicle(customer.Id, car.Id, 1)), "Double rental rejected");
    Check(Throws<ArgumentOutOfRangeException>(() => manager.RentVehicle(customer.Id, bike.Id, 0)), "Zero-day rental rejected");
    Check(Throws<KeyNotFoundException>(() => manager.RentVehicle(Guid.NewGuid(), bike.Id, 1)), "Unknown customer rejected");
    manager.ReturnVehicle(rental.Id);
    Check(car.IsAvailable && !rental.IsActive, "Return releases vehicle");
    Check(Throws<InvalidOperationException>(() => manager.ReturnVehicle(rental.Id)), "Double return rejected");
    Check(manager.GetAvailableVehicles().Count() == 3, "All three available after return");
    Console.WriteLine($"All {passed} self-tests passed.");
}
