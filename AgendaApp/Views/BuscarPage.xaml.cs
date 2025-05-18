using AgendaApp.Modelos;
using Microsoft.Maui.Controls;
using Plugin.Maui.Audio;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;

namespace AgendaApp.Views
{
    public partial class BuscarPage : ContentPage, INotifyPropertyChanged
    {
        public ObservableCollection<MusicaGrupo> MusicaGrupos { get; set; }
        private IAudioPlayer _audioPlayer;
        private readonly IAudioManager _audioManager = AudioManager.Current;
        private System.Timers.Timer _timer;
        private Musica _musicaActual;

        private bool _isPlaying;
        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    OnPropertyChanged(nameof(IsPlaying));
                    OnPropertyChanged(nameof(PlayPauseIcon));
                }
            }
        }

        private string _cancionTitulo;
        public string CancionTitulo
        {
            get => _cancionTitulo;
            set
            {
                if (_cancionTitulo != value)
                {
                    _cancionTitulo = value;
                    OnPropertyChanged(nameof(CancionTitulo));
                }
            }
        }

        private string _cancionArtista;
        public string CancionArtista
        {
            get => _cancionArtista;
            set
            {
                if (_cancionArtista != value)
                {
                    _cancionArtista = value;
                    OnPropertyChanged(nameof(CancionArtista));
                }
            }
        }

        private string _tiempoReproduccion;
        public string TiempoReproduccion
        {
            get => _tiempoReproduccion;
            set
            {
                if (_tiempoReproduccion != value)
                {
                    _tiempoReproduccion = value;
                    OnPropertyChanged(nameof(TiempoReproduccion));
                }
            }
        }

        private double _progressPercentage;
        public double ProgressPercentage
        {
            get => _progressPercentage;
            set
            {
                if (_progressPercentage != value)
                {
                    _progressPercentage = value;
                    OnPropertyChanged(nameof(ProgressPercentage));
                }
            }
        }

        public string PlayPauseIcon => IsPlaying ? "??" : "??";

        public BuscarPage()
        {
            InitializeComponent();
            MusicaGrupos = new ObservableCollection<MusicaGrupo>();
            BindingContext = this;

            _timer = new System.Timers.Timer(500);
            _timer.Elapsed += Timer_Elapsed;

            IsPlaying = false;
            CancionTitulo = "";
            CancionArtista = "";
            TiempoReproduccion = "00:00 / 00:00";
            ProgressPercentage = 0;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var listaMusica = await App.Database.ObtenerMusicasAsync();

            var grupos = listaMusica
                .GroupBy(m => m.Genero)
                .Select(g => new MusicaGrupo(g.Key, new ObservableCollection<Musica>(g.ToList())))
                .ToList();

            MusicaGrupos.Clear();
            foreach (var grupo in grupos)
            {
                MusicaGrupos.Add(grupo);
            }
        }

        private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var seleccion = e.CurrentSelection.FirstOrDefault() as Musica;
            if (seleccion == null)
                return;

            if (_audioPlayer != null && _audioPlayer.IsPlaying)
            {
                _audioPlayer.Stop();
            }

            try
            {
                if (!System.IO.File.Exists(seleccion.RutaArchivo))
                {
                    await DisplayAlert("Error", "Archivo no encontrado.", "OK");
                    return;
                }

                var stream = System.IO.File.OpenRead(seleccion.RutaArchivo);
                _audioPlayer = _audioManager.CreatePlayer(stream);
                _audioPlayer.Play();

                _musicaActual = seleccion;
                IsPlaying = true;

                CancionTitulo = $"Canción: {_musicaActual.Titulo}";
                CancionArtista = $"Artista: {_musicaActual.Artista}";
                TiempoReproduccion = "00:00 / " + TimeSpan.FromSeconds(_audioPlayer.Duration).ToString(@"mm\:ss");

                _timer.Start();

                ((CollectionView)sender).SelectedItem = null;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo reproducir el archivo: {ex.Message}", "OK");
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_audioPlayer == null || !_audioPlayer.IsPlaying)
                return;

            Device.BeginInvokeOnMainThread(() =>
            {
                var tiempoActual = _audioPlayer.CurrentPosition;
                var duracion = _audioPlayer.Duration;

                TiempoReproduccion = $"{TimeSpan.FromSeconds(tiempoActual):mm\\:ss} / {TimeSpan.FromSeconds(duracion):mm\\:ss}";

                if (duracion > 0)
                {
                    ProgressPercentage = (tiempoActual / duracion) * 100;
                }
                else
                {
                    ProgressPercentage = 0;
                }
            });
        }

        private void PlayPauseButton_Clicked(object sender, EventArgs e)
        {
            if (_audioPlayer == null) return;

            if (IsPlaying)
            {
                _audioPlayer.Pause();
                IsPlaying = false;
                _timer.Stop();
            }
            else
            {
                _audioPlayer.Play();
                IsPlaying = true;
                _timer.Start();
            }
        }

        private void StopButton_Clicked(object sender, EventArgs e)
        {
            if (_audioPlayer != null)
            {
                _audioPlayer.Stop();
                IsPlaying = false;
                _timer.Stop();

                TiempoReproduccion = "00:00 / 00:00";
                CancionTitulo = "";
                CancionArtista = "";
                ProgressPercentage = 0;
            }
        }
    }
}