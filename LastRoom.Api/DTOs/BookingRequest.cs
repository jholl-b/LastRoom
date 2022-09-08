namespace LastRoom.Api.DTOs;

public record BookingRequest(
    string ClientIdentification,
    string ClientFullName,
    DateTime CheckInDate,
    DateTime CheckOutDate);