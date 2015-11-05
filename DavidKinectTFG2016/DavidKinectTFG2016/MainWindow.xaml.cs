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

namespace DavidKinectTFG2016
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
            KinectTFGBDEntities modeloEntidades = new KinectTFGBDEntities();

            //Verificamos si el usuario existe en la BD:
            Usuarios usuariosBD = modeloEntidades.Usuarios.SingleOrDefault( us => us.usuario.Equals(usuarioIntroducido));

            if (usuariosBD != null) {
                //verificamos la contraseña: 
                if(usuariosBD.contraseña.Equals(contraseñaIntroducida))
                {
                    //Abrimos la nueva ventana:
                    InicioSesion inicioSesion = new InicioSesion();
                    inicioSesion.Show();
                    this.Close();
                }
                else
                {
                    //Las contraseñas no coinciden
                    textBoxContraseña.Background = Brushes.Red; //Cambiamos el color del TextBox.
                    MessageBox.Show("Contraseña incorrecta");
                    textBoxContraseña.Clear();
                }
            }
            //Usuario no existe
            else
            {
                textBoxUsuario.Background = Brushes.Red;
                MessageBox.Show("Usuario no es correcto o no existe");
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
    }
}
