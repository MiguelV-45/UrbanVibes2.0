using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui;

namespace AgendaApp;

[Activity(
    Label = "AgendaApp",
    Theme = "@style/MainTheme",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize
)]
public class MainActivity : MauiAppCompatActivity
{
}
