using FluentResults;

namespace LastRoom.Api.Errors.Booking;

public class BookingDateTooFarError : Error
{
    public BookingDateTooFarError() : 
        base("You can't book a room more that 30 days in advance.")
    {
        Metadata.Add("ErrorCode", "Booking.BookingDateTooFar");
        Metadata.Add("ErrorType", ErrorType.Validation);
    }
}