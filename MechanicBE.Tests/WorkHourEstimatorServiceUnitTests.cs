using System.Diagnostics;
using MechanicBE.Services;
using MechanicShared.Models;
using NSubstitute;

namespace MechanicBE.Tests;

public class WorkHourEstimatorServiceUnitTests
{
    private readonly TimeProvider _timeProvider;
    private readonly WorkHourEstimationService _service;

    public WorkHourEstimatorServiceUnitTests()
    {
        _timeProvider = Substitute.For<TimeProvider>();
        _service = new WorkHourEstimationService(_timeProvider);
    }

    [Theory]
    [InlineData(FaultCategory.Bodywork, "2018-01-01", 5, 3 * 1 * 0.6)]
    [InlineData(FaultCategory.Motor, "2010-01-01", 8, 8 * 1.5 * 0.8)]
    [InlineData(FaultCategory.RunningGear, "2000-01-01", 3, 6 * 2 * 0.4)]
    public void WorkHours_ShouldReturnCorrectValue(FaultCategory faultCategory, string manufacturingDate, int severity, double expected)
    {
        // Arrange
        var commission = new Commission
        {
            FaultCategory = faultCategory,
            VehicleManufacturingDate = DateOnly.Parse(manufacturingDate),
            Severity = severity
        };
        _timeProvider.GetUtcNow().Returns(new DateTime(2023, 1, 1));

        // Act
        var result = _service.WorkHours(commission);

        // Assert
        Assert.Equal(expected, result);
    }
    
    [Fact]
    public void WorkHours_ShouldThrowUnreachableException_ForInvalidSeverity()
    {
        // Arrange
        var commission = new Commission
        {
            FaultCategory = FaultCategory.Bodywork,
            VehicleManufacturingDate = new DateOnly(2018, 1, 1),
            Severity = 11 // Invalid severity
        };
        _timeProvider.GetUtcNow().Returns(new DateTime(2023, 1, 1));

        // Act & Assert
        Assert.Throws<UnreachableException>(() => _service.WorkHours(commission));
    }

    [Fact]
    public void WorkHours_ShouldReturnCorrectValue_ForRunningGearFaultCategory()
    {
        // Arrange
        var commission = new Commission
        {
            FaultCategory = FaultCategory.RunningGear,
            VehicleManufacturingDate = new DateOnly(2000, 1, 1),
            Severity = 3
        };
        _timeProvider.GetUtcNow().Returns(new DateTime(2023, 1, 1));

        // Act
        var result = _service.WorkHours(commission);

        // Assert
        Assert.Equal(6 * 2 * 0.4, result);
    }
}