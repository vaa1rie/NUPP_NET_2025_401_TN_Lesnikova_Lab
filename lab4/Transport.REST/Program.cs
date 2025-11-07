using Transport.Infrastructure.Repositories;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Transport.Infrastructure;
using Transport.Infrastructure.Models;
using Transport.Infrastructure.Repositories;
using Transport.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TransportContext>(options =>
    options.UseSqlite("Data Source=transport.db"));

builder.Services.AddScoped<IRepository<Bus>, BusRepository>();
builder.Services.AddScoped<IRepository<Tram>, TramRepository>();
builder.Services.AddScoped<IRepository<Trolleybus>, TrolleybusRepository>();
builder.Services.AddScoped<IRepository<Vehicle>, VehicleRepository<Vehicle>>();
builder.Services.AddScoped<IRepository<Transport.Infrastructure.Models.Route>, RouteRepository>();
builder.Services.AddScoped<IRepository<MaintenanceRecord>, MaintenanceRecordRepository>();
builder.Services.AddScoped<IRepository<TechnicalPassport>, TechnicalPassportRepository>();
builder.Services.AddScoped<IRepository<RouteVehicle>, RouteVehicleRepository>();

builder.Services.AddScoped<ICrudServiceAsync<Bus>, BusService>();
builder.Services.AddScoped<ICrudServiceAsync<Tram>, TramService>();
builder.Services.AddScoped<ICrudServiceAsync<Trolleybus>, TrolleybusService>();
builder.Services.AddScoped<ICrudServiceAsync<Vehicle>, VehicleService>();
builder.Services.AddScoped<ICrudServiceAsync<Transport.Infrastructure.Models.Route>, BaseCrudService<Transport.Infrastructure.Models.Route>>();
builder.Services.AddScoped<ICrudServiceAsync<MaintenanceRecord>, BaseCrudService<MaintenanceRecord>>();
builder.Services.AddScoped<ICrudServiceAsync<TechnicalPassport>, BaseCrudService<TechnicalPassport>>();
builder.Services.AddScoped<ICrudServiceAsync<RouteVehicle>, BaseCrudService<RouteVehicle>>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
