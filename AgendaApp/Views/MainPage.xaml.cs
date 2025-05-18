
using AgendaApp.Modelos;
using AgendaApp.Datos;
using AgendaApp.Utils;

namespace AgendaApp.Views;

public partial class MainPage : ContentPage
{
    private BaseDatabase db = new BaseDatabase();

    public MainPage()
    {
        InitializeComponent();
        //this.db = db;

        titleLabel.Text = Traductor.Get("TituloAgenda");
    }
     
    private async void OnConfiguracionClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ConfiguracionPage());
    }
    private async void CerrarSesion_Clicked(object sender, EventArgs e)
    {
        try
        {
            Preferences.Remove("UsuarioActual");
            SecureStorage.Remove("UsuarioId");
            await Shell.Current.GoToAsync("//login");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo cerrar sesión: {ex.Message}", "OK");
        }
    }
}