using FluentResults;

namespace LastRoom.Api.Errors.Booking;

public class StartDateNotAllowedError : Error
{
    public StartDateNotAllowedError() : 
        base($"You can only make a booking starting tomorrow.")
    {
        Metadata.Add("ErrorCode", "Booking.StartDateNotAllowed");
        Metadata.Add("ErrorType", ErrorType.Validation);
    }
}