using apirest.interfaces;
using apirest.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEstadoRepository, EstadoRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

app.MapGet("/producto", async (IProductoRepository _repo) => 
{
    var productos = await _repo.ObtenerTodos();
    return Results.Ok(productos);
})
.WithName("GetProductos")
.WithOpenApi();

app.MapGet("/Usuario", async (IUsuarioRepository _repo) => 
{
    var usuarios = await _repo.ObtenerTodos();
    return Results.Ok(usuarios);
})
.WithName("GetUsuarios")
.WithOpenApi();

app.MapGet("/Estado", async (IEstadoRepository _repo) => 
{
    var estado = await _repo.ObtenerTodos();
    return Results.Ok(estado);
})
.WithName("GetEstado")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}