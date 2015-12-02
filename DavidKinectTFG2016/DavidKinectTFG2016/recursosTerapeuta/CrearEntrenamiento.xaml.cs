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
using DavidKinectTFG2016.clases;

namespace DavidKinectTFG2016.recursosTerapeuta
{
    /// <summary>
    /// Lógica de interacción para CrearEntrenamiento.xaml
    /// </summary>
    public partial class CrearEntrenamiento : Window
    {
        SqlConnection conexion;
        string nombreUsuarioTerapeuta;
        string descripcion;
        string nombrePaciente;
        string apellidosPaciente;
        string nombreUsuarioPaciente;
        int repeticiones1 = 0;
        int repeticiones2 = 0;
        int repeticiones3 = 0;
        int repeticiones4 = 0;
        int repeticiones5 = 0;
        public CrearEntrenamiento(string usuario)
        {
            nombreUsuarioTerapeuta = usuario;
            InitializeComponent();
        }

        /// <summary>
        /// Metodo adicional para llenar el combobox con los nombres de los pacientes del terapeuta.
        /// </summary>
        private void llenarComboBox()
        {
            SqlCommand comando;
            SqlDataReader dr;
            string nombreTerapeuta="";
            try
            {
                string queryTerapeuta = "select nombreTerapeuta from Terapeutas where usuario='" + nombreUsuarioTerapeuta + "'";
                comando = new SqlCommand(queryTerapeuta, conexion);
                dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    nombreTerapeuta = dr.GetString(0);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los datos del terapeuta");
            }

            try { 
            string query = "Select apellidosPaciente from relaciones where nombreTerapeuta ='"+nombreTerapeuta+"'";
                comando = new SqlCommand(query, conexion);
                dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    string apellidos = dr.GetString(0);
                    comboBoxPaciente.Items.Add(apellidos);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los pacientes del terapeuta");
            }

            try
            {
                string query = "Select ejercicio from ejercicios";
                comando = new SqlCommand(query, conexion);
                dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    string ejercicio = dr.GetString(0);
                    comboBoxEjercicio1.Items.Add(ejercicio);
                    comboBoxEjercicio2.Items.Add(ejercicio);
                    comboBoxEjercicio3.Items.Add(ejercicio);
                    comboBoxEjercicio4.Items.Add(ejercicio);
                    comboBoxEjercicio5.Items.Add(ejercicio);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los pacientes del terapeuta");
            }

        }

        /// <summary>
        /// Metodo que abre conexion con base de datos y
        /// carga en el combobox los pacientes del terapeuta
        /// </summary>
        /// <param name="sender"></param> Cargar ventana.
        /// <param name="e"></param> Evento de carga.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textBoxDescripcionEjercicio2.Text = null;
            textBoxDescripcionEjercicio3.Text = null;
            textBoxDescripcionEjercicio4.Text = null;
            textBoxDescripcionEjercicio5.Text = null;

            try
            {
                conexion = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos");
            }

            llenarComboBox();
        }

