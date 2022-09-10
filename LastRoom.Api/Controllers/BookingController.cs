using LastRoom.Api.DTOs;
using LastRoom.Api.Models;
using LastRoom.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LastRoom.Api.Controllers;

[Route("[controller]")]
public class BookingsController : ApiController
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("{ticket:guid}")]
    public async Task<ActionResult<BookingResponse>> Get(Guid ticket)
    {
        var result = await _bookingService.GetBookingAsync(ticket);
        
        if (result.IsFailed)
            return Problem(result.Errors);
        
        var response = new BookingResponse
        {
            Ticket = result.Value.Ticket,
            ClientFullName = result.Value.Client.FullName,
            CheckInDate = result.Value.CheckInDate,
            CheckOutDate = result.Value.CheckOutDate
        };
        
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<ActionResult<BookingDaysResponse>> Get()
    {
        var bookings = await _bookingService.GetAllPossibleBookingDatesAsync();
        var listResponse = new List<BookingDaysResponse>();

        foreach (var booking in bookings)
        {
            listResponse.Add(new BookingDaysResponse
            {
                Date = booking.Key,
                Vacant = booking.Value
            });
        }
        
        return Ok(listResponse);
    }
    
    [HttpPost]
    public async Task<ActionResult<BookingResponse>> Post(BookingRequest request)
    {
        var result = await _bookingService.CreateNewBookingAsync(
            request.ClientIdentification,
            request.ClientFullName,
            request.CheckInDate,
            request.CheckOutDate);

        if (result.IsFailed)
            return Problem(result.Errors);

        var response = new BookingResponse
        {
            Ticket = result.Value.Ticket,
            ClientFullName = result.Value.Client.FullName,
            CheckInDate = result.Value.CheckInDate,
            CheckOutDate = result.Value.CheckOutDate
        };

        return Ok(response);
    }

    [HttpPut("{ticket:guid}")]
    public async Task<ActionResult<BookingResponse>> Put(Guid ticket, BookingRequest request)
    {
        var booking = new Booking
        {
            Ticket = ticket,
            CheckInDate = request.CheckInDate,
            CheckOutDate = request.CheckOutDate,
            Client = new Client
            {
                Identification = request.ClientIdentification,
                FullName = request.ClientFullName
            }
        };

        var result = await _bookingService.UpdateBookingAsync(ticket, booking);
        
        if (result.IsFailed)
            return Problem(result.Errors);

        var response = (new BookingResponse
            {
                Ticket = booking.Ticket,
                ClientFullName = booking.Client.FullName,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate
            });

        return Ok(response);
    }

    [HttpDelete("{ticket:guid}")]
    public async Task<ActionResult> Delete(Guid ticket)
    {
        var result = await _bookingService.CancelBookingAsync(ticket);
        
        if (result.IsFailed)
            return Problem(result.Errors);

        return NoContent();
    }
}