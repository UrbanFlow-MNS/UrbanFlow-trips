using UrbanFlow_trips.Application.DTO.Stop;
using UrbanFlow_trips.Application.Validators;

namespace UrbanFlow_trips.MsTest.Unit.Validators;

[TestClass]
public class CreateStopDtoValidatorTests
{
    private readonly CreateStopDtoValidator _validator = new();

    private static CreateStopDto ValidDto() => new()
    {
        StopName = "Gare Centrale",
        StopLat = 48.85m,
        StopLong = 2.35m,
        AgencyId = 1
    };

    [TestMethod]
    public void Validate_ValidDto_IsValid()
    {
        Assert.IsTrue(_validator.Validate(ValidDto()).IsValid);
    }

    [TestMethod]
    public void Validate_EmptyName_IsInvalid()
    {
        var dto = ValidDto();
        dto.StopName = "";
        var result = _validator.Validate(dto);
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual("Stop name can't be null", result.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Validate_TooLongName_IsInvalid()
    {
        var dto = ValidDto();
        dto.StopName = new string('x', 51);
        var result = _validator.Validate(dto);
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual("Stop name must be less than 50 characters", result.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Validate_LatitudeTooHigh_IsInvalid()
    {
        var dto = ValidDto();
        dto.StopLat = 90.01m;
        var result = _validator.Validate(dto);
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual("Latitude must be between -90 and 90", result.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Validate_LatitudeTooLow_IsInvalid()
    {
        var dto = ValidDto();
        dto.StopLat = -90.01m;
        Assert.IsFalse(_validator.Validate(dto).IsValid);
    }

    [TestMethod]
    public void Validate_LongitudeTooHigh_IsInvalid()
    {
        var dto = ValidDto();
        dto.StopLong = 180.01m;
        var result = _validator.Validate(dto);
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual("Longitude must be between -180 and 180", result.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Validate_LongitudeTooLow_IsInvalid()
    {
        var dto = ValidDto();
        dto.StopLong = -180.01m;
        Assert.IsFalse(_validator.Validate(dto).IsValid);
    }

    [TestMethod]
    public void Validate_BoundaryValues_AreValid()
    {
        var dto = ValidDto();
        dto.StopLat = 90m;
        dto.StopLong = -180m;
        Assert.IsTrue(_validator.Validate(dto).IsValid);
    }
}

[TestClass]
public class UpdateStopDtoValidatorTests
{
    private readonly UpdateStopDtoValidator _validator = new();

    private static UpdateStopDto ValidDto() => new()
    {
        StopName = "Place du Marché",
        StopLat = -33.86m,
        StopLong = 151.2m
    };

    [TestMethod]
    public void Validate_ValidDto_IsValid()
    {
        Assert.IsTrue(_validator.Validate(ValidDto()).IsValid);
    }

    [TestMethod]
    public void Validate_EmptyName_IsInvalid()
    {
        var dto = ValidDto();
        dto.StopName = "";
        Assert.IsFalse(_validator.Validate(dto).IsValid);
    }

    [TestMethod]
    public void Validate_TooLongName_IsInvalid()
    {
        var dto = ValidDto();
        dto.StopName = new string('x', 51);
        Assert.IsFalse(_validator.Validate(dto).IsValid);
    }

    [TestMethod]
    public void Validate_LatitudeOutOfRange_IsInvalid()
    {
        var dto = ValidDto();
        dto.StopLat = 91m;
        Assert.IsFalse(_validator.Validate(dto).IsValid);
    }

    [TestMethod]
    public void Validate_LongitudeOutOfRange_IsInvalid()
    {
        var dto = ValidDto();
        dto.StopLong = -181m;
        Assert.IsFalse(_validator.Validate(dto).IsValid);
    }
}
