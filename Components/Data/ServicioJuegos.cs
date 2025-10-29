using Microsoft.Data.Sqlite;

namespace blazor.Components.Data
{
    public class ServicioJuegos
    {

        private List<Juego> juegos = new List<Juego>
        {
            new Juego{Identificador=1,Nombre="Ravel",Jugado=false},
            new Juego{Identificador=2,Nombre="Carcassonne",Jugado=true}
        };
        public async Task<List<Juego>> ObtenerJuegos()
        {
            juegos.Clear();
            string ruta = "mibase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();


            var comando = conexion.CreateCommand();
            comando.CommandText = "SELECT IDENTIFICADOR,NOMBRE,JUGADO FROM JUEGOS";
            using var lector = await comando.ExecuteReaderAsync();
            while (await lector.ReadAsync())
            {
                juegos.Add(new Juego
                {

                    Identificador = lector.GetInt32(0),
                    Nombre = lector.GetString(1),
                    Jugado = lector.GetInt32(2) == 0 ? false : true
                });
            }
            return juegos;
        }
        public  async Task AgregarJuego(Juego juego)

        {
            string ruta = "mibase.db";
            using var conexion = new SqliteConnection($"DataSource={ruta}");
            await conexion.OpenAsync();


            var comando = conexion.CreateCommand();
            comando.CommandText = "insert into juegos (identificador,nombre,jugado) values ($IDENTIFICADOR,$NOMBRE,$JUGADO)";
            comando.Parameters.AddWithValue("$IDENTIFICADOR", juego.Identificador);
            comando.Parameters.AddWithValue("$NOMBRE",juego.Nombre);
            comando.Parameters.AddWithValue("JUGADO", juego.Jugado ? 1:0);
            comando.ExecuteNonQueryAsync();
            juegos.Add(juego);
           
        }
      
    }
}
