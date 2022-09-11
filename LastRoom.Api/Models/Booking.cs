namespace LastRoom.Api.Models;

public class Booking
{
    public int Id { get; set; }
    public Guid Ticket { get; set; }
    public DateOnly CheckInDate { get; set; }
    public DateOnly CheckOutDate { get; set; }
    
    public int ClientId { get; set; }
    public Client Client { get; set; } = default!;
}