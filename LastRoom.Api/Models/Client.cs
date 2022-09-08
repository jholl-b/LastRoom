namespace LastRoom.Api.Models;

public class Client
{
    public int Id { get; set; }
    public string Identification { get; set; } = default!;
    public string FullName { get; set; } = default!;
}