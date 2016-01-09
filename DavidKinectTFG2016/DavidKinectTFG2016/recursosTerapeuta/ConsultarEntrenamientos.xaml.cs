using System;
using System.Collections.Generic;
using System.Data;
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
using MySql.Data.MySqlClient;

namespace DavidKinectTFG2016.recursosTerapeuta
{
    /// <summary>
    /// Lógica de interacción para ConsultarEntrenamientos.xaml
    /// </summary>
    public partial class ConsultarEntrenamientos : Window
    {
        string nombreUsuarioTerapeuta;
        MySqlConnection conexion;
        int idEntrenamiento;
        public ConsultarEntrenamientos(string usuario)
        {
            nombreUsuarioTerapeuta = usuario;
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que se ejecuta al abrirse la ventana, carga los entrenamientos.
        /// asignados por el terapeuta en el datagrid.
        /// </summary>
        /// <param name="sender"></param> Ventana.
        /// <param name="e"></param> Eventos de la ventana.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion = BDComun.ObtnerConexion();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al conectar con la base de datos");
            }
            try
            {
                string query = "Select * from entrenamientos where usuarioTerapeuta = '" + nombreUsuarioTerapeuta + "'";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                comando.ExecuteNonQuery();

                MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
                DataTable dt = new DataTable("entrenamientos");
                adaptador.Fill(dt);
                dataGridEntrenamientos.ItemsSource = dt.DefaultView;
                adaptador.Update(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los datos en la tabla: " + ex.ToString());
            }
            llenarComboBox();
        }

        /// <summary>
        /// Metodo adicional para llenar el combobox con los nombres de los pacientes libres.
        /// </summary>
        private void llenarComboBox()
        {
            try
            {
                string query = "Select * from entrenamientos where fechaEntrenamiento is not null";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                MySqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    idEntrenamiento = dr.GetInt32(0);
                    comboBoxEntrenamiento.Items.Add(idEntrenamiento);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los entrenamientos realizados: " + ex.ToString());
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
            try
            {
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar la conexion con la base de datos: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que al elegir un ejercicio que queremos dar feedback.
        /// </summary>
        /// <param name="sender"></param> ComboboxEjercicios
        /// <param name="e"></param> Evento del Combobox
        private void comboBoxEntrenamiento_DropDownClosed(object sender, EventArgs e)
        {
            string usuarioPaciente = "";
            string feedbackPaciente = "";
            try
            {
                string query = "Select usuarioPaciente, feedbackPaciente from entrenamientos where idEntrenamiento = '" + comboBoxEntrenamiento.Text + "'";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                MySqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    usuarioPaciente = dr.GetString(0);
                    if (dr["feedbackPaciente"] != DBNull.Value)
                        feedbackPaciente = dr.GetString(1);
                    else
                        feedbackPaciente = "El paciente no ha dejado feedback";
                    textBoxFeedbackPaciente.Text = feedbackPaciente;
                }
                dr.Close();

                query = "Select descripcionPaciente, imagenPaciente from pacientes where usuario = '" + usuarioPaciente + "'";
                comando = new MySqlCommand(query, conexion);
                dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    string descripcionPaciente = dr.GetString(0);
                    textBoxDescripcionPaciente.Text = descripcionPaciente;

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
                MessageBox.Show("Error al obtener los datos del entrenamiento seleccionado: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que asignará el feedback que de el terapeuta al paciente sobre su entrenamiento.
        /// </summary>
        /// <param name="sender"></param> Boton enviar.
        /// <param name="e"></param> Eventos del botón.
        private void buttonEnviar_Click(object sender, RoutedEventArgs e)
        {
            string feedbackTerapeuta = textBoxFeedbackTerapeuta.Text;
            idEntrenamiento = Convert.ToInt32(comboBoxEntrenamiento.Text);
            if (feedbackTerapeuta != "")
            {
                if (Entrenamiento.AñadirFeedback(idEntrenamiento, nombreUsuarioTerapeuta, feedbackTerapeuta) > 0)
                {
                    MessageBox.Show("Feedback enviado con exito.");
                }
                else
                {
                    MessageBox.Show("Error al enviar el feedback. Pruebe de nuevo.");
                }
            }
            else
            {
                MessageBox.Show("Error, no has escrito feedback para publicar.");
            }
        }
    }
}
