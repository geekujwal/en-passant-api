using Edison.Consumer;
using Edison.GameHub;
using Edison.Producer;
using MassTransit;
using Stella.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RoutePrefixConvention("api/edison"));
});

builder.Services.AddSignalR();
builder.Services.AddTransient<MessageProducer>();


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

// Configure the HTTP request pipeline.
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

app.MapHub<GameHub>("/game");

app.Run();
