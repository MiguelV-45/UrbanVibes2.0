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
                        await DisplayAlert("Archivo inválido", "Por favor selecciona un archivo con extensión .mp3", "OK");
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
            if (string.IsNullOrEmpty(rutaArchivoSeleccionado))
            {
                await DisplayAlert("Error", "Debes seleccionar un archivo MP3 primero.", "OK");
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
                await DisplayAlert("Éxito", "Música guardada correctamente", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se pudo guardar la música", "OK");
            }
        }
    }
}