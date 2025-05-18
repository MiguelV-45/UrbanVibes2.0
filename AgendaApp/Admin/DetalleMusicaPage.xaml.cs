using AgendaApp.Datos;
using AgendaApp.Modelos;
using System.Diagnostics;

namespace AgendaApp.Admin
{
    public partial class DetalleMusicaPage : ContentPage
    {
        private readonly BaseDatabase _database;
        private readonly Musica _musicaOriginal;
        private readonly Action _actualizarListaCallback;

        public DetalleMusicaPage(Musica musica, Action actualizarListaCallback = null)
        {
            InitializeComponent();
            _database = new BaseDatabase();
            _musicaOriginal = musica;
            _actualizarListaCallback = actualizarListaCallback;
            BindingContext = _musicaOriginal;
        }

        private async void OnEliminarClicked(object sender, EventArgs e)
        {
            try
            {
                bool confirmado = await DisplayAlert(
                    "Confirmar Eliminación",
                    $"¿Estás seguro de eliminar '{_musicaOriginal.Titulo}'?",
                    "Sí, eliminar",
                    "Cancelar");

                if (!confirmado) return;

                // Verificación adicional
                if (_musicaOriginal == null || _musicaOriginal.Id <= 0)
                {
                    await DisplayAlert("Error", "Datos de la canción inválidos", "OK");
                    return;
                }

                Debug.WriteLine($"Intentando eliminar canción ID: {_musicaOriginal.Id}");

                var resultado = await _database.EliminarMusicaAsync(_musicaOriginal.Id);

                if (resultado == 1) // 1 fila afectada = éxito
                {
                    _actualizarListaCallback?.Invoke();
                    await DisplayAlert("Éxito", "Canción eliminada correctamente", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error",
                        resultado == 0 ? "No se encontró la canción" : "Error desconocido",
                        "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error crítico completo: {ex}");
                await DisplayAlert("Error Crítico",
                    $"Ocurrió un error inesperado: {ex.Message}\n\nDetalles técnicos:\n{ex.GetType().Name}",
                    "OK");
            }
        }
        private async void OnGuardarCambiosClicked(object sender, EventArgs e)
        {
            try
            {
                // Actualizar los valores del objeto
                _musicaOriginal.Titulo = TituloEntry.Text;
                _musicaOriginal.Artista = ArtistaEntry.Text;

                // Guardar en la base de datos
                var resultado = await _database.GuardarMusicaAsync(_musicaOriginal);

                if (resultado > 0)
                {
                    await DisplayAlert("Éxito", "Cambios guardados correctamente", "OK");
                    _actualizarListaCallback?.Invoke();
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "No se pudieron guardar los cambios", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al guardar: {ex}");
                await DisplayAlert("Error", $"Error al guardar: {ex.Message}", "OK");
            }
        }
    }
}