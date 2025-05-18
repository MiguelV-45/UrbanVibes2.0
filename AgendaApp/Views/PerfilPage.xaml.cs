using AgendaApp.Datos;
using AgendaApp.Modelos;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

namespace AgendaApp.Views
{
    public partial class PerfilPage : ContentPage
    {
        private BaseDatabase _database;
        private Usuario _usuario;

        public PerfilPage()
        {
            InitializeComponent();
            _database = new BaseDatabase();
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            await CargarDatosUsuario();
        }

        private async Task CargarDatosUsuario()
        {
            try
            {
                var idStr = await SecureStorage.GetAsync("UsuarioId");
                if (!string.IsNullOrEmpty(idStr) && int.TryParse(idStr, out int id))
                {
                    _usuario = await _database.ObtenerUsuarioPorIdAsync(id);
                    if (_usuario != null)
                    {
                        lblNombreUsuario.Text = _usuario.NombreUsuario;
                        lblEmail.Text = _usuario.Email;
                        lblFechaRegistro.Text = _usuario.FechaRegistro.ToString("dd/MM/yyyy");
                        lblActivo.Text = _usuario.Activo ? "Activo" : "Inactivo";
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo cargar el perfil: {ex.Message}", "OK");
            }
        }

        private async void OnEditarPerfilClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("editarperfil");
        }

        private async void OnCerrarSesionClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Cerrar sesión", "¿Deseas cerrar sesión?", "Sí", "No");
            if (confirm)
            {
                SecureStorage.Remove("UsuarioId");
                await Shell.Current.GoToAsync("//login"); // Navega al login
            }
        }
    }
}