using FluentResults;

namespace LastRoom.Api.Errors.Booking;

public class RoomAlreadyBookedError : Error
{
    public RoomAlreadyBookedError() :
        base("Room already booked during this period.")
    {
        Metadata.Add("ErrorCode", "Booking.RoomAlreadyBooked");
        Metadata.Add("ErrorType", ErrorType.Validation);
    }
}