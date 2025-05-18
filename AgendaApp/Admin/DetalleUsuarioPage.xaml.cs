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
                $"�Eliminar al usuario '{_usuarioOriginal.NombreUsuario}'?", "S�", "No");

            if (confirmado)
            {
                // Opci�n 1: Eliminaci�n f�sica
                // await _database.EliminarUsuarioAsync(_usuarioOriginal.Id);

                // Opci�n 2: Eliminaci�n l�gica (recomendada)
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