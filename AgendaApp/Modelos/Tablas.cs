using SQLite;
using System;

namespace AgendaApp.Modelos
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Telefono { get; set; }

        [Unique]
        public string NombreUsuario { get; set; }

        [Unique]
        public string Email { get; set; }

        public string Password { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool Activo { get; set; } = true;
    }

    public class Musica
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Artista { get; set; }
        public string Genero { get; set; }
        public string RutaArchivo { get; set; }
    }
}