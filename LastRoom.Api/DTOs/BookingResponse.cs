namespace LastRoom.Api.DTOs;

public record BookingResponse(
    Guid Ticket,
    string ClientFullName,
    DateTime CheckInDate,
    DateTime CheckOutDate);