using Edison.Consumer;
using MassTransit;
using System;
using System.Threading.Tasks;

namespace Edison.Producer
{
    public class MessageProducer(IBus bus)
    {
        private readonly IBus _bus = bus;

        public async Task SendMessage()
        {
            var endpoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://localhost/hello_queue"));
            await endpoint.Send<GameMessage>(new { Text = "Hello from MassTransit!" });
            Console.WriteLine("Message sent!");
        }
    }
}