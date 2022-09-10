using System.Text.Json.Serialization;
using LastRoom.Api.Converters;

namespace LastRoom.Api.DTOs;

public record BookingVacationResponse
{
    public bool Vacant { get; init; }
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly Date { get; init; }
}