namespace LastRoom.Api.Services;

public interface IDateTimeProvider
{
    DateOnly DateOnlyUtcNow { get; }
}