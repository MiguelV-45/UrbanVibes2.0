using AgendaApp.Datos;
using AgendaApp.Modelos;

namespace AgendaApp.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly BaseDatabase _database;

        public LoginPage()
        {
            InitializeComponent();
            _database = new BaseDatabase();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.Quit();
            return true;
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                return;
            }

            // Si es admin con contraseña fija
            if (Username.Text == "admin" && Password.Text == "123456")
            {
                Username.Text = string.Empty;
                Password.Text = string.Empty;
                await Shell.Current.GoToAsync("//admin");
                return;
            }

            // Obtener el usuario de la base de datos
            var usuario = await _database.ObtenerUsuarioAsync(Username.Text);

            if (usuario != null && usuario.Password == Password.Text)
            {
                Preferences.Set("UsuarioActual", usuario.NombreUsuario);
                await SecureStorage.SetAsync("UsuarioId", usuario.Id.ToString());

                // Limpiar campos de login
                Username.Text = string.Empty;
                Password.Text = string.Empty;

                await Shell.Current.GoToAsync("///main");
            }
            else
            {
                await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
            }
        }

        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(Username.Text))
            {
                DisplayAlert("Error", "Usuario requerido", "OK");
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password.Text))
            {
                DisplayAlert("Error", "Contraseña requerida", "OK");
                return false;
            }

            return true;
        }

        private async void TapGestureRecognizerPwd_Tapped(object sender, TappedEventArgs e) =>
            await Shell.Current.GoToAsync("//recuperar");

        private async void TapGestureRecognizerReg_Tapped(object sender, TappedEventArgs e) =>
            await Shell.Current.GoToAsync("//registro");
    }
}