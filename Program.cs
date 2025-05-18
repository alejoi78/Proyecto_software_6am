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
builder.Services.AddSwaggerGen(); // Requiere el paquete Swashbuckle.AspNetCore

// Configuración de MySQL (asegúrate de que la clase MySQLConfiguration exista)
var mySQLConfiguration = new MySQLConfiguration(builder.Configuration.GetConnectionString("MySqlConnection"));
builder.Services.AddSingleton<MySQLConfiguration>(mySQLConfiguration); // Corregido: sintaxis de AddSingleton

// Registra el DAO y servicios

builder.Services.AddScoped<IPeliculaDAO, PeliculaDAO>();
builder.Services.AddScoped<IPeliculaNegocio, PeliculaServicio>();
builder.Services.AddScoped<ISerieDAO, SerieDAO>();
builder.Services.AddScoped<ISerieNegocio, SerieNegocio>();
builder.Services.AddScoped<IUsuarioDAO, UsuarioDAO>();
builder.Services.AddScoped<IUsuarioNegocio, UsuarioNegocio>();


var app = builder.Build();
// Configure the HTTP request pipeline.

// Usa la política CORS antes de `UseAuthorization`
app.UseCors("PermitirTodo");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
/*
app.MapGet("/dbconexion", async ([FromServices] DAOsContext dbContext) =>
{
    dbContext.Database.EnsureCreated();
    return Results.Ok("Base de datos en memoria: " + dbContext.Database.IsInMemory());

});
*/

app.Run("http://localhost:5950");
