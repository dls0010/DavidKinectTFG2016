using DavidKinectTFG2016.clases;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Lógica de interacción para FinalizarPaciente.xaml
    /// </summary>
    public partial class FinalizarPaciente : Window
    {
        string nombreUsuario;
        SqlConnection conexion;
        string nombreTerapeuta="";
        public FinalizarPaciente(string usuario)
        {
            nombreUsuario = usuario;
            InitializeComponent();
        }
        /// <summary>
        /// Metodo que carga la tabla del DataGrid en el momento de cargar la ventana.
        /// </summary>
        /// <param name="sender"></param> Cargar ventana.
        /// <param name="e"></param> Evento de carga.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboBoxNombre.Items.Clear();
            textBoxNombre.Clear();
            textBoxApellidos.Clear();
            textBoxIDPaciente.Clear();
            textBoxNIF.Clear();
            textBoxTelefono.Clear();
            textBoxNacimiento.Clear();
            textBoxEstado.Clear();
            textBoxDescripcion.Clear();

            try
            {
                conexion = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.ToString());
            }
            
            try
            {
                nombreTerapeuta = Terapeuta.getNombreTerapeuta(nombreUsuario);
                llenarComboBox();
                string query = "Select nombrePaciente, apellidosPaciente, fechaInicio, fechaFin from relaciones where nombreTerapeuta = '"+nombreTerapeuta+"' ";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.ExecuteNonQuery();

                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable dt = new DataTable("relaciones");
                adaptador.Fill(dt);
                dataGrid.ItemsSource = dt.DefaultView;
                adaptador.Update(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos en la tabla: " + ex.ToString());
            }
        }
        /// <summary>
        /// Metodo adicional para llenar el combobox con los nombres de los pacientes libres.
        /// </summary>
        private void llenarComboBox()
        {
            try
            {
                string query = "Select nombrePaciente from relaciones where nombreTerapeuta = '"+nombreTerapeuta+"'";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    string nombre = dr.GetString(0);
                    comboBoxNombre.Items.Add(nombre);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los pacientes del terapeuta: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que rellena los textbox de la pagina al seleccionar un paciente en el combobox.
        /// </summary>
        /// <param name="sender"></param> ComboboxNombre.
        /// <param name="e"></param> Evento del comboBoxNombre.
        private void comboBoxNombre_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                string query = "Select * from pacientes where nombrePaciente = '" + comboBoxNombre.Text + "'";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    int idPaciente = dr.GetInt32(0);
                    string nombre = dr.GetString(1);
                    string apellidos = dr.GetString(2);
                    string nif = dr.GetString(4);
                    string telefono = dr.GetString(5);
                    string nacimiento = dr.GetDateTime(6).ToString();
                    string estado = dr.GetString(7);
                    string descripcion = dr.GetString(8);

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

                    textBoxIDPaciente.Text = idPaciente.ToString();
                    textBoxNombre.Text = nombre;
                    textBoxApellidos.Text = apellidos;
                    textBoxNIF.Text = nif;
                    textBoxTelefono.Text = telefono;
                    textBoxNacimiento.Text = nacimiento;
                    textBoxEstado.Text = estado;
                    textBoxDescripcion.Text = descripcion;
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los datos del paciente seleccionado: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que finalizara el tratamiento en la tabla Relaciones.
        /// Y pondrá el estado del Paciente a Recuperado.
        /// </summary>
        /// <param name="sender"></param> Boton de adquerir.
        /// <param name="e"></param> Evento de adquerir.
        private void buttonFinalizar_Click(object sender, RoutedEventArgs e)
        {
            DateTime hoy = DateTime.Today;
            string fechaFin = hoy.ToString("yyyy/MM/dd");
            try
            {
                if (Relacion.finalizarRelacion(textBoxIDPaciente.Text, nombreUsuario, textBoxNombre.Text, textBoxApellidos.Text, fechaFin) > 0)
                {
                    MessageBox.Show("Paciente recuperado.");
                    string query = "Update pacientes set estadoPaciente ='Recuperado' where nifPaciente = '" + textBoxNIF.Text + "'";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.ExecuteNonQuery();
                    this.Window_Loaded(this, e);
                }
                else
                {
                    MessageBox.Show("Fallo al finalizar el tratamiento del paciente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fallo al actualizar el campo estado del paciente: " + ex.ToString());
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
                MessageBox.Show("Error al cerrar la conexion con la base de datos: " + ex.ToString());
            }
        }
    }
}