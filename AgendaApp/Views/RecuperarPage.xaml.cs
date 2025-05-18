namespace AgendaApp.Views;

public partial class RecuperarPage : ContentPage
{
    public RecuperarPage()
    {
        InitializeComponent();

        // Activar bot�n de retroceso si est� en NavigationPage
        Shell.SetBackButtonBehavior(this, new BackButtonBehavior
        {
            IsVisible = true
        });
    }

    private async void Guardar_Clicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text?.Trim();
        string newPassword = NewPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword))
        {
            await DisplayAlert("Error", "Por favor completa todos los campos.", "OK");
            return;
        }

        if (newPassword.Length < 6)
        {
            await DisplayAlert("Error", "La nueva contrase�a debe tener al menos 6 caracteres.", "OK");
            return;
        }

        await DisplayAlert("�xito", $"Tu contrase�a ha sido actualizada para {email}", "OK");
        await Navigation.PopAsync(); // Regresa a la p�gina anterior
    }

    private async void VolverLogin_Tapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//login");
    }
}