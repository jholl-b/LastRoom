namespace LastRoom.Api.Services;

public interface IDateTimeProvider
{
    DateTime DateTimeUtcNow { get; }
    DateOnly DateOnlyUtcNow { get; }
}