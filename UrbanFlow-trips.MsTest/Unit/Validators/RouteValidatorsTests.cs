using UrbanFlow_trips.Application.DTO.Route;
using UrbanFlow_trips.Application.Validators;

namespace UrbanFlow_trips.MsTest.Unit.Validators;

[TestClass]
public class CreateRouteDtoValidatorTests
{
    private readonly CreateRouteDtoValidator _validator = new();

    private static CreateRouteDto ValidDto() => new()
    {
        AgencyId = 1,
        RouteShortName = "A",
        RouteLongName = "Ligne A - Centre",
        RouteTypeId = 2
    };

    [TestMethod]
    public void Validate_ValidDto_IsValid()
    {
        var result = _validator.Validate(ValidDto());
        Assert.IsTrue(result.IsValid);
    }

    [TestMethod]
    public void Validate_EmptyShortName_IsInvalid()
    {
        var dto = ValidDto();
        dto.RouteShortName = "";
        var result = _validator.Validate(dto);
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.Any(e => e.PropertyName == nameof(dto.RouteShortName)));
    }

    [TestMethod]
    public void Validate_TooLongShortName_IsInvalid()
    {
        var dto = ValidDto();
        dto.RouteShortName = new string('x', 51);
        var result = _validator.Validate(dto);
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual("Short name must be less than 50 characters", result.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Validate_EmptyLongName_IsInvalid()
    {
        var dto = ValidDto();
        dto.RouteLongName = "";
        Assert.IsFalse(_validator.Validate(dto).IsValid);
    }

    [TestMethod]
    public void Validate_TooLongLongName_IsInvalid()
    {
        var dto = ValidDto();
        dto.RouteLongName = new string('x', 101);
        var result = _validator.Validate(dto);
        Assert.IsFalse(result.IsValid);
        Assert.AreEqual("Name of city can't be superior to 100 characters", result.Errors[0].ErrorMessage);
    }

    [TestMethod]
    public void Validate_RouteTypeIdZero_IsInvalid()
    {
        var dto = ValidDto();
        dto.RouteTypeId = 0;
        var result = _validator.Validate(dto);
        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.Any(e => e.PropertyName == nameof(dto.RouteTypeId)));
    }

    [TestMethod]
    public void Validate_NegativeRouteTypeId_IsInvalid()
    {
        var dto = ValidDto();
        dto.RouteTypeId = -3;
        Assert.IsFalse(_validator.Validate(dto).IsValid);
    }
}

[TestClass]
public class UpdateRouteDtoValidatorTests
{
    private readonly UpdateRouteDtoValidator _validator = new();

    private static UpdateRouteDto ValidDto() => new()
    {
        RouteShortName = "B",
        RouteLongName = "Ligne B - Nord",
        RouteTypeId = 1
    };

    [TestMethod]
    public void Validate_ValidDto_IsValid()
    {
        Assert.IsTrue(_validator.Validate(ValidDto()).IsValid);
    }

    [TestMethod]
    public void Validate_EmptyShortName_IsInvalid()
    {
        var dto = ValidDto();
        dto.RouteShortName = "";
        Assert.IsFalse(_validator.Validate(dto).IsValid);
    }

    [TestMethod]
    public void Validate_TooLongShortName_IsInvalid()
    {
        var dto = ValidDto();
        dto.RouteShortName = new string('x', 51);
        Assert.IsFalse(_validator.Validate(dto).IsValid);
    }

    [TestMethod]
    public void Validate_EmptyLongName_IsInvalid()
    {
        var dto = ValidDto();
        dto.RouteLongName = "";
        Assert.IsFalse(_validator.Validate(dto).IsValid);
    }

    [TestMethod]
    public void Validate_TooLongLongName_IsInvalid()
    {
        var dto = ValidDto();
        dto.RouteLongName = new string('x', 101);
        Assert.IsFalse(_validator.Validate(dto).IsValid);
    }

    [TestMethod]
    public void Validate_RouteTypeIdZero_IsInvalid()
    {
        var dto = ValidDto();
        dto.RouteTypeId = 0;
        Assert.IsFalse(_validator.Validate(dto).IsValid);
    }
}
