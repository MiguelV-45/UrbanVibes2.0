using AgendaApp.Datos;
using AgendaApp.Modelos;
using System.Collections.ObjectModel;

namespace AgendaApp.Admin
{
    public partial class UsuariosPage : ContentPage
    {
        private readonly BaseDatabase _database;
        private ObservableCollection<Usuario> _usuarios;

        public bool IsRefreshing { get; set; }
        public Command RefreshCommand { get; }

        public UsuariosPage()
        {
            InitializeComponent();
            _database = new BaseDatabase();

            RefreshCommand = new Command(async () => await LoadUsuariosAsync());
            BindingContext = this;

            LoadUsuariosAsync();
        }

        private async Task LoadUsuariosAsync()
        {
            try
            {
                IsRefreshing = true;
                OnPropertyChanged(nameof(IsRefreshing));

                var usuarios = await _database.ObtenerUsuariosAsync();
                _usuarios = new ObservableCollection<Usuario>(usuarios);
                usuariosCollectionView.ItemsSource = _usuarios;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar usuarios: {ex.Message}", "OK");
            }
            finally
            {
                IsRefreshing = false;
                OnPropertyChanged(nameof(IsRefreshing));
                refreshView.IsRefreshing = false;
            }
        }

        private async void OnUsuarioSeleccionado(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is Usuario usuarioSeleccionado)
            {
                await Navigation.PushAsync(new DetalleUsuarioPage(usuarioSeleccionado));
                usuariosCollectionView.SelectedItem = null;
            }
        }
    }
}