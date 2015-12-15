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
using System.Windows.Shapes;
using DavidKinectTFG2016.clases;

namespace DavidKinectTFG2016.registrosVarios
{
    /// <summary>
    /// Lógica de interacción para RegistroAdministrador.xaml
    /// </summary>
    public partial class RegistroAdministrador : Window
    {
        string nombreUsuario;
        public RegistroAdministrador(string nombre)
        {
            nombreUsuario = nombre;
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que cierra la ventana.
        /// </summary>
        /// <param name="sender"></param> Boton Cancelar.
        /// <param name="e"></param> Evento del boton.
        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Usuario.BorrarUsuario(nombreUsuario) > 0)
            {
                MessageBox.Show("Has cancelado el proceso de registro.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al cancelar el proceso de registro.");
            }
            this.Close();
        }

        /// <summary>
        /// Inserccion de la fecha seleccionada por el usuario en un texbox.
        /// </summary>
        /// <param name="sender"></param> DateTimePicker
        /// <param name="e"></param> Argumento del evento.
        private void buttonNacimiento_Click(object sender, RoutedEventArgs e)
        {
            textBoxNacimiento.Text = dateCalendario.SelectedDate.Value.ToString("yyyy/MM/dd");
        }

        /// <summary>
        ///  Comportamiento de registro del terapeuta en la base de datos.
        /// </summary>
        /// <param name="sender"></param> Boton de registro.
        /// <param name="e"></param> Argumento del evento.
        private void buttonRegistrar_Click(object sender, RoutedEventArgs e)
        {
            //Recogemos los datos.
            string nombreAdministrador = textBoxNombre.Text;
            string apellidosAdministrador = textBoxApellidos.Text;
            string nifAdministrador = textBoxNIF.Text;
            string nacimientoAdministrador = textBoxNacimiento.Text;

            if (Administrador.registrarAdministrador(nombreAdministrador, apellidosAdministrador, nombreUsuario, nifAdministrador, nacimientoAdministrador) > 0)
            {
                MessageBox.Show("Administrador registrado con exito.");
                this.Close();
            }
            else
            {
                MessageBox.Show("El administrador ha completado mal el formulario.");
            }
        }
    }
}
