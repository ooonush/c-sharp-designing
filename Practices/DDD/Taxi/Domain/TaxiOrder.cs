using System;
using System.Globalization;
using System.Linq;
using Ddd.Infrastructure;

namespace Ddd.Taxi.Domain
{
	// In real aplication it whould be the place where database is used to find driver by its Id.
	// But in this exercise it is just a mock to simulate database
	public class DriversRepository
	{
		public Driver GetDriverById(int driverId)
		{
			switch (driverId)
			{
				case 15:
				{
					var personName = new PersonName("Drive", "Driverson");
					var car = new Car("Lada sedan", "Baklazhan", "A123BT 66");
					return new Driver(driverId, personName, car);
				}
				default:
					throw new Exception("Unknown driver id " + driverId);
			}
		}
	}

	public class TaxiApi : ITaxiApi<TaxiOrder>
	{
		private readonly DriversRepository _driversRepo;
		private readonly Func<DateTime> _currentTime;
		private int _idCounter;

		public TaxiApi(DriversRepository driversRepo, Func<DateTime> currentTime)
		{
			_driversRepo = driversRepo;
			_currentTime = currentTime;
		}

		public TaxiOrder CreateOrderWithoutDestination(string firstName, string lastName, string street, string building)
		{
			var clientName = new PersonName(firstName, lastName);
			var startAddress = new Address(street, building);
			return new TaxiOrder(_idCounter++, clientName, startAddress, _currentTime());
		}

		public void UpdateDestination(TaxiOrder order, string street, string building) 
			=> order.UpdateDestination(new Address(street, building));

		public void AssignDriver(TaxiOrder order, int driverId) 
			=> order.AssignDriver(_driversRepo.GetDriverById(driverId), _currentTime());

		public void UnassignDriver(TaxiOrder order) => order.UnassignDriver();

		public string GetDriverFullInfo(TaxiOrder order) => order.GetDriverFullInfo();

		public string GetShortOrderInfo(TaxiOrder order) => order.GetShortOrderInfo();
		
		public void Cancel(TaxiOrder order) => order.Cancel(_currentTime());

		public void StartRide(TaxiOrder order) => order.StartRide(_currentTime());

		public void FinishRide(TaxiOrder order) => order.FinishRide(_currentTime());
	}

	public class TaxiOrder : Entity<int>
	{
		public PersonName ClientName { get; private set; }
		public Address Start { get; private set; }
		public Address Destination { get; private set; }
		public Driver Driver { get; private set; }
		public TaxiOrderStatus Status { get; private set; }
		public DateTime CreationTime { get; private set; }
		public DateTime DriverAssignmentTime { get; private set; }
		public DateTime CancelTime { get; private set; }
		public DateTime StartRideTime { get; private set; }
		public DateTime FinishRideTime { get; private set; }
		
		public TaxiOrder(int id, PersonName clientName, Address start, DateTime creationTime) : base(id)
		{
			ClientName = clientName;
			Start = start;
			CreationTime = creationTime;
		}

		public void UpdateDestination(Address destinationAddress)
		{
			Destination = destinationAddress;
		}

		public void AssignDriver(Driver driver, DateTime assignmentTime)
		{
			if (!Driver.NullOrEmpty(Driver))
				throw new InvalidOperationException("Driver allredy assigned");

			Driver = driver;
			DriverAssignmentTime = assignmentTime;
			Status = TaxiOrderStatus.WaitingCarArrival;
		}

		public void UnassignDriver()
		{
			if (Status != TaxiOrderStatus.WaitingCarArrival)
				throw new InvalidOperationException("WaitingForDriver");

			Driver = Driver.Empty;
			Status = TaxiOrderStatus.WaitingForDriver;
		}

		public string GetDriverFullInfo()
		{
			if (Status == TaxiOrderStatus.WaitingForDriver) return null;
			return string.Join(" ",
				"Id: " + Driver.Id,
				"DriverName: " + FormatName(Driver.PersonName.FirstName, Driver.PersonName.LastName),
				"Color: " + Driver.Car.Color,
				"CarModel: " + Driver.Car.Model,
				"PlateNumber: " + Driver.Car.PlateNumber);
		}
		
		public string GetShortOrderInfo()
		{
			return string.Join(" ",
				"OrderId: " + Id,
				"Status: " + Status,
				"Client: " + FormatName(ClientName?.FirstName, ClientName?.LastName),
				"Driver: " + FormatName(Driver?.PersonName?.FirstName, Driver?.PersonName?.LastName),
				"From: " + FormatAddress(Start?.Street, Start?.Building),
				"To: " + FormatAddress(Destination?.Street, Destination?.Building),
				"LastProgressTime: " + GetLastProgressTime()
					.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		}

		public DateTime GetLastProgressTime()
		{
			switch (Status)
			{
				case TaxiOrderStatus.WaitingForDriver:
					return CreationTime;
				case TaxiOrderStatus.WaitingCarArrival:
					return DriverAssignmentTime;
				case TaxiOrderStatus.InProgress:
					return StartRideTime;
				case TaxiOrderStatus.Finished:
					return FinishRideTime;
				case TaxiOrderStatus.Canceled:
					return CancelTime;
				default:
					throw new NotSupportedException(Status.ToString());
			}
		}

		public void Cancel(DateTime cancelTime)
		{
			if (Status != TaxiOrderStatus.WaitingForDriver) 
				throw new InvalidOperationException("Can't cancel ride an order that without waiting for driver");
			
			Status = TaxiOrderStatus.Canceled;
			CancelTime = cancelTime;
		}

		public void StartRide(DateTime startTime)
		{
			if (Status != TaxiOrderStatus.WaitingCarArrival) 
				throw new InvalidOperationException("Can't start ride an order that without waiting car arrival");

			Status = TaxiOrderStatus.InProgress;
			StartRideTime = startTime;
		}

		public void FinishRide(DateTime finishTime)
		{
			if (Status != TaxiOrderStatus.InProgress) 
				throw new InvalidOperationException("Can't finish an order that is not started");

			Status = TaxiOrderStatus.Finished;
			FinishRideTime = finishTime;
		}

		private string FormatName(string firstName, string lastName)
		{
			return string.Join(" ", new[] { firstName, lastName }.Where(n => n != null));
		}

		private string FormatAddress(string street, string building)
		{
			return string.Join(" ", new[] { street, building }.Where(n => n != null));
		}
	}

	public class Car : ValueType<Car>
	{
		public static readonly Car Empty = new Car(null, null, null);
		
		public readonly string Color;
		public readonly string Model;
		public readonly string PlateNumber;
		
		public Car(string model, string color, string plateNumber)
		{
			Color = color;
			Model = model;
			PlateNumber = plateNumber;
		}
	}
	
	public class Driver : Entity<int>
	{
		public static readonly Driver Empty = new Driver(-1, new PersonName(null, null), Car.Empty);
		
		public readonly PersonName PersonName;
		public readonly Car Car;
		
		public Driver(int id, PersonName personName, Car car) : base(id)
		{
			PersonName = personName;
			Car = car;
		}
		
		public static bool NullOrEmpty(Driver driver)
		{
			return driver == null || driver.Equals(Empty);
		}
	}
}