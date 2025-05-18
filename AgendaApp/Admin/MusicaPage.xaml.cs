using AgendaApp.Modelos;
using AgendaApp.Datos;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
            await CargarMusicasAsync();
        }

        private async Task CargarMusicasAsync()
        {
            var musicasLista = await _database.ObtenerMusicasAsync();
            Musicas.Clear();
            foreach (var musica in musicasLista)
                Musicas.Add(musica);
        }

        private async void OnCancionSeleccionada(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is not Musica cancionSeleccionada)
                return;

            // Pasar callback para actualizar la lista después de eliminar
            await Navigation.PushAsync(new DetalleMusicaPage(
                cancionSeleccionada,
                actualizarListaCallback: async () => await CargarMusicasAsync()));

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}