using MassTransit;
using System;
using System.Threading.Tasks;

namespace Edison.Consumer
{
    public class GameMessage
    {
        public string Text { get; set; }
    }

    public class HelloConsumer : IConsumer<GameMessage>
    {
        public async Task Consume(ConsumeContext<GameMessage> context)
        {
            Console.WriteLine($"Received: {context.Message.Text}");
            await Task.CompletedTask;
        }
    }
}