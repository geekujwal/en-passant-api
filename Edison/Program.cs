using Edison.Abstractions;
using Edison.Consumer;
using Edison.Producer;
using Edison.Services;
using MassTransit;
using StackExchange.Redis;
using Stella.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RoutePrefixConvention("api/edison"));
});

builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddHttpClient();
builder.Services.AddTransient<MessageProducer>();
builder.Services.AddSingleton<IWebSocketService, WebSocketService>();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<HelloConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("hello_queue", e =>
        {
            e.ConfigureConsumer<HelloConsumer>(context);
        });
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();
var webSocket = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};

webSocket.AllowedOrigins.Add("https://localhost:5173");
app.UseWebSockets(webSocket);


app.MapControllers();


app.Run();
