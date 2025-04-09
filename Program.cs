using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIPeliculas.endpoints;
using MinimalAPIPeliculas.Entidades;
using MinimalAPIPeliculas.Repository.Generos;

var builder = WebApplication.CreateBuilder(args);
var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!;
//servicios 

builder.Services.AddCors(opciones=>
{
    opciones.AddDefaultPolicy(configuracion=>
    {
        configuracion.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });
    opciones.AddPolicy("libre",configuracion=>{
        configuracion.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

//servicios
builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IRepositorioGeneros, RepositorioGeneros>();

var app = builder.Build(); 

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
 // middlewares
app.MapGet("/", [EnableCors(PolicyName ="libre")] () => "Hello World!");

app.MapGroup("/generos").MapGeneros();

app.Run();