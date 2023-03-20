using LastRoom.Api.DTOs;
using LastRoom.Api.Models;
using LastRoom.Api.Services;
using LastRoom.Api.Services.Interfaces;
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
        {
            return Problem(result.Errors);
        }
        
        var response = MapBookingToBookingResponse(result.Value);
        
        return Ok(response);
    }
    
    [HttpGet("days")]
    public async Task<ActionResult<List<BookingDaysResponse>>> Get()
    {
        var bookings = await _bookingService.GetAllPossibleBookingDatesAsync();
        
        var listResponse = MapDictOfDateToListOfBookingDaysResponse(bookings);
        
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

        if (result.IsFailed) return Problem(result.Errors);

        var response = MapBookingToBookingResponse(result.Value);

        return Ok(response);
    }

    [HttpPut("{ticket:guid}")]
    public async Task<ActionResult<BookingResponse>> Put(Guid ticket, BookingRequest request)
    {
        var booking = MapBookingRequestToBooking(ticket, request);

        var result = await _bookingService.UpdateBookingAsync(ticket, booking);
        
        if (result.IsFailed) return Problem(result.Errors);

        var response = MapBookingToBookingResponse(booking);

        return Ok(response);
    }

    [HttpDelete("{ticket:guid}")]
    public async Task<ActionResult> Delete(Guid ticket)
    {
        var result = await _bookingService.CancelBookingAsync(ticket);
        
        if (result.IsFailed) return Problem(result.Errors);

        return NoContent();
    }

    private Booking MapBookingRequestToBooking(Guid ticket, BookingRequest request)
        => new Booking
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

    private BookingResponse MapBookingToBookingResponse(Booking booking)
        => new BookingResponse
        {
            Ticket = booking.Ticket,
            ClientFullName = booking.Client.FullName,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate
        };

    private List<BookingDaysResponse> MapDictOfDateToListOfBookingDaysResponse(Dictionary<DateOnly, bool> bookings)
        => bookings
            .Select(x => new BookingDaysResponse
            {
                Date = x.Key,
                Vacant = x.Value
            })
            .ToList();
}