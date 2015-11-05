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
    /// Lógica de interacción para RegistroTerapeuta.xaml
    /// </summary>
    public partial class RegistroTerapeuta : Window
    {
        string nombreUsuario;
        public RegistroTerapeuta(string nombre)
        {
            nombreUsuario = nombre;
            InitializeComponent();
        }

        /// <summary>
        /// Comportamiento del boton cancelar. Cerrará la ventana.
        /// </summary>
        /// <param name="sender"></param> Boton cancelar.
        /// <param name="e"></param> Argumento del evento.
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

        /// <summary>
        ///  Comportamiento de registro del terapeuta en la base de datos.
        /// </summary>
        /// <param name="sender"></param> Boton de registro.
        /// <param name="e"></param> Argumento del evento.
        private void buttonRegistrar_Click(object sender, RoutedEventArgs e)
        {
            //Recogemos los datos.
            string nombreTerapeuta = textBoxNombre.Text;
            string apellidosTerapeuta = textBoxApellidos.Text;
            string nifTerapeuta = textBoxNIF.Text;
            string nacimientoTerapeuta = textBoxNacimiento.Text;

            if (Terapeuta.registrarTerapeuta(nombreTerapeuta, apellidosTerapeuta, nombreUsuario, nifTerapeuta, nacimientoTerapeuta) > 0)
            {
                MessageBox.Show("Terapeuta registrado con exito.");
                this.Close();
            }
            else
            {
                MessageBox.Show("El terapeuta ha completado mal el formulario.");
            }
        }
    }
}
