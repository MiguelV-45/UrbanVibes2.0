using Microsoft.Maui.Storage;

namespace AgendaApp.Utils
{
    public static class ConfiguracionApp
    {
        public static void GuardarTema(bool esOscuro)
        {
            Preferences.Set("temaOscuro", esOscuro);
        }

        public static bool ObtenerTema()
        {
            return Preferences.Get("temaOscuro", false);
        }

        public static void AplicarTemaGuardado()
        {
            bool esOscuro = ObtenerTema();
            App.Current.UserAppTheme = esOscuro ? AppTheme.Dark : AppTheme.Light;
        }
    }
}
