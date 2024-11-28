using FluentValidation;
using FluentValidation.Results;
using MechanicBE.Contexts;
using MechanicBE.Services;
using MechanicShared.Errors;
using MechanicShared.Models;
using MechanicShared.Models.Dto;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace MechanicBE.Tests;

public class CommissionServiceUnitTests
{
    private readonly MechanicContext _dbContext;
    private readonly IClientService _clientService;
    private readonly IValidator<CreateCommission> _createCommissionValidator;
    private readonly IValidator<UpdateCommission> _updateCommissionValidator;
    private readonly CommissionService _commissionService;
    private readonly Client _client;

    public CommissionServiceUnitTests()
    {
        var options = new DbContextOptionsBuilder<MechanicContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new MechanicContext(options);

        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();

        _client = new Client
            { Id = Guid.NewGuid(), Name = "Client", Address = "Address", Email = "client1@example.com" };
        _dbContext.Clients.Add(_client);
        _dbContext.SaveChanges();

        _clientService = Substitute.For<IClientService>();
        _createCommissionValidator = Substitute.For<IValidator<CreateCommission>>();
        _updateCommissionValidator = Substitute.For<IValidator<UpdateCommission>>();
        _commissionService = new CommissionService(_dbContext, _clientService, _createCommissionValidator,
            _updateCommissionValidator);
    }

    [Fact]
    public async Task GetCommissionsAsync_ReturnsAllCommissions()
    {
        // Arrange
        var commission1 = new Commission
        {
            Id = Guid.NewGuid(), ClientId = _client.Id, LicensePlateNumber = "ASD-123",
            FaultCategory = FaultCategory.Bodywork, Severity = 1
        };
        var commission2 = new Commission
        {
            Id = Guid.NewGuid(), ClientId = _client.Id, LicensePlateNumber = "ASD-321",
            FaultCategory = FaultCategory.Brake, Severity = 5
        };
        await _dbContext.Commissions.AddRangeAsync(commission1, commission2);
        await _dbContext.SaveChangesAsync();

        // Act
        var commissions = await _commissionService.GetCommissionsAsync();

        // Assert
        Assert.Equal(2, commissions.Count);
    }

    [Fact]
    public async Task CreateCommissionAsync_CreatesCommission_WhenValid()
    {
        // Arrange
        var createCommissionDto = new CreateCommission
        {
            ClientId = _client.Id,
            FaultCategory = FaultCategory.Bodywork,
            Severity = 1,
            VehicleManufacturingDate = DateOnly.FromDateTime(DateTime.Now),
            LicensePlateNumber = "ABC-123",
            Description = "Test Description"
        };

        _createCommissionValidator.ValidateAsync(createCommissionDto).Returns(Task.FromResult(new ValidationResult()));

        // Act
        var result = await _commissionService.CreateCommissionAsync(createCommissionDto);

        // Assert
        Assert.NotNull(result);
        var commission = result.Unwrap();
        Assert.Equal(createCommissionDto.ClientId, commission.ClientId);
        Assert.Equal(createCommissionDto.FaultCategory, commission.FaultCategory);
        Assert.Equal(createCommissionDto.Severity, commission.Severity);
        Assert.Equal(createCommissionDto.LicensePlateNumber, commission.LicensePlateNumber);
        Assert.Equal(createCommissionDto.Description, commission.Description);
    }

    [Fact]
    public async Task DeleteCommissionAsync_DeletesCommission_WhenExists()
    {
        // Arrange
        var commission = new Commission
        {
            Id = Guid.NewGuid(), ClientId = _client.Id, LicensePlateNumber = "ASD-123",
            FaultCategory = FaultCategory.Bodywork, Severity = 1
        };
        await _dbContext.Commissions.AddAsync(commission);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _commissionService.DeleteCommissionAsync(commission.Id);

        // Assert
        Assert.Null(result); // Assuming no error means success
        var deletedCommission = await _commissionService.EnsureCommissionExists(commission.Id);
        Assert.IsType<NotFoundError>(deletedCommission.UnwrapErr());
    }


    [Fact]
    public async Task UpdateCommissionAsync_UpdatesCommission_WhenValid()
    {
        // Arrange
        var commission = new Commission
        {
            Id = Guid.NewGuid(),
            ClientId = _client.Id,
            LicensePlateNumber = "ASD-123",
            VehicleManufacturingDate = DateOnly.FromDateTime(DateTime.Now),
            Description = "",
            FaultCategory = FaultCategory.Bodywork,
            Severity = 1,
            Status = CommissionStatus.Todo
        };
        await _dbContext.Commissions.AddAsync(commission);
        await _dbContext.SaveChangesAsync();

        var updateCommissionDto = new UpdateCommission
        {
            Id = commission.Id,
            ClientId = commission.ClientId,
            FaultCategory = FaultCategory.Brake,
            Severity = 2,
            VehicleManufacturingDate = DateOnly.FromDateTime(DateTime.Now),
            LicensePlateNumber = "XYZ-789",
            Description = "Updated Description",
            Status = CommissionStatus.Doing,
        };

        _updateCommissionValidator.ValidateAsync(updateCommissionDto)
            .Returns(Task.FromResult(new ValidationResult()));
        _clientService.EnsureClientExists(updateCommissionDto.ClientId).Returns(_client);

        // Act
        var result = await _commissionService.UpdateCommissionAsync(updateCommissionDto);

        // Assert
        Assert.Null(result); // Assuming
    }
    
    [Fact]
    public async Task EnsureCommissionExists_WhenExists_ReturnsCommission()
    {
        // Arrange
        var commission = new Commission
        {
            Id = Guid.NewGuid(),
            ClientId = _client.Id,
            LicensePlateNumber = "ASD-123",
            VehicleManufacturingDate = DateOnly.FromDateTime(DateTime.Now),
            Description = "",
            FaultCategory = FaultCategory.Bodywork,
            Severity = 1,
            Status = CommissionStatus.Todo
        };
        await _dbContext.Commissions.AddAsync(commission);
        await _dbContext.SaveChangesAsync();
        
        // Act
        var result = await _commissionService.EnsureCommissionExists(commission.Id);

        // Assert
        Assert.IsType<Commission>(result.Unwrap());
        Assert.Equal(commission.Id, result.Unwrap().Id);
    }

    [Fact]
    public async Task EnsureClientExists_ReturnsNotFoundError_WhenClientDoesNotExist()
    {
        // Arrange
        var commissionId = Guid.NewGuid();

        // Act
        var result = await _commissionService.EnsureCommissionExists(commissionId);

        // Assert
        Assert.Equal("Commission with id could not be found", result.UnwrapErr().Message);
    }
}