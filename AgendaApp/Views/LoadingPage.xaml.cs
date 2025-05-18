using AgendaApp.Datos;
using System.Diagnostics;

namespace AgendaApp.Views
{
    public partial class LoadingPage : ContentPage
    {
        private readonly BaseDatabase _database;

        public LoadingPage()
        {
            InitializeComponent();
            _database = new BaseDatabase();
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            try
            {
                var isAuth = await IsAuthenticatedAsync();

                await Task.Delay(1000); // Breve espera para la animaci�n

                await Shell.Current.GoToAsync(isAuth ? "///main" : "//login");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error de autenticaci�n: {ex.Message}");
                await Shell.Current.GoToAsync("//login");
            }

            base.OnNavigatedTo(args);
        }

        private async Task<bool> IsAuthenticatedAsync()
        {
            // Verificar si hay un token de autenticaci�n v�lido
            var usuarioId = await SecureStorage.GetAsync("UsuarioId");
            if (string.IsNullOrEmpty(usuarioId)) return false;

            // Verificar si el usuario existe en la base de datos
            if (!int.TryParse(usuarioId, out var id)) return false;

            var usuario = await _database.ObtenerUsuarioPorIdAsync(id);
            return usuario != null;
        }
    }
}