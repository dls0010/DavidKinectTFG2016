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
using DavidKinectTFG2016.recursosPaciente;
using System.IO;
using MySql.Data.MySqlClient;

namespace DavidKinectTFG2016.iniciosSesionVarios
{
    /// <summary>
    /// Lógica de interacción para InicioSesionPaciente.xaml
    /// </summary>
    public partial class InicioSesionPaciente : Window
    {
        string nombreUsuario;
        MySqlConnection conexion;
        public InicioSesionPaciente(string nombre)
        {
            nombreUsuario = nombre;
            InitializeComponent();
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
        /// Metodo que carga la foto del paciente en el momento de cargar la ventana.
        /// </summary>
        /// <param name="sender"></param> Cargar ventana.
        /// <param name="e"></param> Evento de carga.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "Select * from pacientes where usuario = '" + nombreUsuario + "'";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                MySqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    byte[] imagen = (byte[])(dr["imagenPaciente"]);
                    if (imagen == null)
                        imagenFoto.Source = new BitmapImage(new Uri("/images/usuario.jpg"));
                    else
                    {
                        MemoryStream mstream = new MemoryStream(imagen);
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = mstream;
                        image.EndInit();
                        imagenFoto.Source = image;
                    }
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los datos del paciente: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que nos lleva a la pantalla de comienzo de ejercicio seleccionado.
        /// </summary>
        /// <param name="sender"></param> Boton Ejercicio.
        /// <param name="e"></param> Evento del boton.
        private void buttonEjercicio_Click(object sender, RoutedEventArgs e)
        {
            int repeticiones = Convert.ToInt32(textBoxRepeticiones.Text);
            if (comboBoxEjercicios.Text == "Ejercicio1")
            {
                Ejercicio1 ejercicio1 = new Ejercicio1(nombreUsuario, repeticiones);
                ejercicio1.Show();
            }
            if (comboBoxEjercicios.Text == "Ejercicio2")
            {
                Ejercicio2 ejercicio2 = new Ejercicio2(nombreUsuario, repeticiones);
                ejercicio2.Show();
            }
            if (comboBoxEjercicios.Text == "Ejercicio3")
            {
                Ejercicio3 ejercicio3 = new Ejercicio3(nombreUsuario, repeticiones);
                ejercicio3.Show();
            }
            if (comboBoxEjercicios.Text == "Ejercicio4")
            {
                Ejercicio4 ejercicio4 = new Ejercicio4(nombreUsuario, repeticiones);
                ejercicio4.Show();
            }
            if (comboBoxEjercicios.Text == "Ejercicio5")
            {
                Ejercicio5 ejercicio5 = new Ejercicio5(nombreUsuario, repeticiones);
                ejercicio5.Show();
            }
            if (comboBoxEjercicios.Text == "Ejercicio6")
            {
                Ejercicio6 ejercicio6 = new Ejercicio6(nombreUsuario, repeticiones);
                ejercicio6.Show();
            }
        }

        /// <summary>
        /// Metodo adicional para llenar el combobox con los ejercicios.
        /// </summary>
        private void llenarComboBox()
        {
            try
            {
                string query = "Select * from ejercicios";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                MySqlDataReader dr = comando.ExecuteReader();
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
        /// Metodo que rellena el textbox descripcion de la pagina al seleccionar un ejercicio en el combobox.
        /// </summary>
        /// <param name="sender"></param> ComboboxEjercicios.
        /// <param name="e"></param> Evento del comboBoxEjercicios
        private void comboBoxEjercicios_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                string query = "Select descripcion, imagenEjercicio from ejercicios where ejercicio = '" + comboBoxEjercicios.Text + "'";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                MySqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    string descripcion = dr.GetString(0);
                    textBoxDescripcion.Text = descripcion;

                    if (dr["imagenEjercicio"] != DBNull.Value)
                    {
                        byte[] imagen = (byte[])(dr["imagenEjercicio"]);

                        MemoryStream mstream = new MemoryStream(imagen);
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = mstream;
                        image.EndInit();
                        imagenEjercicio.Source = image;
                    }
                    else
                    {
                        imagenEjercicio.Source = null;
                    }
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener la descripcion de los ejercicios: " + ex.ToString());
            }

        }

        /// <summary>
        /// Metodo que nos lleva a la pantalla de consulta del historial del paciente.
        /// </summary>
        /// <param name="sender"></param> Boton consultar.
        /// <param name="e"></param> Evento del boton.
        private void buttonConsultar_Click(object sender, RoutedEventArgs e)
        {
            ConsultaHistorial consultar = new ConsultaHistorial(nombreUsuario);
            consultar.Show();
        }

        /// <summary>
        /// Metodo que cierra la ventana y la conexion con la base de datos.
        /// </summary>
        /// <param name="sender"></param> Boton cancelar.
        /// <param name="e"></param> Eventos del boton.
        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar la conexion con la base de datos: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que cierra la conexion con la BD al cerrar la ventana.
        /// </summary>
        /// <param name="sender"></param> Cerrar ventana.
        /// <param name="e"></param> Evento de cerrar.
        private void Window_Closed(object sender, EventArgs e)
        {
            conexion.Close();
        }

        /// <summary>
        /// Abre ventana para consultar los entrenamientos realizados por el paciente
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConsultarEntrenamientos_Click(object sender, RoutedEventArgs e)
        {
            ConsultarEntrenamientos consultar = new ConsultarEntrenamientos(nombreUsuario);
            consultar.Show();
        }
    }
}
