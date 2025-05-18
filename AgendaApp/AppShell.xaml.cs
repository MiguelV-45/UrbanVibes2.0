using AgendaApp.Views;
using AgendaApp.Admin;

namespace AgendaApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Rutas principales
            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("registro", typeof(RegistroPage));
            Routing.RegisterRoute("recuperar", typeof(RecuperarPage));
            Routing.RegisterRoute("main", typeof(MainPage));
            Routing.RegisterRoute("configuracion", typeof(ConfiguracionPage));

            // Rutas adicionales para navegación por código
            Routing.RegisterRoute("perfil", typeof(PerfilPage));
            Routing.RegisterRoute("editarperfil", typeof(Views.EditarPerfilPage));
            Routing.RegisterRoute("buscar", typeof(BuscarPage));
            Routing.RegisterRoute("biblioteca", typeof(BibliotecaPage));
            Routing.RegisterRoute("crearmusica", typeof(CrearMusicaPage));

            // Ruta Admin
            Routing.RegisterRoute("admin", typeof(Admin.AdminPage));
            Routing.RegisterRoute("usuarios", typeof(Admin.UsuariosPage));
            Routing.RegisterRoute("canciones", typeof(Admin.MusicaPage));
        }
    }
}
