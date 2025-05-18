// Views/RegistroPage.xaml.cs
using AgendaApp.Datos;
using AgendaApp.Modelos;

namespace AgendaApp.Views
{
    public partial class RegistroPage : ContentPage
    {
        private readonly BaseDatabase _database;

        public RegistroPage()
        {
            InitializeComponent();
            _database = new BaseDatabase();
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            if (!ValidarCampos()) return;

            try
            {
                var usuarioExistente = await _database.ExisteUsuarioAsync(UsernameEntry.Text);
                if (usuarioExistente)
                {
                    await DisplayAlert("Error", "El nombre de usuario ya existe", "OK");
                    return;
                }

                var emailExistente = await _database.ExisteEmailAsync(EmailEntry.Text);
                if (emailExistente)
                {
                    await DisplayAlert("Error", "El correo electrónico ya está registrado", "OK");
                    return;
                }

                var nuevoUsuario = new Usuario
                {
                    NombreUsuario = UsernameEntry.Text.Trim(),
                    Email = EmailEntry.Text.Trim(),
                    Password = PasswordEntry.Text // En producción: BCrypt.HashPassword(PasswordEntry.Text)
                };

                var resultado = await _database.GuardarUsuarioAsync(nuevoUsuario);

                if (resultado > 0)
                {
                    await DisplayAlert("Éxito", "Registro completado", "OK");
                    await Shell.Current.GoToAsync("//login");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al registrar: {ex.Message}", "OK");
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(UsernameEntry.Text))
            {
                DisplayAlert("Error", "Nombre de usuario requerido", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(EmailEntry.Text) || !EmailEntry.Text.Contains("@"))
            {
                DisplayAlert("Error", "Correo electrónico inválido", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(PasswordEntry.Text) || PasswordEntry.Text.Length < 6)
            {
                DisplayAlert("Error", "La contraseña debe tener al menos 6 caracteres", "OK");
                return false;
            }

            return true;
        }

        private async void OnLoginTapped(object sender, EventArgs e) =>
            await Shell.Current.GoToAsync("//login");

        private async void OnBackTapped(object sender, EventArgs e) =>
            await Shell.Current.GoToAsync("//login");
    }
}