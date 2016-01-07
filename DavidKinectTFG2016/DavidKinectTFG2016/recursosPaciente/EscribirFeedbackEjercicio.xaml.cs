using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace DavidKinectTFG2016.recursosPaciente
{
    /// <summary>
    /// Lógica de interacción para EscribirFeedbackEjercicio.xaml
    /// </summary>
    public partial class EscribirFeedbackEjercicio : Window
    {
        SqlConnection conexion;
        string nombreUsuarioPaciente;
        private string ejercicio;
        private int repeticiones;
        private string duracion;
        private string feedback;

        /// <summary>
        /// Clase que recibe tanto el nombre de usuario, el ejercicio, el numero de repeticiones y el tiempo del ejercicio.
        /// Consulta y obtiene la valoración por parte del paciente acerca del ejercicio realizado.
        /// </summary>
        /// <param name="nombreUsuario"></param> nombre de usuario del paciente.
        /// <param name="ejercicio"></param> nombre del ejercicio realizado.
        /// <param name="repeticiones"></param> repeticiones realizadas en el ejercicio.
        /// <param name="tiempo"></param> tiempo que ha llevado la realización del ejercicio.
        public EscribirFeedbackEjercicio(string nombreUsuario, string ejercicio, int repeticiones, TimeSpan tiempo)
        {
            nombreUsuarioPaciente = nombreUsuario;
            this.ejercicio = ejercicio;
            this.repeticiones = repeticiones;
            this.duracion = tiempo.ToString(@"dd\.hh\:mm\:ss");
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que carga la BD.
        /// </summary>
        /// <param name="sender"></param> Cargar ventana.
        /// <param name="e"></param> Evento de carga.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la Base de datos: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que cierra la ventana, al pulsar el boton de cancelar, cierra la conexion con la BD.
        /// </summary>
        /// <param name="sender"></param> Boton Cancelar.
        /// <param name="e"></param> Evento del boton.
        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar la conexion con la BD: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que cierra la conexion con la base de datos al cerrar la ventana.
        /// </summary>
        /// <param name="sender"></param> Boton cerrar.
        /// <param name="e"></param>Evento de cerrar.
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar la conexion con la BD: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que escribe el feedback del paciente a la BD tabla entrenamientos.
        /// </summary>
        /// <param name="sender"></param> Boton mandar.
        /// <param name="e"></param> Evento del botón.
        private void buttonMandar_Click(object sender, RoutedEventArgs e)
        {
            feedback = textBoxFeedback.Text;
            if (Ejercicio.registrarEjercicio(nombreUsuarioPaciente,ejercicio,repeticiones,duracion,feedback) > 0)
            {
               MessageBox.Show("Feedback enviado correctamente.");
               this.Close();
            }
            else
            {
                MessageBox.Show("Error al mandar el feedback del ejercicio.");
            }
        }
    }
}
