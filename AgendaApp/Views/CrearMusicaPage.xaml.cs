using AgendaApp.Modelos;
using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Maui.Storage;

namespace AgendaApp.Views
{
    public partial class CrearMusicaPage : ContentPage
    {
        private string rutaArchivoSeleccionado;

        public CrearMusicaPage()
        {
            InitializeComponent();
        }

        private async void SeleccionarArchivoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var resultado = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Selecciona un archivo MP3",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.Android, new[] { "audio/mpeg" } },
                        { DevicePlatform.iOS, new[] { "public.mp3" } },
                        { DevicePlatform.MacCatalyst, new[] { "public.mp3" } },
                        { DevicePlatform.WinUI, new[] { ".mp3" } }
                    })
                });

                if (resultado != null)
                {
                    if (!resultado.FileName.EndsWith(".mp3", StringComparison.OrdinalIgnoreCase))
                    {
                        await DisplayAlert("Archivo inv�lido", "Por favor selecciona un archivo con extensi�n .mp3", "OK");
                        return;
                    }

                    rutaArchivoSeleccionado = resultado.FullPath;
                    archivoSeleccionadoLabel.Text = Path.GetFileName(rutaArchivoSeleccionado);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo seleccionar el archivo: {ex.Message}", "OK");
            }
        }

        private async void GuardarButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Validaci�n b�sica
                if (string.IsNullOrWhiteSpace(tituloEntry.Text) ||
                    string.IsNullOrWhiteSpace(artistaEntry.Text) ||
                    string.IsNullOrWhiteSpace(rutaArchivoSeleccionado))
                {
                    await DisplayAlert("Error", "Por favor completa todos los campos y selecciona un archivo", "OK");
                    return;
                }

                var musica = new Musica
                {
                    Titulo = tituloEntry.Text,
                    Artista = artistaEntry.Text,
                    Genero = generoEntry.Text,
                    RutaArchivo = rutaArchivoSeleccionado
                };

                int resultado = await App.Database.GuardarMusicaAsync(musica);

                if (resultado > 0)
                {
                    // Limpiar los campos
                    tituloEntry.Text = string.Empty;
                    artistaEntry.Text = string.Empty;
                    generoEntry.Text = string.Empty;
                    archivoSeleccionadoLabel.Text = "Archivo no seleccionado";
                    rutaArchivoSeleccionado = null;

                    // Mostrar confirmaci�n
                    await DisplayAlert("�xito", "M�sica guardada correctamente", "OK");

                    // Navegar a MainPage usando la ruta registrada
                    await Shell.Current.GoToAsync("//main");
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo guardar la m�sica", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurri� un error: {ex.Message}", "OK");
            }
        }
    }
}