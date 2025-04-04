using Microsoft.AspNetCore.Cors;
using MinimalAPIPeliculas.Entidades;

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

var app = builder.Build(); 

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
 // middlewares
app.MapGet("/", [EnableCors(PolicyName ="libre")] () => "Hello World!");


app.MapGet("/generos", ()=>{
    var generos = new List<Genero>{
        new Genero
        {
            Id =1,
            Nombre = "Drama"
        },
        new Genero
        {
            Id =2,
            Nombre = "Action"
        },new Genero
        {
            Id =3,
            Nombre = "Comedy"
        },
    };
    return generos;
}).CacheOutput(c =>c.Expire(TimeSpan.FromSeconds(15)));

//midlewares

app.Run();
