using FluentResults;

namespace LastRoom.Api.Errors.Booking;

public class CheckOutBeforeCheckInError : Error
{
    public CheckOutBeforeCheckInError() :
        base("Check-in date before check-out date.")
        {
            Metadata.Add("ErrorCode", "Booking.CheckOutBeforeCheck");
            Metadata.Add("ErrorType", ErrorType.Validation);
        }
}