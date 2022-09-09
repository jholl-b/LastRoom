using FluentResults;
using LastRoom.Api.Models;

namespace LastRoom.Api.Services;

public interface IBookingService
{
    public Task<Result<Booking>> GetBookingAsync(Guid ticket);
    public Task<List<Booking>> GetAllBookingsAsync();
    public Task<Result<Booking>> CreateNewBookingAsync(string clientIdentification,
        string clientFullName,
        DateOnly checkInDate,
        DateOnly checkOutDate);
    public Task<Result> UpdateBookingAsync(Guid ticket, Booking booking);
    public Task<Result> CancelBooking(Guid ticket);
}