using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
Console.WriteLine($"Issuer={builder.Configuration["Auth0:Issuer"]}");
Console.WriteLine($"Audience={builder.Configuration["Auth0:Audience"]}");
Console.WriteLine($"Issuer1={builder.Configuration["Auth0.Issuer"]}");
Console.WriteLine($"Audience1={builder.Configuration["Auth0.Audience"]}");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth0:Issuer"];
        options.Audience = builder.Configuration["Auth0:Audience"];

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };
    });



//builder.Services.AddAuthorization();
//configure authorization (defining policies)
builder.Services.AddAuthorization(options =>
{
    // Read Policy
    options.AddPolicy("ReadOrdersPolicy", policy =>
        policy.RequireAuthenticatedUser()
            .RequireClaim("scope", "read:orders"));

    // Write Policy
    options.AddPolicy("WriteOrdersPolicy", policy =>
        policy.RequireAuthenticatedUser()
            .RequireClaim("scope", "write:orders"));

    // Admin only policy
    options.AddPolicy("AdminOnlyPolicy", policy =>
        policy.RequireAuthenticatedUser()
            .RequireRole("Admin"));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.RequireAuthorization();

app.MapGet("/", () => Results.Ok());

app.MapGet("/api/orders", () =>
{
    var sampleOrders = new[] { new { Id = 1, Item = "Laptop" } };
    return Results.Ok(sampleOrders);
})
.RequireAuthorization("ReadOrdersPolicy");

app.MapPost("/api/orders", () =>
{
    return Results.Ok($"hi");
})
.RequireAuthorization("WriteOrdersPolicy");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
