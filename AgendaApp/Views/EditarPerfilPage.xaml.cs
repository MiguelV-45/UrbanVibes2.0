using AgendaApp.Datos;
using AgendaApp.Modelos;
using System;
using System.Threading.Tasks;

namespace AgendaApp.Views
{
    public partial class EditarPerfilPage : ContentPage
    {
        private BaseDatabase _database;
        private Usuario _usuarioActual;

        public EditarPerfilPage()
        {
            InitializeComponent();
            _database = new BaseDatabase();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CargarDatosUsuario();
        }

        private async Task CargarDatosUsuario()
        {
            try
            {
                var idStr = await SecureStorage.GetAsync("UsuarioId");
                if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
                {
                    _usuarioActual = await _database.ObtenerUsuarioPorIdAsync(id);
                    if (_usuarioActual != null)
                    {
                        entryNombre.Text = _usuarioActual.Nombre;
                        entryUsuario.Text = _usuarioActual.NombreUsuario;
                        entryEmail.Text = _usuarioActual.Email;
                        entryTelefono.Text = _usuarioActual.Telefono;
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo cargar el usuario: {ex.Message}", "OK");
            }
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            if (_usuarioActual == null)
            {
                await DisplayAlert("Error", "Usuario no cargado.", "OK");
                return;
            }

            _usuarioActual.Nombre = entryNombre.Text?.Trim();
            _usuarioActual.NombreUsuario = entryUsuario.Text?.Trim();
            _usuarioActual.Email = entryEmail.Text?.Trim();
            _usuarioActual.Telefono = entryTelefono.Text?.Trim();

            try
            {
                var resultado = await _database.GuardarUsuarioAsync(_usuarioActual);
                if (resultado > 0)
                {
                    await DisplayAlert("Éxito", "Perfil actualizado correctamente.", "OK");
                    await Shell.Current.GoToAsync(".."); // Regresar a la página anterior
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo actualizar el perfil.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
        private async void OnEditarPerfilClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("editarperfil");
        }
    }
}