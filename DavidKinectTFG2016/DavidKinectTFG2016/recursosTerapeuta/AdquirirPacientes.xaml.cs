using System;
using System.Collections.Generic;
using System.Data;
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
using System.IO;
using MySql.Data.MySqlClient;

namespace DavidKinectTFG2016.recursosTerapeuta
{
    /// <summary>
    /// Lógica de interacción para AdquirirPacientes.xaml
    /// </summary>
    public partial class AdquirirPacientes : Window
    {
        string nombreUsuario;
        MySqlConnection conexion;
        public AdquirirPacientes(string usuario)
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

            llenarComboBox();
            try
            {
                string query = "Select nombrePaciente,apellidosPaciente,usuario,nifPaciente,telefonoPaciente,nacimientoPaciente,estadoPaciente,descripcionPaciente from pacientes where estadoPaciente = 'Libre'";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.ExecuteNonQuery();

                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable dt = new DataTable("pacientes");
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
                string query = "Select * from pacientes where estadoPaciente = 'Libre'";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                MySqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    string nombre = dr.GetString(1);
                    comboBoxNombre.Items.Add(nombre);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los pacientes libres: " + ex.ToString());
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
                MySqlCommand comando = new MySqlCommand(query, conexion);
                MySqlDataReader dr = comando.ExecuteReader();
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
        /// Metodo que creará un nuevo registro en la tabla Relaciones.
        /// </summary>
        /// <param name="sender"></param> Boton de adquerir.
        /// <param name="e"></param> Evento de adquerir.
        private void buttonAdquirir_Click(object sender, RoutedEventArgs e)
        {
            DateTime hoy = DateTime.Today;
            string fechaInicio = hoy.ToString("yyyy/MM/dd");
            if (comboBoxNombre.Text != "")
            {
                try
                {
                    if (Relacion.registrarRelacion(textBoxIDPaciente.Text, nombreUsuario, textBoxNombre.Text, textBoxApellidos.Text, fechaInicio) > 0)
                    {
                        MessageBox.Show("Paciente atendido.");
                        string query = "Update pacientes set estadoPaciente ='En tratamiento' where nifPaciente = '" + textBoxNIF.Text + "'";
                        MySqlCommand comando = new MySqlCommand(query, conexion);
                        comando.ExecuteNonQuery();
                        this.Window_Loaded(this, e);
                    }
                    else
                    {
                        MessageBox.Show("Fallo al asignar el paciente.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fallo al actualizar el campo estado del paciente: " + ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Debe de seleccionar el nombre del paciente elegido");
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
