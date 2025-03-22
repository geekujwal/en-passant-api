using Stella.Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new RoutePrefixConvention("api/lilith"));
});

// todo save this in secrets
builder.Services.AddJwtAuthentication("your-256-bit-secretyour-256-bit-secretyour-256-bit-secretyour-256-bit-secret", "example", "example");
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
