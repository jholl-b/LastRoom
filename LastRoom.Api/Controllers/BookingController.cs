using LastRoom.Api.DTOs;
using LastRoom.Api.Models;
using LastRoom.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace LastRoom.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("etc/{ticket:guid}")]
    public async Task<ActionResult<BookingResponse>> Get(Guid ticket)
    {
        //TODO
        return Ok(null);
    }
    
    [HttpGet]
    public async Task<ActionResult<BookingResponse>> Get()
    {
        var bookings = await _bookingService.GetAllBookingsAsync();
        
        //TODO
        var listResponse = new List<BookingResponse>();
        foreach (var booking in bookings)
        {
            listResponse.Add(new BookingResponse
            (
                booking.Ticket,
                booking.Client.FullName,
                booking.CheckInDate.ToDateTime(TimeOnly.MinValue),
                booking.CheckOutDate.ToDateTime(TimeOnly.MaxValue)));
        }
        
        return Ok(listResponse);
    }
    
    [HttpPost]
    public async Task<ActionResult<BookingResponse>> Post(BookingRequest request)
    {
        //TODO
        var booking = await _bookingService.CreateNewBookingAsync(
            request.ClientIdentification,
            request.ClientFullName,
            DateOnly.FromDateTime(request.CheckInDate),
            DateOnly.FromDateTime(request.CheckOutDate));

        return Ok(booking);
    }
}