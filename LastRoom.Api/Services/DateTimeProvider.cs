namespace LastRoom.Api.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime DateTimeUtcNow => DateTime.UtcNow;
    public DateOnly DateOnlyUtcNow => DateOnly.FromDateTime(DateTime.UtcNow);
}