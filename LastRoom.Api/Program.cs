using LastRoom.Api;
using LastRoom.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
var fullPath = Path.Join(path, builder.Configuration.GetConnectionString("LastRoom"));
var connectionString = $"Data Source={fullPath}";

builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IBookingService, BookingService>();

builder.Services.AddDbContext<LastRoomDbContext>(opt => 
    opt.UseSqlite(connectionString));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();