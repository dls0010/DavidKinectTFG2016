using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.IO;
using System.Windows;
using MySql.Data.MySqlClient;

namespace DavidKinectTFG2016.clases
{
    /// <summary>
    /// Clase Paciente que registrar en la base de datos.
    /// </summary>
    public class Paciente
    {
        /// <summary>
        /// Metodo que controla el registro un nuevo paciente en la base de datos.
        /// </summary>
        /// <param name="pNombre"></param> Nombre del paciente.
        /// <param name="pApellidos"></param> Apellidos del paciente. 
        /// <param name="pNombreUsuario"></param> Nombre Usuario del paciente 
        /// <param name="pNIF"></param> NIF del paciente.
        /// <param name="pNacimiento"></param> Nacimiento del paciente.
        /// <param name="pEstado"></param> Estado del paciente.
        /// <param name="pImagen"></param> Imagen del Terapeuta.
        /// <param name="pDescripcion"></param> Descripcion del Terapeuta.
        /// <returns>
        /// 0: Ha ocurrido un fallo. No se ha llevado a cabo la inserción.
        /// != 0 Proceso realizado correctamente.
        /// </returns>
        public static int RegistrarPaciente(string pNombre, string pApellidos, string pNombreUsuario, string pNIF, string pTelefono, string pNacimiento, string pEstado, string pDescripcion, string pathImagen)
        {
            byte[] imagen = null;
            if (pathImagen != null)
            {
                FileStream fstream = new FileStream(pathImagen, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fstream);
                imagen = br.ReadBytes((int)fstream.Length);
            }

            int resultado = 1;
            int error = 0;
            MySqlConnection conn;

            try
            {
                conn = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return error;
            }

            string query = "Insert Into pacientes (nombrePaciente,apellidosPaciente,usuario, nifPaciente,telefonoPaciente,nacimientoPaciente,estadoPaciente,descripcionPaciente,imagenPaciente)" + "values('" + pNombre + "','" + pApellidos + "','" + pNombreUsuario + "','" + pNIF + "','" + pTelefono + "','" + pNacimiento + "','" + pEstado + "','" + pDescripcion + "',@IMG);";
            MySqlCommand comando = new MySqlCommand(query, conn);
            MySqlDataReader reader;
            try
            {
                if (pathImagen != null)
                    comando.Parameters.Add(new MySqlParameter("@IMG", imagen));

                reader = comando.ExecuteReader();

                conn.Close();

                return resultado;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return error;
            }
        }
        
        /// <summary>
        /// Metodo que obtiene todos los datos de la base de datos tabla Pacientes.
        /// </summary>
        /// <param name="usuario"></param> nombre de usuario del paciente.
        /// <returns>
        /// String con el nombre del paciente correspondiente.
        /// </returns>
        public static string[] getPaciente(string usuario)
        {
            string nombre = "";
            string apellido = "";
            string[] nombreCompleto = new string[] { "nombre", "apellido" };
            try
            {
                MySqlConnection con = BDComun.ObtnerConexion();
                MySqlCommand comando = new MySqlCommand();
                comando.Connection = con;
                comando.CommandType = CommandType.Text;
                comando.CommandText = string.Format("Select nombrePaciente, apellidosPaciente from pacientes where usuario = '" + usuario + "'");
                MySqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    nombre = reader.GetString(0);
                    apellido = reader.GetString(1);
                    nombreCompleto[0] = nombre;
                    nombreCompleto[1] = apellido;
                }
                return nombreCompleto;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return nombreCompleto;
            }
        }
        
        /// <summary>
        /// Metodo que obtiene el nombre completo del paciente pasandole el nombre de usuario.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>
        /// nombre: nombre completo del paciente
        /// </returns>
        public static string getNombreCompletoPaciente(string usuario)
        {
            string nombreCompleto = "";

            try
            {
                MySqlConnection con = BDComun.ObtnerConexion();
                MySqlCommand comando = new MySqlCommand();
                comando.Connection = con;
                comando.CommandType = CommandType.Text;
                comando.CommandText = string.Format("Select nombrePaciente, apellidosPaciente from pacientes where usuario = '" + usuario + "'");
                MySqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    nombreCompleto = reader.GetString(0);
                    nombreCompleto = nombreCompleto + " ";
                    nombreCompleto = nombreCompleto + reader.GetString(1);
                }
                return nombreCompleto;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return nombreCompleto;
            }
        }

        /// <summary>
        /// Metodo que obtiene el nombre de usuario de un paciente pasandole los apellidos.
        /// </summary>
        /// <param name="apellidos"></param>
        /// <returns>
        /// nombre de usuario.
        /// </returns>
        public static string getUsuario(string apellidos)
        {
            string nombreUsuario = "";

            try
            {
                MySqlConnection con = BDComun.ObtnerConexion();
                MySqlCommand comando = new MySqlCommand();
                comando.Connection = con;
                comando.CommandType = CommandType.Text;
                comando.CommandText = string.Format("Select usuario from pacientes where apellidosPaciente = '" + apellidos + "'");
                MySqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    nombreUsuario = reader.GetString(0);
                }
                return nombreUsuario;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return nombreUsuario;
            }
        }

        /// <summary>
        /// Metodo publico que obtiene un adaptador que contiene
        /// todos los pacientes de la BD.
        /// </summary>
        /// <returns>
        /// MySqlDataAdapter adaptador que contiene todos los pacientes
        /// </returns>
        public static MySqlDataAdapter getAdaptadorPacientes()
        {
            MySqlCommand comando = null;
            MySqlConnection conexion = null;
            MySqlDataAdapter adaptador = null;
            try
            {
                conexion = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.ToString());
            }

            string query = "Select idPaciente,nombrePaciente,apellidosPaciente,usuario,nifPaciente,telefonoPaciente,nacimientoPaciente,estadoPaciente,descripcionPaciente from pacientes";
            try
            {
                comando = new MySqlCommand(query, conexion);
                comando.ExecuteNonQuery();
                adaptador = new MySqlDataAdapter(comando);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la tabla: " + ex.ToString());
            }
            return adaptador;
        }
    }
}
