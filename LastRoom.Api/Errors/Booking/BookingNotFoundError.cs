using FluentResults;

namespace LastRoom.Api.Errors.Booking;

public class BookingNotFoundError : Error
{
    public BookingNotFoundError() :
        base("Booking not found.")
    {
        Metadata.Add("ErrorCode", "Booking.BookingNotFound");
        Metadata.Add("ErrorType", ErrorType.Validation);
    }
}