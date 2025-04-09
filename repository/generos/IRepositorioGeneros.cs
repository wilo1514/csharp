using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalAPIPeliculas.Entidades;

namespace MinimalAPIPeliculas.Repository.Generos
{
    using MinimalAPIPeliculas.Entidades;

    public interface IRepositorioGeneros
    {
        Task<int> CrearGenero(Genero genero);
        Task<IEnumerable<Genero>> ObtenerTodos();
        Task<Genero?> ObtenerPorId(int id);
        Task<bool> ActualizarGenero(Genero genero);
        Task<bool> EliminarGenero(int id);
    }
}