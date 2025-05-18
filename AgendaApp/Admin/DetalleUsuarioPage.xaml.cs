using AgendaApp.Datos;
using AgendaApp.Modelos;

namespace AgendaApp.Admin
{
    public partial class DetalleUsuarioPage : ContentPage
    {
        private BaseDatabase _database = new BaseDatabase();
        private Usuario _usuarioOriginal;

        public DetalleUsuarioPage(Usuario usuario)
        {
            InitializeComponent();
            _usuarioOriginal = usuario;
            BindingContext = _usuarioOriginal;
        }

        private async void OnEliminarClicked(object sender, EventArgs e)
        {
            bool confirmado = await DisplayAlert("Confirmar",
                $"¿Eliminar al usuario '{_usuarioOriginal.NombreUsuario}'?", "Sí", "No");

            if (confirmado)
            {
                // Opción 1: Eliminación física
                // await _database.EliminarUsuarioAsync(_usuarioOriginal.Id);

                // Opción 2: Eliminación lógica (recomendada)
                _usuarioOriginal.Activo = false;
                await _database.GuardarUsuarioAsync(_usuarioOriginal);

                await Navigation.PopAsync();
            }
        }

        private async void OnGuardarCambiosClicked(object sender, EventArgs e)
        {
            _usuarioOriginal.NombreUsuario = NombreUsuarioEntry.Text;
            _usuarioOriginal.Nombre = NombreEntry.Text;
            _usuarioOriginal.Email = EmailEntry.Text;
            _usuarioOriginal.Telefono = TelefonoEntry.Text;

            await _database.GuardarUsuarioAsync(_usuarioOriginal);
            await Navigation.PopAsync();
        }
    }
}