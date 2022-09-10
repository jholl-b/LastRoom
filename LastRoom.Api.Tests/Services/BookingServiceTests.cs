using LastRoom.Api.Models;
using LastRoom.Api.Services;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace LastRoom.Api.Tests.Services;

public class BookingServiceTests : IDisposable
{
    private readonly IBookingService _sut;
    private readonly Mock<IDateTimeProvider> _dateTimeProvider;
    private readonly LastRoomDbContext _dbContext;

    public BookingServiceTests()
    {
        var dbOptions = new DbContextOptionsBuilder<LastRoomDbContext>()
            .UseInMemoryDatabase(databaseName: "db")
            .Options;
        _dbContext = new LastRoomDbContext(dbOptions);

        _dateTimeProvider = new Mock<IDateTimeProvider>();
        _dateTimeProvider
            .Setup(x => x.DateOnlyUtcNow)
            .Returns(new DateOnly(2022, 01, 01));
        
        _sut = new BookingService(_dbContext, _dateTimeProvider.Object);
    }

    public void Dispose()
    {
        _dbContext.Database.EnsureDeleted();
    }

    private async Task<Booking> CreateBookingForTestAsync(string name = "", string id = "", DateOnly? checkIn = null, DateOnly? checkOut = null)
    {
        var clientName = name ?? "Test";
        var clientId = id ?? "123456789";
        var checkInDate =  checkIn ?? new DateOnly(2022, 01, 05);
        var checkOutDate = checkOut ?? new DateOnly(2022, 01, 07);

        var booking = await _sut.CreateNewBookingAsync(
            clientIdentification: clientId,
            clientFullName: clientName,
            checkInDate: checkInDate,
            checkOutDate: checkOutDate);

        return booking.Value;
    }

    [Fact]
    public async Task GetBookingAsync_ShouldReturnBooking_WhenTicketIsValid()
    {
        // Arrange
        var booking = await CreateBookingForTestAsync();

        // Act
        var result = await _sut.GetBookingAsync(booking.Ticket);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.IsType<Booking>(result.Value);
        Assert.Equal(booking.Ticket, result.Value.Ticket);
    }

    [Fact]
    public async Task GetBookingAsync_ShouldNotReturnBooking_WhenTicketIsNotValid()
    {
        // Arrange
        var booking = await CreateBookingForTestAsync();

        // Act
        var result = await _sut.GetBookingAsync(Guid.Empty);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Booking.BookingNotFound", result.Errors[0].Metadata["ErrorCode"]);
    }

    [Fact]
    public async Task GetAllPossibleBookingDatesAsync_ShouldReturn30Days()
    {
        // Arrange
        await CreateBookingForTestAsync();

        // Act
        var result = await _sut.GetAllPossibleBookingDatesAsync();

        // Assert
        Assert.True(result.Count == 30);
        Assert.IsType<Dictionary<DateOnly, bool>>(result);
    }
    
    [Fact]
    public async Task CreateNewBookingAsync_ShouldCreateBooking_WhenAllParametersAreValid()
    {
        // Arrange
        var clientName = "Test";
        var clientId = "123456789";
        var checkInDate = new DateOnly(2022, 01, 05);
        var checkOutDate = new DateOnly(2022, 01, 07);
    
        // Act
        var result = await _sut.CreateNewBookingAsync(
            clientIdentification: clientId,
            clientFullName: clientName,
            checkInDate: checkInDate,
            checkOutDate: checkOutDate);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(clientName, result.Value.Client.FullName);
        Assert.Equal(clientId, result.Value.Client.Identification);
    }

    [Fact]
    public async Task CreateNewBookingAsync_ShouldNotCreateBooking_WhenDayAlreadyBooked()
    {
        //Arrange
        var clientName = "Pre Register";
        var clientId = "987654321";
        var booking = await CreateBookingForTestAsync();

        //Act
        var result = await _sut.CreateNewBookingAsync(
            clientIdentification: clientId,
            clientFullName: clientName,
            checkInDate: booking.CheckInDate,
            checkOutDate: booking.CheckOutDate);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Booking.RoomAlreadyBooked", result.Errors[0].Metadata["ErrorCode"]);
    }

    [Fact]
    public async Task CreateNewBookingAsync_ShouldNotCreateBooking_WhenCheckOutBeforeCheckIn()
    {
        //Arrange
        var clientName = "Test";
        var clientId = "123456789";
        var checkInDate = new DateOnly(2022, 01, 04);
        var checkOutDate = new DateOnly(2022, 01, 02);

        //Act
        var result = await _sut.CreateNewBookingAsync(
            clientIdentification: clientId,
            clientFullName: clientName,
            checkInDate: checkInDate,
            checkOutDate: checkOutDate);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Booking.CheckOutBeforeCheck", result.Errors[0].Metadata["ErrorCode"]);
    }

    [Fact]
    public async Task CreateNewBookingAsync_ShouldNotCreateBooking_WhenStayPeriodTooLong()
    {
        //Arrange
        var clientName = "Test";
        var clientId = "123456789";
        var checkInDate = new DateOnly(2022, 01, 02);
        var checkOutDate = new DateOnly(2022, 01, 05);

        //Act
        var result = await _sut.CreateNewBookingAsync(
            clientIdentification: clientId,
            clientFullName: clientName,
            checkInDate: checkInDate,
            checkOutDate: checkOutDate);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Booking.PeriodNotAllowed", result.Errors[0].Metadata["ErrorCode"]);
    }

