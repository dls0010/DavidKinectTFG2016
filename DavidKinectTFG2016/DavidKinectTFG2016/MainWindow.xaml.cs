using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DavidKinectTFG2016.iniciosSesionVarios;
using DavidKinectTFG2016.clases;
using System.Threading;
using System.Globalization;
using System.Diagnostics;

namespace DavidKinectTFG2016
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string idioma = "español";
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que verifica si los valores de inicio sesion son correctos.
        /// </summary>
        /// <param name="sender"></param> Boton Iniciar.
        /// <param name="e"></param> Eventos del boton.
        private void iniciar_click_buttonIniciar(object sender, RoutedEventArgs e)
        {
            //Obtenemos los datos introducidos:
            string usuarioIntroducido = textBoxUsuario.Text;
            string contraseñaIntroducida = textBoxContraseña.Password;

            //Utilizamos el modelo de identidades
            KinectTFGBDEntities1 modeloEntidades = new KinectTFGBDEntities1();

            if (Usuario.Autentificar(usuarioIntroducido, contraseñaIntroducida) > 0)
            {
                MessageBox.Show("Usuario correcto");
                string tipoUsuario = Usuario.obtenerTipo(textBoxUsuario.Text);
                if (tipoUsuario == "Paciente")
                {
                    InicioSesionPaciente inicioPaciente = new InicioSesionPaciente(usuarioIntroducido);
                    inicioPaciente.Show();
                }
                else if (tipoUsuario == "Terapeuta")
                {
                    InicioSesionTerapeuta inicioTerapeuta = new InicioSesionTerapeuta(usuarioIntroducido);
                    inicioTerapeuta.Show();
                }
                else if (tipoUsuario == "Administrador")
                {
                    InicioSesionAdministrador inicioAdministrador = new InicioSesionAdministrador(usuarioIntroducido);
                    inicioAdministrador.Show();
                }

            }
            else
            {
                textBoxUsuario.Background = Brushes.Red;
                textBoxContraseña.Background = Brushes.Red;
                MessageBox.Show("Usuario no es correcto o no existe. Vuelve a intentarlo.");
                textBoxUsuario.Clear();
                textBoxContraseña.Clear();
            }
        }

        /// <summary>
        /// Metodo que permite abrir la ventana de registro.
        /// </summary>
        /// <param name="sender"></param> Boton registrar.
        /// <param name="e"></param> Evento del boton.
        private void registrar_click_buttonRegistrar(object sender, RoutedEventArgs e)
        {
            Registro registro = new Registro();
            registro.Show();
        }

        /// <summary>
        /// Metodo que permite cambiar el idioma de la aplicacion a Español.
        /// </summary>
        /// <param name="sender"></param> Boton Español.
        /// <param name="e"></param> Evento del boton.
        private void buttonSpanish_Click(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-ES");
            idiomas.WrapperIdiomas.ChangeCulture(Thread.CurrentThread.CurrentUICulture);
            idioma = "español";
        }

        /// <summary>
        /// Metodo que permite cambiar el idioma de la aplicacion a Ingles.
        /// </summary>
        /// <param name="sender"></param> Boton Ingles.
        /// <param name="e"></param> Evento del boton.
        private void buttonEnglish_Click(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            idiomas.WrapperIdiomas.ChangeCulture(Thread.CurrentThread.CurrentUICulture);
            idioma = "ingles";
        }

        /// <summary>
        /// Metodo que permite consultar el Acerca De, de la aplicación.
        /// </summary>
        /// <param name="sender"></param> Boton Acerca de.
        /// <param name="e"></param> Evento del boton.
        private void buttonAcerca_Click(object sender, RoutedEventArgs e)
        {
            AcercaDe acercaDe = new AcercaDe();
            acercaDe.Show();
        }

        /// <summary>
        /// Metodo que permite cerrar la aplicacion.
        /// </summary>
        /// <param name="sender"></param> Boton Salir.
        /// <param name="e"></param> Evento del boton.
        private void buttonSalir_Click(object sender, RoutedEventArgs e)
        {
           App.Current.Shutdown();
        }

        /// <summary>
        /// Metodo que abre el documento HTML de ayuda en el navegador.
        /// </summary>
        /// <param name="sender"></param> Boton de ayuda...
        /// <param name="e"></param> Evento del boton.
        private void buttonAyuda_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (idioma == "ingles")
                    System.Diagnostics.Process.Start("ayudaEN.html");
                else
                    System.Diagnostics.Process.Start("ayuda.html");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
