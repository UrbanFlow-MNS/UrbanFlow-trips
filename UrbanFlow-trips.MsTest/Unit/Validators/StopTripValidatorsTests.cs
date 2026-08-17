using Moq;
using UrbanFlow_trips.Application.DTO.StopTrip;
using UrbanFlow_trips.Application.Validators;
using UrbanFlow_trips.Domain.Interfaces;

namespace UrbanFlow_trips.MsTest.Unit.Validators;

[TestClass]
public class CreateStopTripDtoValidatorTests
{
    private readonly CreateStopTripDtoValidator _validator = new(Mock.Of<IStopRepository>());

    [TestMethod]
    public void Validate_DepartureBeforeArrival_IsValid()
    {
        var dto = new CreateStopTripDto
        {
            TripId = 1,
            StopId = 1,
            StopSequence = 1,
            ArrivalTime = new TimeOnly(8, 0),
            DepartureTime = new TimeOnly(7, 55)
        };
        Assert.IsTrue(_validator.Validate(dto).IsValid);
    }

    [TestMethod]
    public void Validate_DepartureAfterArrival_IsInvalid()
    {
        var dto = new CreateStopTripDto
        {
            ArrivalTime = new TimeOnly(8, 0),
            DepartureTime = new TimeOnly(8, 5)
        };
        var result = _validator.Validate(dto);
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual("Departure time must be before arrival time", result.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Validate_NullDeparture_IsValid()
    {
        var dto = new CreateStopTripDto
        {
            ArrivalTime = new TimeOnly(8, 0),
            DepartureTime = null
        };
        Assert.IsTrue(_validator.Validate(dto).IsValid);
    }
}

[TestClass]
public class UpdateStopTripDtoValidatorTests
{
    private readonly UpdateStopTripDtoValidator _validator = new();

    [TestMethod]
    public void Validate_DepartureBeforeArrival_IsValid()
    {
        var dto = new UpdateStopTripDto
        {
            ArrivalTime = new TimeOnly(9, 30),
            DepartureTime = new TimeOnly(9, 15)
        };
        Assert.IsTrue(_validator.Validate(dto).IsValid);
    }

    [TestMethod]
    public void Validate_DepartureAfterArrival_IsInvalid()
    {
        var dto = new UpdateStopTripDto
        {
            ArrivalTime = new TimeOnly(9, 0),
            DepartureTime = new TimeOnly(9, 30)
        };
        var result = _validator.Validate(dto);
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual("Departure time must be before arrival time", result.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Validate_NullDeparture_IsValid()
    {
        var dto = new UpdateStopTripDto
        {
            ArrivalTime = new TimeOnly(9, 0),
            DepartureTime = null
        };
        Assert.IsTrue(_validator.Validate(dto).IsValid);
    }
}
