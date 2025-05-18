using System;
using Microsoft.Maui.Controls;

namespace AgendaApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void IrListaContactos(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("contactos");
        }

        private async void IrCrearContacto(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("crear");
        }

        private async void IrConfiguracion(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("configuracion");
        }

        private async void Perfil_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("perfil");
        }

        private async void Inicio_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("main");
        }

        private async void Buscar_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("buscar");
        }

        private async void Biblioteca_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("biblioteca");
        }

        private async void Crear_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("crear");
        }
    }
}
