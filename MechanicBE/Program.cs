using FluentValidation;
using FluentValidation.AspNetCore;
using MechanicBE.Contexts;
using MechanicBE.Models.Dto;
using MechanicBE.Services;
using MechanicBE.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<IWorkHourEstimator, WorkHourEstimationService>();

builder.Services.AddScoped<IValidator<CreateClient>, CreateClientValidator>();
builder.Services.AddScoped<IValidator<UpdateClient>, UpdateClientValidator>();
builder.Services.AddScoped<IValidator<CreateCommission>, CreateCommissionValidator>();
builder.Services.AddScoped<IValidator<UpdateCommission>, UpdateCommissionValidator>();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICommissionService, CommissionService>();

builder.Services.AddDbContext<MechanicContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLite"))
);

var app = builder.Build();

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