namespace LastRoom.Api.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateOnly DateOnlyUtcNow => DateOnly.FromDateTime(DateTime.UtcNow);
}