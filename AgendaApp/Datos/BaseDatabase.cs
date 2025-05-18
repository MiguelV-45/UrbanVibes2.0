// Datos/BaseDatabase.cs
using SQLite;
using AgendaApp.Modelos;
using System.Diagnostics;


namespace AgendaApp.Datos
{
    public class BaseDatabase
    {
        private SQLiteAsyncConnection _db;

        public BaseDatabase()
        {
            Inicializar();
        }

        private async void Inicializar()
        {
            if (_db != null) return;

            try
            {
                var rutaDB = Path.Combine(FileSystem.AppDataDirectory, "agendaapp.db");
                _db = new SQLiteAsyncConnection(rutaDB);

                // Crear todas las tablas necesarias
                await _db.CreateTableAsync<Usuario>();
                await _db.CreateTableAsync<Musica>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al inicializar DB: {ex.Message}");
            }
        }

        // Métodos para Usuario
        public async Task<Usuario> ObtenerUsuarioAsync(string nombreUsuario)
        {
            try
            {
                return await _db.Table<Usuario>()
                    .Where(u => u.NombreUsuario == nombreUsuario && u.Activo)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener usuario: {ex.Message}");
                return null;
            }
        }

        public async Task<int> GuardarUsuarioAsync(Usuario usuario)
        {
            try
            {
                return usuario.Id == 0 ?
                    await _db.InsertAsync(usuario) :
                    await _db.UpdateAsync(usuario);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al guardar usuario: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> ExisteUsuarioAsync(string nombreUsuario)
        {
            try
            {
                return await _db.Table<Usuario>()
                    .Where(u => u.NombreUsuario == nombreUsuario)
                    .CountAsync() > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al verificar usuario: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            try
            {
                return await _db.Table<Usuario>()
                    .Where(u => u.Email == email)
                    .CountAsync() > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al verificar email: {ex.Message}");
                return false;
            }
        }

        public async Task<Usuario> ObtenerUsuarioPorIdAsync(int id)
        {
            try
            {
                return await _db.Table<Usuario>()
                    .Where(u => u.Id == id && u.Activo)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener usuario por ID: {ex.Message}");
                return null;
            }
        }

        public async Task<int> EliminarUsuarioAsync(int id)
        {
            try
            {
                return await _db.DeleteAsync<Usuario>(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al eliminar usuario: {ex.Message}");
                return 0;
            }
        }

        // ✅ Nuevo método para obtener todos los usuarios
        public async Task<List<Usuario>> ObtenerUsuariosAsync()
        {
            try
            {
                return await _db.Table<Usuario>().Where(u => u.Activo).ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener usuarios: {ex.Message}");
                return new List<Usuario>();
            }
        }

        // Métodos para Musica
        public async Task<List<Musica>> ObtenerMusicasAsync()
        {
            try
            {
                return await _db.Table<Musica>().ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener músicas: {ex.Message}");
                return new List<Musica>();
            }
        }

        public async Task<int> GuardarMusicaAsync(Musica musica)
        {
            try
            {
                return musica.Id == 0 ?
                    await _db.InsertAsync(musica) :
                    await _db.UpdateAsync(musica);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al guardar música: {ex.Message}");
                return 0;
            }
        }

        public async Task<int> EliminarMusicaAsync(int id)
        {
            try
            {
                // Verifica que la conexión esté inicializada
                if (_db == null)
                {
                    Debug.WriteLine("Error: La conexión a la base de datos es nula");
                    return 0;
                }

                // Verifica que el ID sea válido
                if (id <= 0)
                {
                    Debug.WriteLine($"Error: ID inválido ({id})");
                    return 0;
                }

                // Ejecuta la eliminación
                var resultado = await _db.DeleteAsync<Musica>(id);
                Debug.WriteLine($"Resultado de eliminación: {resultado} fila(s) afectada(s)");

                return resultado;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error completo al eliminar: {ex.ToString()}");
                return 0;
            }
        }
    }
}
