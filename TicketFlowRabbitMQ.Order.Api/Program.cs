using Data.RabbitMQ; // <- onde está o RabbitMQPublisher
using Microsoft.AspNetCore.Mvc;
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
builder.Services.AddDbContext<TicketFlowContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("TicketFlowDbConnection")));

// ==== Repositórios e Serviços
builder.Services.AddScoped<IFlowRepository, FlowRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// ==== RabbitMQ
builder.Services.AddSingleton<IEventBus, RabbitMQPublisher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/scalar/v1/test", () => "API rodando!");
app.MapGet("/test-di", ([FromServices] IOrderService service) =>
{
    return Results.Ok($"Service injetado: {service.GetType().FullName}");
});


app.Run();
