using Microsoft.Maui.Controls;

namespace AgendaApp.Admin
{
    public partial class AdminPage : ContentPage
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private async void BtnVerUsuarios_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("usuarios");  // ? SIN //
        }

        private async void BtnVerCanciones_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("canciones");  // ? SIN //
        }

        private async void BtnCerrarSesion_Clicked(object sender, EventArgs e)
        {
            bool confirmacion = await DisplayAlert("Cerrar Sesión", "¿Estás seguro de que deseas cerrar sesión?", "Sí", "No");
            if (confirmacion)
            {
                // Regresa a la página de inicio de sesión (LoginPage)
                await Shell.Current.GoToAsync("login"); // Asegúrate de que la ruta "//login" esté definida
            }
        }
    }
}