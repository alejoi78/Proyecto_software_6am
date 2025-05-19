using Microsoft.EntityFrameworkCore;
using Proyecto_software_6am.DAOs;
using Proyecto_software_6am.DAOs.Interfaces;
using System;
using Microsoft.AspNetCore.Mvc;
using Proyecto_software_6am.Utilidades;
using MySql.Data.MySqlClient;
using Proyecto_software_6am.Servicio;
using Proyecto_software_6am.Servicio.Interfaces;
using Proyecto_software_6am.Entidades;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Configura Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS (esto es lo que necesitas añadir)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorFrontend",
        policy => policy.WithOrigins(
                    "https://localhost:7133",  // URL de tu frontend Blazor (HTTPS)
                    "http://localhost:5133",   // URL alternativa (HTTP)
                    "https://localhost:5001",  // Posibles URLs adicionales
                    "http://localhost:5000")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials());
});

// Configuración de MySQL
var mySQLConfiguration = new MySQLConfiguration(builder.Configuration.GetConnectionString("MySqlConnection"));
builder.Services.AddSingleton<MySQLConfiguration>(mySQLConfiguration);

// Registra el DAO y servicios
builder.Services.AddScoped<IPeliculaDAO, PeliculaDAO>();
builder.Services.AddScoped<IPeliculaNegocio, PeliculaServicio>();
builder.Services.AddScoped<ISerieDAO, SerieDAO>();
builder.Services.AddScoped<ISerieNegocio, SerieNegocio>();
builder.Services.AddScoped<IUsuarioDAO, UsuarioDAO>();
builder.Services.AddScoped<IUsuarioNegocio, UsuarioNegocio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Añade el middleware CORS (importante que esté en esta posición)
app.UseCors("AllowBlazorFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run("http://localhost:5950");