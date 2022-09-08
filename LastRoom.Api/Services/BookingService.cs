using FluentResults;
using LastRoom.Api.Errors.Booking;
using LastRoom.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LastRoom.Api.Services;

public class BookingService : IBookingService
{
    private readonly LastRoomDbContext _dbContext;

    public BookingService(LastRoomDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Booking GetAllBookings(Guid ticket)
    {
        throw new NotImplementedException();
    }
    
    public async Task<List<Booking>> GetAllBookingsAsync()
    {
        var bookings = await _dbContext
            .Bookings
            .Include(x => x.Client)
            .AsNoTracking()
            .ToListAsync();

        return bookings;
    }

    public async Task<Result<Booking>> CreateNewBookingAsync(
        string clientIdentification, 
        string clientFullName, 
        DateOnly checkInDate, 
        DateOnly checkOutDate)
    {
        var reserved = _dbContext
            .Bookings
            .Any(x => x.CheckInDate <= checkOutDate && checkInDate <= x.CheckOutDate );
        
        if (reserved)
            return Result.Fail(new RoomAlreadyBookedError());
        
        var period = checkOutDate.DayNumber - checkInDate.DayNumber;
        if (period > 3)
            return Result.Fail(new StayPeriodTooLongError());
        
        if (checkInDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber > 30)
            return Result.Fail(new BookingDateTooFarError());

        if (checkInDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber < 1)
            return Result.Fail(new StartDateNotAllowedError());
        
        //Can book
        var booking = new Booking
        {
            Ticket = Guid.NewGuid(),
            Client = new Client
            {
                Identification = clientIdentification,
                FullName = clientFullName
            },
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate
        };

        await _dbContext.AddAsync(booking);
        await _dbContext.SaveChangesAsync();

        return booking;
    }

    public Booking UpdateBooking(Guid ticket, Booking booking)
    {
        throw new NotImplementedException();
    }

    public void CancelBooking(Guid ticket)
    {
        throw new NotImplementedException();
    }
}