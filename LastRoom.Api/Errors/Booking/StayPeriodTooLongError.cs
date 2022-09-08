using FluentResults;

namespace LastRoom.Api.Errors.Booking;

public class StayPeriodTooLongError : Error
{
    public StayPeriodTooLongError() : 
        base("You can't book a room for more that 3 days.")
    {
        Metadata.Add("ErrorCode", "Booking.PeriodNotAllowed");
        Metadata.Add("ErrorType", ErrorType.Validation);
    }
}