using FluentValidation;
using FluentValidation.AspNetCore;
using System.Reflection;
using Tedee.Models;
using Tedee.Models.Validators;
using Microsoft.EntityFrameworkCore;
using Tedee;
using Tedee.Repositories.Interfaces;
using Tedee.Repositories;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMvc();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpResponseExceptionFilter>();
});
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;
//});
builder.Services.AddDbContext<TedeeContext>(opt =>
    opt.UseInMemoryDatabase("TedeeList"));

builder.Services.AddScoped<IValidator<BaseTrip>, TripValidator>();
builder.Services.AddScoped<IValidator<BaseRegisteredEmail>, RegisteredEmailValidator>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
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