    [Fact]
    public async Task CreateNewBookingAsync_ShouldNotCreateBooking_WhenBookingDateTooFar()
    {
        //Arrange
        var clientName = "Test";
        var clientId = "123456789";
        var checkInDate = new DateOnly(2022, 02, 02);
        var checkOutDate = new DateOnly(2022, 02, 03);

        //Act
        var result = await _sut.CreateNewBookingAsync(
            clientIdentification: clientId,
            clientFullName: clientName,
            checkInDate: checkInDate,
            checkOutDate: checkOutDate);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Booking.BookingDateTooFar", result.Errors[0].Metadata["ErrorCode"]);
    }

    [Fact]
    public async Task CreateNewBookingAsync_ShouldNotCreateBooking_WhenStartDateNotAllowed()
    {
        // Arrange
        var clientName = "Test";
        var clientId = "123456789";
        var checkInDate = new DateOnly(2022, 01, 01);
        var checkOutDate = new DateOnly(2022, 01, 03);

        //Act
        var result = await _sut.CreateNewBookingAsync(
            clientIdentification: clientId,
            clientFullName: clientName,
            checkInDate: checkInDate,
            checkOutDate: checkOutDate);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Booking.StartDateNotAllowed", result.Errors[0].Metadata["ErrorCode"]);
    }

    [Fact]
    public async Task UpdateBookingAsync_ShouldUpdateBooking_WhenTicketIsValid()
    {
        // Arrange
        var clientNewName = "New Test";
        var booking = await CreateBookingForTestAsync();

        booking.Client.FullName = clientNewName;

        // Act
        var result = await _sut.UpdateBookingAsync(
            ticket: booking.Ticket,
            booking: booking
        );

        // Assert
        var newBooking = await _sut.GetBookingAsync(booking.Ticket);

        Assert.True(result.IsSuccess);
        Assert.True(newBooking.IsSuccess);
        Assert.Equal(clientNewName, newBooking.Value.Client.FullName);
    }

    [Fact]
    public async Task UpdateBookingAsync_ShouldNotUpdateBooking_WhenTicketIsNotValid()
    {
        // Arrange
        var ticket = Guid.Empty;
        var booking = await CreateBookingForTestAsync();

        // Act
        var result = await _sut.UpdateBookingAsync(
            ticket: ticket,
            booking: booking
        );

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Booking.BookingNotFound", result.Errors[0].Metadata["ErrorCode"]);
    }

    [Fact]
    public async Task UpdateBookingAsync_ShouldNotUpdateBooking_WhenDateHasAlreadyBooked()
    {
        // Arrange
        var clientName1 = "Test 01";
        var clientId1 = "123456789";
        var checkInDate1 = new DateOnly(2022, 01, 02);
        var checkOutDate1 = new DateOnly(2022, 01, 03);

        var clientName2 = "Test 02";
        var clientId2 = "123456789";
        var checkInDate2 = new DateOnly(2022, 01, 05);
        var checkOutDate2 = new DateOnly(2022, 01, 07);

        var booking1 = await CreateBookingForTestAsync(clientName1, clientId1, checkInDate1, checkOutDate1);
        var booking2 = await CreateBookingForTestAsync(clientName2, clientId2, checkInDate2, checkOutDate2);

        // Act
        var bookingToUpdate = booking2;
        bookingToUpdate.CheckInDate = checkInDate1;
        bookingToUpdate.CheckOutDate = checkOutDate1;

        var result = await _sut.UpdateBookingAsync(
            ticket: bookingToUpdate.Ticket,
            booking: bookingToUpdate
        );

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Booking.RoomAlreadyBooked", result.Errors[0].Metadata["ErrorCode"]);
    }

    [Fact]
    public async Task UpdateBookingAsync_ShouldNotUpdateBooking_WhenCreatedValidationFail()
    {
        // Arrange
        var booking = await CreateBookingForTestAsync();

        // Act
        booking.CheckInDate = booking.CheckOutDate.AddDays(1);

        var result = await _sut.UpdateBookingAsync(
            ticket: booking.Ticket,
            booking: booking
        );

        // Assert
        Assert.True(result.IsFailed);
        Assert.Equal("Booking.CheckOutBeforeCheck", result.Errors[0].Metadata["ErrorCode"]);
    }

    [Fact]
    public async Task CancelBookingAsync_ShouldRemoveBooking_WhenTicketIsValid()
    {
        // Arrange
        var booking = await CreateBookingForTestAsync();

        // Act
        var result = await _sut.CancelBookingAsync(booking.Ticket);

        // Assert
        var bookings = await _sut.GetAllPossibleBookingDatesAsync();

        Assert.True(result.IsSuccess);
        Assert.False(bookings.Any(x => x.Value == false));
    }

    [Fact]
    public async Task CancelBookingAsync_ShouldNotRemoveBooking_WhenTicketIsNotValid()
    {
        // Arrange
        var booking = await CreateBookingForTestAsync();

        // Act
        var result = await _sut.CancelBookingAsync(Guid.Empty);

        // Assert
        var bookings = await _sut.GetAllPossibleBookingDatesAsync();

        Assert.True(result.IsFailed);
        Assert.Equal("Booking.BookingNotFound", result.Errors[0].Metadata["ErrorCode"]);
    }

}