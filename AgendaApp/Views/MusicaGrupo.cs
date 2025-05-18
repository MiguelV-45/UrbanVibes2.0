using System.Collections.ObjectModel;

namespace AgendaApp.Modelos
{
    public class MusicaGrupo : ObservableCollection<Musica>
    {
        public string Genero { get; set; }

        public MusicaGrupo(string genero, ObservableCollection<Musica> musicas) : base(musicas)
        {
            Genero = genero;
        }
    }
}
