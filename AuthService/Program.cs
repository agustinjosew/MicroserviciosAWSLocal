var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/login", async (LoginRequest request) => 
    {
        // logica de la implementacion de login
        return Results.Ok(new { Message = "No se ha implementando el endpoint de login todavia." });
    })
    .WithName("Login")
    .WithTags("Authentication")
    .WithOpenApi();

app.Run();

record LoginRequest(string Username, string Password);