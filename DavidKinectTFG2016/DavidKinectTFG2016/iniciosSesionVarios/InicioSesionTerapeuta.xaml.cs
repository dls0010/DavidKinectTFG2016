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
using DavidKinectTFG2016.recursosTerapeuta;
using System.IO;
using MySql.Data.MySqlClient;

namespace DavidKinectTFG2016.iniciosSesionVarios
{
    /// <summary>
    /// Lógica de interacción para InicioSesionTerapeuta.xaml
    /// </summary>
    public partial class InicioSesionTerapeuta : Window
    {
        string nombreUsuario;
        MySqlConnection conexion;
        public InicioSesionTerapeuta(string nombre)
        {
            nombreUsuario = nombre;
            InitializeComponent();
            conexion = BDComun.ObtnerConexion();
        }

        /// <summary>
        /// Metodo que carga la foto del terapeuta en el momento de cargar la ventana.
        /// </summary>
        /// <param name="sender"></param> Cargar ventana.
        /// <param name="e"></param> Evento de carga.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "Select * from terapeutas where usuario = '" + nombreUsuario + "'";
                MySqlCommand comando = new MySqlCommand(query, conexion);
                MySqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    byte[] imagen = (byte[])(dr["imagenTerapeuta"]);
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
                MessageBox.Show("Error al obtener los datos del terapeuta: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que enlaza para comprobar que pacientes estan al cargo del terapeuta.
        /// </summary>
        /// <param name="sender"></param> Boton Consultar.
        /// <param name="e"></param> Evento del boton.
        private void buttonConsultar_Click(object sender, RoutedEventArgs e)
        {
            ConsultarPacientes consultar = new ConsultarPacientes(nombreUsuario);
            consultar.Show();
        }

        private void buttonAdquirir_Click(object sender, RoutedEventArgs e)
        {
            AdquirirPacientes adquirir = new AdquirirPacientes(nombreUsuario);
            adquirir.Show();
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
        /// Metodo que abre ventana para crear un entrenamiento a un determinado paciente.
        /// </summary>
        /// <param name="sender"></param> Boton Crear...    
        /// <param name="e"></param> Evento del botón.
        private void buttonCrear_Click(object sender, RoutedEventArgs e)
        {
            CrearEntrenamiento crear = new CrearEntrenamiento(nombreUsuario);
            crear.Show();
        }

        /// <summary>
        /// Metodo que abre la ventana de consulta de entrenamientos de los pacientes.
        /// </summary>
        /// <param name="sender"></param> Boton consultar.
        /// <param name="e"></param> Eventos del boton.
        private void buttonConsultarEntrenamientos_Click(object sender, RoutedEventArgs e)
        {
            ConsultarEntrenamientos consultar = new ConsultarEntrenamientos(nombreUsuario);
            consultar.Show();
        }

        /// <summary>
        /// Metodo que abre ventana para que el terapeuta pueda consultar que ejercicios hay en la BD.
        /// </summary>
        /// <param name="sender"></param> Boton Consultar.
        /// <param name="e"></param> Eventos del boton.
        private void buttonConsultarEjercicios_Click(object sender, RoutedEventArgs e)
        {
            ConsultarEjercicios consultar = new ConsultarEjercicios();
            consultar.Show();
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
        /// Metodo que finaliza un tratamiento, poniendo la fecha fin del tratamiento en la relacion.
        /// Y en el campo estadoPaciente a "recuperado".
        /// </summary>
        /// <param name="sender"></param> Boton finalizar tratamiento.
        /// <param name="e"></param> evento del botón finalizar.
        private void buttonFinalizarTratamiento_Click(object sender, RoutedEventArgs e)
        {
            FinalizarPaciente finalizar = new FinalizarPaciente(nombreUsuario);
            finalizar.Show();
        }
    }
}
