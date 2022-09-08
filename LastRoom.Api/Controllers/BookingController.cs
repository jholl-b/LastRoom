using LastRoom.Api.DTOs;
using LastRoom.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LastRoom.Api.Controllers;

[Route("[controller]")]
public class BookingController : ApiController
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
        var result = await _bookingService.CreateNewBookingAsync(
            request.ClientIdentification,
            request.ClientFullName,
            DateOnly.FromDateTime(request.CheckInDate),
            DateOnly.FromDateTime(request.CheckOutDate));

        if (result.IsFailed)
            return Problem(result.Errors);

        var response = new BookingResponse(
            result.Value.Ticket,
            result.Value.Client.FullName,
            result.Value.CheckInDate.ToDateTime(TimeOnly.MinValue),
            result.Value.CheckOutDate.ToDateTime(TimeOnly.MaxValue));

        return Ok(response);
    }
}