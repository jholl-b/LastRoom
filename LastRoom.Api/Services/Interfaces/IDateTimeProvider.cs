namespace LastRoom.Api.Services.Interfaces;

public interface IDateTimeProvider
{
    DateOnly DateOnlyUtcNow { get; }
}