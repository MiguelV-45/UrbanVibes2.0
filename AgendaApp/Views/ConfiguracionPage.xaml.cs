using AgendaApp.Utils;
using AgendaApp.Datos; // Asegúrate de tener este using para acceder a UsuarioDatabase

namespace AgendaApp.Views
{
    public partial class ConfiguracionPage : ContentPage
    {
        private readonly BaseDatabase _database;

        public ConfiguracionPage()
        {
            InitializeComponent();
            _database = new BaseDatabase(); // Inicializa la base de datos
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("¿Seguro que quieres cerrar sesión?", "Se perderán tus datos de sesión.", "Sí", "No"))
            {
                try
                {
                    // Obtener el ID del usuario actual
                    var usuarioId = await SecureStorage.GetAsync("UsuarioId");

                    if (!string.IsNullOrEmpty(usuarioId) && int.TryParse(usuarioId, out int id))
                    {
                        // Eliminar el usuario de la base de datos
                        await _database.EliminarUsuarioAsync(id);
                    }

                    // Limpiar preferencias y almacenamiento seguro
                    Preferences.Remove("UsuarioActual");
                    SecureStorage.RemoveAll();

                    // Redirigir al login
                    await Shell.Current.GoToAsync("///login");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo cerrar sesión: {ex.Message}", "OK");
                }
            }
        }

        private void OnCambiarTemaClicked(object sender, EventArgs e)
        {
            bool temaOscuro = App.Current.UserAppTheme == AppTheme.Dark;
            bool nuevoTemaOscuro = !temaOscuro;

            ConfiguracionApp.GuardarTema(nuevoTemaOscuro);
            App.Current.UserAppTheme = nuevoTemaOscuro ? AppTheme.Dark : AppTheme.Light;
        }
    }
}