using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using MinimalAPIPeliculas.Entidades;
using MinimalAPIPeliculas.Repository.Generos;

namespace MinimalAPIPeliculas.endpoints
{
    public static class GenerosEndpoints
    {
        public static RouteGroupBuilder MapGeneros ( this RouteGroupBuilder group){
            group.MapGet("/", ObtenerGeneros).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)));

// Obtener por ID
            group.MapGet("/{id:int}", FindbyId);

// Crear género
            group.MapPost("/", PostGeneros);

// Actualizar género
            group.MapPut("/{id:int}", UpdateGenero );

// Eliminar género
            group.MapDelete("/{id:int}", DeleteGenero );


            return group ; 
        }

        /////////////////////////////////////////////////////////////// metodos //////////////////////////////////////////////////////////////////////////
        static async Task<Ok<IEnumerable<Genero>>> ObtenerGeneros (IRepositorioGeneros repositorio)
        {
            var generos = await repositorio.ObtenerTodos();
            return TypedResults.Ok(generos);
        }

        static async Task<Results<Ok<Genero>, NotFound>> FindbyId (int id, IRepositorioGeneros repo)
        {
            var genero = await repo.ObtenerPorId(id);
            return genero is not null ? TypedResults.Ok(genero) : TypedResults.NotFound();
        }

        static async Task<Created<Genero>> PostGeneros (Genero genero, IRepositorioGeneros repo)
        {
            var id = await repo.CrearGenero(genero);
            return TypedResults.Created($"/{id}", genero);
        }

        static async Task<Results<NotFound, NoContent, StatusCodeHttpResult>> UpdateGenero (int id, Genero genero, IRepositorioGeneros repo)
        {
            var generoExistente = await repo.ObtenerPorId(id);
            if (generoExistente is null) return TypedResults.NotFound();

            genero.Id = id;
            var actualizado = await repo.ActualizarGenero(genero);
            return actualizado ? TypedResults.NoContent() : TypedResults.StatusCode(500);
        }

        static async Task<Results<NoContent, NotFound>> DeleteGenero (int id, IRepositorioGeneros repo)
        {
            var eliminado = await repo.EliminarGenero(id);
            return eliminado ? TypedResults.NoContent() : TypedResults.NotFound();
        }
    }

        
}