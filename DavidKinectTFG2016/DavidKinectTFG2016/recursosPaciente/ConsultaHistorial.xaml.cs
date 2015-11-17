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
    /// Lógica de interacción para ConsultaHistorial.xaml
    /// </summary>
    public partial class ConsultaHistorial : Window
    {
        SqlConnection conexion;
        String nombreUsuario;
        public ConsultaHistorial(string usuario)
        {
            nombreUsuario = usuario;
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que carga la tabla al abrirse la ventana.
        /// </summary>
        /// <param name="sender"></param> Ventana.
        /// <param name="e"></param>Evento de ventana.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try {
                conexion = BDComun.ObtnerConexion();
            }catch(Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos");
            }

            string query = "Select * from historial where usuarioPaciente = '" + nombreUsuario + "'";

            try {
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.ExecuteNonQuery();

                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable dt = new DataTable("historial");
                adaptador.Fill(dt);
                dataGrid.ItemsSource = dt.DefaultView;
                adaptador.Update(dt);
            }catch(Exception ex)
            {
                MessageBox.Show("Error al cargar los datos en la tabla");
            }
        }
    }
}
