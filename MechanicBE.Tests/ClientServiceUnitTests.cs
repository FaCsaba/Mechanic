using FluentValidation;
using FluentValidation.Results;
using MechanicBE.Contexts;
using MechanicBE.Services;
using MechanicShared.Models;
using MechanicShared.Models.Dto;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace MechanicBE.Tests;

// Given When Then
public class ClientServiceUnitTests
{
    private readonly IValidator<CreateClient> _createClientValidatorMock;
    private readonly IValidator<UpdateClient> _updateClientValidatorMock;
    private readonly MechanicContext _dbContext;
    private readonly ClientService _clientService;

    public ClientServiceUnitTests()
    {
        _createClientValidatorMock = Substitute.For<IValidator<CreateClient>>();
        _updateClientValidatorMock = Substitute.For<IValidator<UpdateClient>>();

        var options = new DbContextOptionsBuilder<MechanicContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new MechanicContext(options);

        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
        
        _clientService = new ClientService(_dbContext, _createClientValidatorMock, _updateClientValidatorMock);
    }

    [Fact]
    public async Task GetAllClientsAsync_ReturnsAllClients()
    {
        // Arrange
        var client1 = new Client { Id = Guid.NewGuid(), Name = "Client 1", Address = "Address 1", Email = "client1@example.com" };
        var client2 = new Client { Id = Guid.NewGuid(), Name = "Client 2", Address = "Address 2", Email = "client2@example.com" };
        await _dbContext.Clients.AddRangeAsync(client1, client2);
        await _dbContext.SaveChangesAsync();

        // Act
        var clients = await _clientService.GetAllClientsAsync();

        // Assert
        Assert.Equal(2, clients.Count());
    }

    [Fact]
    public async Task GetClientAsync_ReturnsClient_WhenExists()
    {
        // Arrange
        var client = new Client { Id = Guid.NewGuid(), Name = "Client 1", Address = "Address 1", Email = "client1@example.com" };
        await _dbContext.Clients.AddAsync(client);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _clientService.GetClientAsync(client.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(client.Name, result.Name);
    }

    [Fact]
    public async Task CreateClientAsync_CreatesClient_WhenValid()
    {
        // Arrange
        var createClientDto = new CreateClient { Name = "New Client", Address = "New Address", Email = "newclient@example.com" };
        _createClientValidatorMock.ValidateAsync(createClientDto).Returns(new ValidationResult());

        // Act
        var result = await _clientService.CreateClientAsync(createClientDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createClientDto.Name, result.Unwrap().Name);
    }

    [Fact]
    public async Task UpdateClientAsync_UpdatesClient_WhenValid()
    {
        // Arrange
        var client = new Client { Id = Guid.NewGuid(), Name = "Old Name", Address = "Old Address", Email = "old@example.com" };
        await _dbContext.Clients.AddAsync(client);
        await _dbContext.SaveChangesAsync();

        var updateClientDto = new UpdateClient { Id = client.Id, Name = "New Name", Address = "New Address", Email = "new@example.com" };
        _updateClientValidatorMock.ValidateAsync(updateClientDto).Returns(new ValidationResult());

        // Act
        var result = await _clientService.UpdateClientAsync(updateClientDto);

        // Assert
        Assert.Null(result);
        var updatedClient = await _clientService.GetClientAsync(client.Id);
        Assert.NotNull(updatedClient);
        Assert.Equal("New Name", updatedClient.Name);
    }

    [Fact]
    public async Task DeleteClientAsync_DeletesClient_WhenExists()
    {
        // Arrange
        var client = new Client
            { Id = Guid.NewGuid(), Name = "Client to Delete", Address = "Address", Email = "delete@example.com" };
        await _dbContext.Clients.AddAsync(client);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _clientService.DeleteClientAsync(client.Id);

        // Assert
        Assert.Null(result);
        var deletedClient = await _clientService.GetClientAsync(client.Id);
        Assert.Null(deletedClient);
    }
    
    [Fact]
    public async Task EnsureClientExists_ReturnsClient_WhenExists()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var client = new Client { Id = clientId, Name = "Existing Client", Address = "Address", Email = "existing@example.com" };
        await _dbContext.Clients.AddAsync(client);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _clientService.EnsureClientExists(clientId);

        // Assert
        Assert.IsType<Client>(result.Unwrap());
        Assert.Equal(clientId, result.Unwrap().Id);
    }

    [Fact]
    public async Task EnsureClientExists_ReturnsNotFoundError_WhenClientDoesNotExist()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act
        var result = await _clientService.EnsureClientExists(clientId);

        // Assert
        Assert.Equal("Client with id could not be found", result.UnwrapErr().Message);
    }
}