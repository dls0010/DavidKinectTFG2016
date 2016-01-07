using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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

namespace DavidKinectTFG2016.recursosPaciente
{
    /// <summary>
    /// Lógica de interacción para PrevisualizarEjercicio.xaml
    /// </summary>
    public partial class PrevisualizarEjercicio : Window
    {
        SqlConnection conexion;
        string nombreEjercicio;
        int repeticiones;
        public PrevisualizarEjercicio(string idEjercicio, int repeticiones)
        {
            this.nombreEjercicio = idEjercicio;
            this.repeticiones = repeticiones;
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que carga la BD y obtiene los datos del ejercicio que corresponda mostrar.
        /// </summary>
        /// <param name="sender"></param> Cargar ventana.
        /// <param name="e"></param> Evento de carga.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBlockSiguienteEjercicio.Text = "NUEVO EJERCICIO \n LAS REPETICIONES SON: " + repeticiones;
            try
            {
                conexion = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la Base de datos: " + ex.ToString());
            }
            try
            {
                string query = "Select descripcion,imagenEjercicio from ejercicios where ejercicio = '" + nombreEjercicio + "'";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    string descripcion = dr.GetString(0);
                    textBoxDescripcion.Text = descripcion;

                    byte[] imagen = (byte[])(dr["imagenEjercicio"]);

                    MemoryStream mstream = new MemoryStream(imagen);
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = mstream;
                    image.EndInit();
                    imagenEjercicio.Source = image;
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener la descripcion de los ejercicios: " + ex.ToString());
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
        /// Metodo que cierra la ventana, cierra BD y da paso al ejercicio para que comience.
        /// </summary>
        /// <param name="sender"></param> Boton Ejercicio.
        /// <param name="e"></param> Evento del boton.
        private void buttonEjercicio_Click(object sender, RoutedEventArgs e)
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
    }
}
