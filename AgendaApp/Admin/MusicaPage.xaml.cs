using AgendaApp.Modelos;
using AgendaApp.Datos;
using System.Collections.ObjectModel;

namespace AgendaApp.Admin
{
    public partial class MusicaPage : ContentPage
    {
        public ObservableCollection<Musica> Musicas { get; set; }
        private BaseDatabase _database;

        public MusicaPage()
        {
            InitializeComponent();

            _database = new BaseDatabase();
            Musicas = new ObservableCollection<Musica>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var musicasLista = await _database.ObtenerMusicasAsync();
            Musicas.Clear();
            foreach (var musica in musicasLista)
                Musicas.Add(musica);
        }

        private async void MusicaCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cancionSeleccionada = e.CurrentSelection.FirstOrDefault() as Musica;
            if (cancionSeleccionada == null)
                return;

            bool confirmacion = await DisplayAlert("Eliminar Canción",
                $"¿Deseas eliminar '{cancionSeleccionada.Titulo}' de {cancionSeleccionada.Artista}?",
                "Sí", "No");

            if (confirmacion)
            {
                int resultado = await _database.EliminarMusicaAsync(cancionSeleccionada.Id);
                if (resultado > 0)
                {
                    Musicas.Remove(cancionSeleccionada);
                    await DisplayAlert("Éxito", "Canción eliminada correctamente.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo eliminar la canción.", "OK");
                }
            }

            MusicaCollectionView.SelectedItem = null;
        }
    }
}