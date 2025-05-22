using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Travel_agency.BLL.Abstractions;
using Travel_agency.BLL.Services;
using Travel_agency.DataAccess;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TravelAgencyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Use Local or Default connection string 

builder.Services.AddScoped<IHotelBookingRepository, HotelBookingRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelRoomRepository, HotelRoomRepository>();
builder.Services.AddScoped<ITicketBookingRepository, TicketBookingRepository>();
builder.Services.AddScoped<ITourBookingRepository, TourBookingRepository>();
builder.Services.AddScoped<ITourRepository, TourRepository>();
builder.Services.AddScoped<ITransportRepository, TransportRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddScoped<IHotelBookingService, HotelBookingService>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<IHotelRoomService, HotelRoomService>();
builder.Services.AddScoped<ITicketBookingService, TicketBookingService>();
builder.Services.AddScoped<ITourBookingService, TourBookingService>();
builder.Services.AddScoped<ITourQueryService, TourQueryService>();
builder.Services.AddScoped<ITourService, TourService>();
builder.Services.AddScoped<ITransportService, TransportService>();
builder.Services.AddScoped<IUserService, UserService>();


builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TravelAgencyDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();