        /// <summary>
        /// Metodo que al elegir un paciente muestra su fotografia y su descripcion.
        /// </summary>
        /// <param name="sender"></param> ComboboxPaciente
        /// <param name="e"></param> Evento del Combobox
        private void comboBoxPaciente_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                string query = "Select * from pacientes where apellidosPaciente = '" + comboBoxPaciente.Text + "'";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    nombrePaciente= dr.GetString(1);
                    apellidosPaciente = dr.GetString(2);
                    nombreUsuarioPaciente = dr.GetString(3);
                    descripcion = dr.GetString(8);
                    textBoxDescripcion.Text = descripcion;

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
                MessageBox.Show("Error al obtener los datos del paciente seleccionado");
            }
        }

        /// <summary>
        /// Metodo que llama a obtenerDescripcionEjercicio al seleccionar un elemento en el combobox1
        /// </summary>
        /// <param name="sender"></param> comboBoxEjercicio1
        /// <param name="e"></param> Evento del Combobox
        private void comboBoxEjercicio1_DropDownClosed(object sender, EventArgs e)
        {
            obtenerDescripcionEjercicio(comboBoxEjercicio1, textBoxDescripcionEjercicio1);
            textBoxRepeticionesEjercicio1.IsEnabled = true;
            textBoxRepeticionesEjercicio1.Text = Convert.ToString(10);
        }

        /// <summary>
        /// Metodo que llama a obtenerDescripcionEjercicio al seleccionar un elemento en el combobox1
        /// </summary>
        /// <param name="sender"></param> comboBoxEjercicio1
        /// <param name="e"></param> Evento del Combobox
        private void comboBoxEjercicio2_DropDownClosed(object sender, EventArgs e)
        {
            obtenerDescripcionEjercicio(comboBoxEjercicio2, textBoxDescripcionEjercicio2);
            textBoxRepeticionesEjercicio2.IsEnabled = true;
            textBoxRepeticionesEjercicio2.Text = Convert.ToString(10);
        }

        /// <summary>
        /// Metodo que llama a obtenerDescripcionEjercicio al seleccionar un elemento en el combobox1
        /// </summary>
        /// <param name="sender"></param> comboBoxEjercicio1
        /// <param name="e"></param> Evento del Combobox
        private void comboBoxEjercicio3_DropDownClosed(object sender, EventArgs e)
        {
            obtenerDescripcionEjercicio(comboBoxEjercicio3, textBoxDescripcionEjercicio3);
            textBoxRepeticionesEjercicio3.IsEnabled = true;
            textBoxRepeticionesEjercicio3.Text = Convert.ToString(10);
        }

        /// <summary>
        /// Metodo que llama a obtenerDescripcionEjercicio al seleccionar un elemento en el combobox1
        /// </summary>
        /// <param name="sender"></param> comboBoxEjercicio1
        /// <param name="e"></param> Evento del Combobox
        private void comboBoxEjercicio4_DropDownClosed(object sender, EventArgs e)
        {
            obtenerDescripcionEjercicio(comboBoxEjercicio4, textBoxDescripcionEjercicio4);
            textBoxRepeticionesEjercicio4.IsEnabled = true;
            textBoxRepeticionesEjercicio4.Text = Convert.ToString(10);
        }

        /// <summary>
        /// Metodo que llama a obtenerDescripcionEjercicio al seleccionar un elemento en el combobox1
        /// </summary>
        /// <param name="sender"></param> comboBoxEjercicio1
        /// <param name="e"></param> Evento del Combobox
        private void comboBoxEjercicio5_DropDownClosed(object sender, EventArgs e)
        {
            obtenerDescripcionEjercicio(comboBoxEjercicio5, textBoxDescripcionEjercicio5);
            textBoxRepeticionesEjercicio5.IsEnabled = true;
            textBoxRepeticionesEjercicio5.Text = Convert.ToString(10);
        }

        /// <summary>
        /// Metodo que al elegir un ejercicio muestra su descripcion.
        /// Evitamos asi el codigo repetido en todos los eventos de cada comboboxEjercicioX
        /// </summary>
        /// <param name="comboBoxEjercicio"></param> comboboxEjercicio 
        /// <param name="textBoxDescripcionEjercicio"></param> textboxEjercicio
        private void obtenerDescripcionEjercicio(ComboBox comboBoxEjercicio, TextBox textBoxDescripcionEjercicio)
        {
            try
            {
                string query = "Select descripcion from ejercicios where ejercicio = '" + comboBoxEjercicio.Text + "'";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    string descripcion = dr.GetString(0);
                    textBoxDescripcionEjercicio.Text = descripcion;
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los datos del paciente seleccionado");
            }
        }

        /// <summary>
        /// Metodo que se produce al pulsar el boton cancelar:
        /// cerrar la ventana.
        /// cerrar conexion con la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar la conexion con la base de datos");
            }
        }

        /// <summary>
        /// Metodo que cierra la conexion con la BD al cerrar la ventana.
        /// </summary>
        /// <param name="sender"></param> Cerrar ventana.
        /// <param name="e"></param> Evento de cerrar.
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar la conexion con la base de datos");
            }
        }
        
        /// <summary>
        /// Metodo que comprueba el formulario de los entrenamientos.
        /// y manda guardar los datos en la base de datos.
        /// </summary>
        /// <param name="sender"></param> Boton Crear Ejercicios.
        /// <param name="e"></param> Eventos del boton.
        private void buttonAsignarEjercicio_Click(object sender, RoutedEventArgs e)
        {
            string nombreCompletoPaciente = nombrePaciente + " " + apellidosPaciente;
            string nombreTerapeuta = Relacion.getTerapeuta(nombrePaciente, apellidosPaciente);
            repeticiones1 = Convert.ToInt32(textBoxRepeticionesEjercicio1.Text);
            if(textBoxRepeticionesEjercicio2.IsEnabled == true)
                repeticiones2 = Convert.ToInt32(textBoxRepeticionesEjercicio2.Text);
            if (textBoxRepeticionesEjercicio3.IsEnabled == true)
                repeticiones3 = Convert.ToInt32(textBoxRepeticionesEjercicio3.Text);
            if (textBoxRepeticionesEjercicio4.IsEnabled == true)
                repeticiones4 = Convert.ToInt32(textBoxRepeticionesEjercicio4.Text);
            if (textBoxRepeticionesEjercicio5.IsEnabled == true)
                repeticiones5 = Convert.ToInt32(textBoxRepeticionesEjercicio5.Text);

            if (Entrenamiento.RegistrarEntrenamiento(nombreCompletoPaciente, nombreUsuarioPaciente,nombreTerapeuta,nombreUsuarioTerapeuta,comboBoxEjercicio1.Text, repeticiones1,comboBoxEjercicio2.Text, repeticiones2,comboBoxEjercicio3.Text, repeticiones3, comboBoxEjercicio4.Text, repeticiones4, comboBoxEjercicio5.Text, repeticiones5, null, null,null,null ) > 0)
            {
                MessageBox.Show("Entrenamiento registrado con exito.");
                this.Close();
            }
            else
            {
                if (textBoxDescripcionEjercicio1.Text == "")
                    MessageBox.Show("Al menos tienes que asignar un ejercicio.");
                else
                    MessageBox.Show("Error al registrar entrenamiento. Pruebe de nuevo.");
            }
        }
    }

}
