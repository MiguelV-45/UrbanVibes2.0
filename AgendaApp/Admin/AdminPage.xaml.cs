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
            bool confirmacion = await DisplayAlert("Cerrar Sesi�n", "�Est�s seguro de que deseas cerrar sesi�n?", "S�", "No");
            if (confirmacion)
            {
                // Regresa a la p�gina de inicio de sesi�n (LoginPage)
                await Shell.Current.GoToAsync("login"); // Aseg�rate de que la ruta "//login" est� definida
            }
        }
    }
}