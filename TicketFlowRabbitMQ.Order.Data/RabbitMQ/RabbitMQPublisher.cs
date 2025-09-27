using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TicketFlowRabbitMQ.Order.Domain.Interfaces;

namespace Data.RabbitMQ; // Camada Data

// Implementa a interface de domínio IEventBus
public class RabbitMQPublisher : IEventBus
{
    private const string ExchangeName = "ticketflow.exchange";


    async public void Publish<T>(T @event) where T : IDomainEvent
    {
        //------------------------------------------------------------------------------------------------
        // Config
        //------------------------------------------------------------------------------------------------
        var eventName = @event.GetType().Name;
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest",
        };

        using var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(eventName, false, false, false, null);

        //------------------------------------------------------------------------------------------------
        // Set message
        //------------------------------------------------------------------------------------------------
        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);

        //------------------------------------------------------------------------------------------------
        // Set Publish
        //------------------------------------------------------------------------------------------------
        await channel.BasicPublishAsync("", eventName, body);
    }
}