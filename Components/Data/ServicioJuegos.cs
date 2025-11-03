using Microsoft.Data.Sqlite;

namespace blazor.Components.Data
{
    public class ServicioJuegos
    {
        private List<Juego> juegos = new List<Juego>();

        public async Task<List<Juego>> ObtenerJuegos()
        {
            juegos.Clear();
            string ruta = "mibase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = "SELECT IDENTIFICADOR, NOMBRE, JUGADO FROM JUEGOS";
            using var lector = await comando.ExecuteReaderAsync();

            while (await lector.ReadAsync())
            {
                juegos.Add(new Juego
                {
                    Identificador = lector.GetInt32(0),
                    Nombre = lector.GetString(1),
                    Jugado = lector.GetInt32(2) != 0
                });
            }

            return juegos;
        }

        public async Task AgregarJuego(Juego juego)
        {
            string ruta = "mibase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = "INSERT INTO JUEGOS (IDENTIFICADOR, NOMBRE, JUGADO) VALUES ($IDENTIFICADOR, $NOMBRE, $JUGADO)";
            comando.Parameters.AddWithValue("$IDENTIFICADOR", juego.Identificador);
            comando.Parameters.AddWithValue("$NOMBRE", juego.Nombre);
            comando.Parameters.AddWithValue("$JUGADO", juego.Jugado ? 1 : 0);
            await comando.ExecuteNonQueryAsync();

            juegos.Add(juego);
        }

        public async Task EliminarJuego(Juego juego)
        {
            string ruta = "mibase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = "DELETE FROM JUEGOS WHERE IDENTIFICADOR = $IDENTIFICADOR";
            comando.Parameters.AddWithValue("$IDENTIFICADOR", juego.Identificador);
            await comando.ExecuteNonQueryAsync();

            juegos.RemoveAt(juego.Identificador-1);
        }
        public async Task ActualizarJuego(Juego juego)
        {
            string ruta = "mibase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();

            var comando = conexion.CreateCommand();
            comando.CommandText = "UPDATE JUEGOS SET NOMBRE = $NOMBRE, JUGADO = $JUGADO WHERE IDENTIFICADOR = $IDENTIFICADOR";
            comando.Parameters.AddWithValue("$NOMBRE", juego.Nombre);
            comando.Parameters.AddWithValue("$JUGADO", juego.Jugado ? 1 : 0);
            comando.Parameters.AddWithValue("$IDENTIFICADOR", juego.Identificador);
            await comando.ExecuteNonQueryAsync();

            var index = juegos.FindIndex(x => x.Identificador == juego.Identificador);
            if (index >= 0)
            {
                juegos[index].Nombre = juego.Nombre;
                juegos[index].Jugado = juego.Jugado;
            }
        }
    }
}

