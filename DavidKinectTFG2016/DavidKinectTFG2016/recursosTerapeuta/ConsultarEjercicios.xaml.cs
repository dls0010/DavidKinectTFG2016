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

namespace DavidKinectTFG2016.recursosTerapeuta
{
    /// <summary>
    /// Lógica de interacción para ConsultarEjercicios.xaml
    /// </summary>
    public partial class ConsultarEjercicios : Window
    {
        SqlConnection conexion;
        public ConsultarEjercicios()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que carga la BD y rellena el combobox con todos los ejercicios.
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
            llenarComboBox();
        }

        /// <summary>
        /// Metodo adicional para llenar el combobox con los ejercicios.
        /// </summary>
        private void llenarComboBox()
        {
            try
            {
                string query = "Select * from ejercicios";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    string nombreEjercicio = dr.GetString(1);
                    comboBoxEjercicios.Items.Add(nombreEjercicio);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos de los ejercicios: " + ex.ToString());
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
        /// Metodo que rellena el textbox descripcion de la pagina al seleccionar un ejercicio en el combobox.
        /// </summary>
        /// <param name="sender"></param> ComboboxEjercicios.
        /// <param name="e"></param> Evento del comboBoxEjercicios
        private void comboBoxEjercicios_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                string query = "Select descripcion,imagenEjercicio from ejercicios where ejercicio = '" + comboBoxEjercicios.Text + "'";
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
    }
}
