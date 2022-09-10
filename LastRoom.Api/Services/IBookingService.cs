using FluentResults;
using LastRoom.Api.Models;

namespace LastRoom.Api.Services;

public interface IBookingService
{
    public Task<Result<Booking>> GetBookingAsync(Guid ticket);
    public Task<Dictionary<DateOnly, bool>> GetAllPossibleBookingDatesAsync();
    public Task<Result<Booking>> CreateNewBookingAsync(
        string clientIdentification,
        string clientFullName,
        DateOnly checkInDate,
        DateOnly checkOutDate);
    public Task<Result> UpdateBookingAsync(Guid ticket, Booking booking);
    public Task<Result> CancelBookingAsync(Guid ticket);
}