using LastRoom.Api.Models;
using LastRoom.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LastRoom.Api.Tests.Services;

public class BookingServiceTests
{
    private readonly IBookingService _sut;
    private readonly Mock<IDateTimeProvider> _dateTimeProvider;

    public BookingServiceTests()
    {
        var dbOptions = new DbContextOptionsBuilder<LastRoomDbContext>()
            .UseInMemoryDatabase(databaseName: "db")
            .Options;
        var dbContext = new LastRoomDbContext(dbOptions);

        _dateTimeProvider = new Mock<IDateTimeProvider>();
        _dateTimeProvider
            .Setup(x => x.DateOnlyUtcNow)
            .Returns(new DateOnly(2022, 01, 01));
        
        _sut = new BookingService(dbContext, _dateTimeProvider.Object);
    }
    
    [Fact]
    public async Task CreateNewBookingAsync_ShouldCreateBooking_WhenAllParametersAreValid()
    {
        // Arrange
        var clientName = "Test";
        var clientId = "123456789";
        
        // Act
        var result = await _sut.CreateNewBookingAsync(
            clientIdentification: clientId,
            clientFullName: clientName,
            checkInDate: new DateOnly(2022, 01, 05),
            checkOutDate: new DateOnly(2022, 01, 07));
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.Client.FullName, clientName);
        Assert.Equal(result.Value.Client.Identification, clientId);
    }
}