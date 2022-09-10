using System.Text.Json.Serialization;
using LastRoom.Api.Converters;

namespace LastRoom.Api.DTOs;

public class BookingRequest
{
    public string ClientIdentification { get; init; } = default!;
    public string ClientFullName { get; init; } = default!;
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly CheckInDate { get; init; }
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly CheckOutDate { get; init; }
}