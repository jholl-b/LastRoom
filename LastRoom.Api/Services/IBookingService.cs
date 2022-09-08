using LastRoom.Api.Models;

namespace LastRoom.Api.Services;

public interface IBookingService
{
    public Booking GetAllBookings(Guid ticket);
    public Task<IList<Booking>> GetAllBookingsAsync();
    public Task<Booking> CreateNewBookingAsync(
        string clientIdentification,
        string clientFullName,
        DateOnly checkInDate,
        DateOnly checkOutDate);
    public Booking UpdateBooking(Guid ticket, Booking booking);
    public void CancelBooking(Guid ticket);
}