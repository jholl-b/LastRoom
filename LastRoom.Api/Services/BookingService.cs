﻿using FluentResults;
using LastRoom.Api.Errors.Booking;
using LastRoom.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LastRoom.Api.Services;

public class BookingService : IBookingService
{
    private readonly LastRoomDbContext _dbContext;
    private readonly IDateTimeProvider _date;

    public BookingService(LastRoomDbContext dbContext, IDateTimeProvider date)
    {
        _dbContext = dbContext;
        _date = date;
    }

    public async Task<Result<Booking>> GetBookingAsync(Guid ticket)
    {
        var booking = await GetABookingByTicketAsync(ticket);

        if (booking is null)
            return Result.Fail(new BookingNotFoundError());

        return booking;
    }
    
    public async Task<List<Booking>> GetAllBookingsAsync()
    {
        var bookings = await _dbContext
            .Bookings
            .Include(x => x.Client)
            .AsNoTracking()
            .ToListAsync();

        return bookings;
    }

    public async Task<Result<Booking>> CreateNewBookingAsync(
        string clientIdentification, 
        string clientFullName, 
        DateOnly checkInDate, 
        DateOnly checkOutDate)
    {
        var reserved = _dbContext
            .Bookings
            .Any(x => x.CheckInDate <= checkOutDate && checkInDate <= x.CheckOutDate );
        
        if (reserved)
            return Result.Fail(new RoomAlreadyBookedError());

        var validation = ValidateBooking(checkInDate, checkOutDate);
        if (validation.IsFailed)
            return validation;

        var booking = new Booking
        {
            Ticket = Guid.NewGuid(),
            Client = new Client
            {
                Identification = clientIdentification,
                FullName = clientFullName
            },
            CheckInDate = checkInDate,
            CheckOutDate = checkOutDate
        };

        await _dbContext.AddAsync(booking);
        await _dbContext.SaveChangesAsync();

        return booking;
    }

    public async Task<Result> UpdateBookingAsync(Guid ticket, Booking booking)
    {
        var dbBooking = await GetABookingByTicketAsync(ticket);

        if (dbBooking is null)
            return Result.Fail(new BookingNotFoundError());
        
        var reserved = _dbContext
            .Bookings
            .Any(x => x.CheckInDate <= booking.CheckInDate 
                      && booking.CheckOutDate <= x.CheckOutDate
                      && x.Ticket != booking.Ticket);
        
        if (reserved)
            return Result.Fail(new RoomAlreadyBookedError());
        
        var validation = ValidateBooking(booking.CheckInDate, booking.CheckOutDate);
        if (validation.IsFailed)
            return validation;

        dbBooking.Client.FullName = booking.Client.FullName;
        dbBooking.Client.Identification = booking.Client.Identification;
        dbBooking.CheckInDate = booking.CheckInDate;
        dbBooking.CheckOutDate = booking.CheckOutDate;

        await _dbContext.SaveChangesAsync();

        return Result.Ok();
    }

    public async Task<Result> CancelBooking(Guid ticket)
    {
        var dbBooking = await GetABookingByTicketAsync(ticket);

        if (dbBooking is null)
            return Result.Fail(new BookingNotFoundError());

        _dbContext.Remove(dbBooking);
        await _dbContext.SaveChangesAsync();

        return Result.Ok();
    }

    private async Task<Booking?> GetABookingByTicketAsync(Guid ticket)
    {
        return await _dbContext
            .Bookings
            .Include(x => x.Client)
            .SingleOrDefaultAsync(x => x.Ticket == ticket);
    }

    private Result ValidateBooking(DateOnly checkInDate, DateOnly checkOutDate)
    {
        var period = checkOutDate.DayNumber - checkInDate.DayNumber;
        if (period >= 3)
            return Result.Fail(new StayPeriodTooLongError());
        
        if (checkInDate.DayNumber - _date.DateOnlyUtcNow.DayNumber > 30)
            return Result.Fail(new BookingDateTooFarError());

        if (checkInDate.DayNumber - _date.DateOnlyUtcNow.DayNumber < 1)
            return Result.Fail(new StartDateNotAllowedError());

        return Result.Ok();
    }
}