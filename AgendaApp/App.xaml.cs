using AgendaApp.Utils;
using AgendaApp.Datos;  // <-- Importa el namespace donde está tu base de datos

namespace AgendaApp
{
    public partial class App : Application
    {
        // Propiedad estática para acceder a la base de datos
        public static BaseDatabase Database { get; private set; }

        public App()
        {
            InitializeComponent();

            // Inicializamos la base de datos una vez aquí
            Database = new BaseDatabase();

            ConfiguracionApp.AplicarTemaGuardado();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}