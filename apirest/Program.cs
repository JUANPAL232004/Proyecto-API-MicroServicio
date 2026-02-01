using apirest.interfaces; 
using apirest.Repositories;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using apirest.Interfaces;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "wwwroot" // Forzamos que busque la carpeta wwwroot
});

// 1. Configuración de JWT
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key) 
    };
});

// 2. Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de Dependencias
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEstadoRepository, EstadoRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();


var app = builder.Build(); 

app.UseFileServer();
// --- CONFIGURACIÓN DEL MIDDLEWARE ---

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// IMPORTANTE: DefaultFiles y StaticFiles SIEMPRE antes de MapControllers
app.UseDefaultFiles(); 
app.UseStaticFiles();

app.UseRouting(); // Agregamos routing explícito
app.UseCors("AllowAll"); 

app.UseAuthentication(); 
app.UseAuthorization();  

app.MapControllers();

// Si nada de lo anterior sirve (ej. entras a una ruta que no existe), carga el HTML
app.MapFallbackToFile("index.html");

app.Run();