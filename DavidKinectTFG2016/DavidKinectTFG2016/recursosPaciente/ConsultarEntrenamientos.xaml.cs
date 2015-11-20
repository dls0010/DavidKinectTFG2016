using System;
using System.Collections.Generic;
using System.Data;
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

namespace DavidKinectTFG2016.recursosPaciente
{
    /// <summary>
    /// Lógica de interacción para ConsultarEntrenamientos.xaml
    /// </summary>
    public partial class ConsultarEntrenamientos : Window
    {
        string nombreUsuarioPaciente;
        SqlConnection conexion;
        public ConsultarEntrenamientos(string usuario)
        {
            nombreUsuarioPaciente = usuario;
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que se ejecuta al abrirse la ventana, carga los entrenamientos.
        /// asignados al paciente en el datagrid.
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
                string query = "Select * from entrenamientos where usuarioPaciente = '" + nombreUsuarioPaciente + "'";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.ExecuteNonQuery();

                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable dt = new DataTable("entrenamientos");
                adaptador.Fill(dt);
                dataGridEntrenamientos.ItemsSource = dt.DefaultView;
                adaptador.Update(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los datos en la tabla");
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
    }
}
