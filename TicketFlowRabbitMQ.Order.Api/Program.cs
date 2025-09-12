using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TicketFlowRabbitMQ.Order.Application.Interfaces;
using TicketFlowRabbitMQ.Order.Application.Services;
using TicketFlowRabbitMQ.Order.Data.Context;
using TicketFlowRabbitMQ.Order.Data.Repository;
using TicketFlowRabbitMQ.Order.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ==== Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ==== DBContext
builder.Services.AddDbContext<TicketFlowContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("TicketFlowDbConnection")));

// ==== Mapping interfece X service
builder.Services.AddScoped<IFlowRepository, FlowRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/scalar/v1/test", () => "API rodando!");


app.Run();
