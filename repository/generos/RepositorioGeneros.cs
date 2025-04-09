using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPIPeliculas.Entidades;
namespace MinimalAPIPeliculas.Repository.Generos
{
    public class RepositorioGeneros : IRepositorioGeneros
    {
        private readonly string? connectionString;

        public RepositorioGeneros(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CrearGenero(Genero genero)
        {
            using var conexion = new SqlConnection(connectionString);
            var id = await conexion.QuerySingleAsync<int>(@"
                INSERT INTO Generos (Nombre) VALUES (@Nombre);
                SELECT SCOPE_IDENTITY();", genero);
            genero.Id = id;
            return id;
        }

        public async Task<IEnumerable<Genero>> ObtenerTodos()
        {
            using var conexion = new SqlConnection(connectionString);
            return await conexion.QueryAsync<Genero>("SELECT * FROM Generos");
        }

        public async Task<Genero?> ObtenerPorId(int id)
        {
            using var conexion = new SqlConnection(connectionString);
            return await conexion.QueryFirstOrDefaultAsync<Genero>(
                "SELECT * FROM Generos WHERE Id = @Id", new { Id = id });
        }

        public async Task<bool> ActualizarGenero(Genero genero)
        {
            using var conexion = new SqlConnection(connectionString);
            var filas = await conexion.ExecuteAsync(
                "UPDATE Generos SET Nombre = @Nombre WHERE Id = @Id", genero);
            return filas > 0;
        }

        public async Task<bool> EliminarGenero(int id)
        {
            using var conexion = new SqlConnection(connectionString);
            var filas = await conexion.ExecuteAsync(
                "DELETE FROM Generos WHERE Id = @Id", new { Id = id });
            return filas > 0;
        }
    }
}
