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

// Configuración de CORS general
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddControllers();

// Configura Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS específica para Blazor
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

// Usa la política CORS general
app.UseCors("PermitirTodo");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usa la política CORS específica para Blazor
app.UseCors("AllowBlazorFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run("http://localhost:5950");

