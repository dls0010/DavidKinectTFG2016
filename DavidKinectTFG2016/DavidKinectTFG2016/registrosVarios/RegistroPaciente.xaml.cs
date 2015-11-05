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
    /// Lógica de interacción para RegistroPaciente.xaml
    /// </summary>
    public partial class RegistroPaciente : Window
    {
        string nombreUsuario;
        public RegistroPaciente(string nombre)
        {
            nombreUsuario = nombre;
            InitializeComponent();
        }

        /// <summary>
        ///  Comportamiento de registro del paciente en la base de datos.
        /// </summary>
        /// <param name="sender"></param> Boton de registro.
        /// <param name="e"></param> Argumento del evento.
        private void buttonRegistrar_Click(object sender, RoutedEventArgs e)
        {
            //Recogemos los datos.
            string nombrePaciente = textBoxNombre.Text;
            string apellidosPaciente = textBoxApellidos.Text;
            string nifPaciente = textBoxNIF.Text;
            string telefonoPaciente = textBoxTelefono.Text;
            string nacimientoPaciente = textBoxNacimiento.Text;
            string estadoPaciente = textBoxEstado.Text;

            if (Paciente.RegistrarPaciente(nombrePaciente, apellidosPaciente, nombreUsuario, nifPaciente, telefonoPaciente, nacimientoPaciente, estadoPaciente) > 0)
            {
                MessageBox.Show("Paciente registrado con exito.");
                this.Close();
            }
            else
            {
                MessageBox.Show("El paciente ha completado mal el formulario.");
            }
        }

        /// <summary>
        /// Metodo que cierra la ventana.
        /// </summary>
        /// <param name="sender"></param> Boton Cancelar.
        /// <param name="e"></param> Evento del boton.
        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
