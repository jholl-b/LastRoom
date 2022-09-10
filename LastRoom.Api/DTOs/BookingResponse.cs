using System.Text.Json.Serialization;
using LastRoom.Api.Converters;

namespace LastRoom.Api.DTOs;

public class BookingResponse
{
    public Guid Ticket { get; init; }
    public string ClientFullName { get; init; } = default!;
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly CheckInDate { get; init; }
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly CheckOutDate { get; init; }
}