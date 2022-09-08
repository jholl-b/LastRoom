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
    
    public async Task<IList<Booking>> GetAllBookingsAsync()
    {
        return await _dbContext
            .Bookings
            .Include(x => x.Client)
            .ToListAsync();
    }

    public async Task<Booking> CreateNewBookingAsync(
        string clientIdentification, 
        string clientFullName, 
        DateOnly checkInDate, 
        DateOnly checkOutDate)
    {
        //TODO
        //room must be available
        var reservated = _dbContext
            .Bookings
            .Any(x => x.CheckInDate < checkOutDate 
                      && checkInDate < x.CheckOutDate );
        
        //TODO change exception to Result value
        if (reservated) throw new Exception("Room booked during this period");

        //the stay can’t be longer than 3 days
        var period = checkOutDate.DayNumber - checkInDate.DayNumber;
        if ( period > 3 ) throw new Exception("You can't book a room for more that 3 days");
        
        //can’t be reserved more than 30 days in advance
        if (checkInDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber > 30)
            throw new Exception("You can't book a room more that 30 days in advance");
        
        //can reserve starting from the next day
        if (checkInDate.DayNumber - DateOnly.FromDateTime(DateTime.Now).DayNumber < 1)
            throw new Exception("You can't book a room for a date earlier than tomorrow");
        
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