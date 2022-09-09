using LastRoom.Api.DTOs;
using LastRoom.Api.Models;
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

    [HttpGet("{ticket:guid}")]
    public async Task<ActionResult<BookingResponse>> Get(Guid ticket)
    {
        var result = await _bookingService.GetBookingAsync(ticket);
        
        if (result.IsFailed)
            return Problem(result.Errors);
        
        var response = new BookingResponse(
            result.Value.Ticket,
            result.Value.Client.FullName,
            result.Value.CheckInDate.ToDateTime(TimeOnly.MinValue),
            result.Value.CheckOutDate.ToDateTime(TimeOnly.MaxValue));
        
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<BookingResponse>> Get()
    {
        //TODO return only dates
        
        var bookings = await _bookingService.GetAllBookingsAsync();
        
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

    [HttpPut("{ticket:guid}")]
    public async Task<ActionResult<BookingResponse>> Put(Guid ticket, BookingRequest request)
    {
        var booking = new Booking
        {
            Ticket = ticket,
            CheckInDate = DateOnly.FromDateTime(request.CheckInDate),
            CheckOutDate = DateOnly.FromDateTime(request.CheckOutDate),
            Client = new Client
            {
                Identification = request.ClientIdentification,
                FullName = request.ClientFullName
            }
        };

        var result = await _bookingService.UpdateBookingAsync(ticket, booking);
        
        if (result.IsFailed)
            return Problem(result.Errors);

        var response = new BookingResponse(
            booking.Ticket,
            booking.Client.FullName,
            booking.CheckInDate.ToDateTime(TimeOnly.MinValue),
            booking.CheckOutDate.ToDateTime(TimeOnly.MaxValue));

        return Ok(response);
    }

    [HttpDelete("{ticket:guid}")]
    public async Task<ActionResult> Delete(Guid ticket)
    {
        var result = await _bookingService.CancelBooking(ticket);
        
        if (result.IsFailed)
            return Problem(result.Errors);

        return NoContent();
    }
}