using AgendaApp.Modelos;
using AgendaApp.Datos;
using System.Collections.ObjectModel;

namespace AgendaApp.Admin
{
    public partial class UsuariosPage : ContentPage
    {
        public ObservableCollection<Usuario> Usuarios { get; set; }
        private BaseDatabase _database;

        public UsuariosPage()
        {
            InitializeComponent();

            _database = new BaseDatabase();
            Usuarios = new ObservableCollection<Usuario>();
            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var usuariosLista = await _database.ObtenerUsuariosAsync();
            Usuarios.Clear();
            foreach (var usuario in usuariosLista)
                Usuarios.Add(usuario);
        }

        private async void UsuariosCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var usuarioSeleccionado = e.CurrentSelection.FirstOrDefault() as Usuario;
            if (usuarioSeleccionado == null)
                return;

            bool confirmar = await DisplayAlert("Eliminar", $"¿Eliminar a {usuarioSeleccionado.NombreUsuario}?", "Sí", "No");
            if (confirmar)
            {
                int resultado = await _database.EliminarUsuarioAsync(usuarioSeleccionado.Id);

                if (resultado > 0)
                {
                    Usuarios.Remove(usuarioSeleccionado);
                    await DisplayAlert("Eliminado", "Usuario eliminado.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo eliminar el usuario.", "OK");
                }
            }

            UsuariosCollectionView.SelectedItem = null;
        }
    }
}