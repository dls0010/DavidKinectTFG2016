using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DavidKinectTFG2016.clases
{
    /// <summary>
    /// Clase Paciente que registrar en la base de datos.
    /// </summary>
    class Paciente
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
        /// <returns>
        /// 0: Ha ocurrido un fallo. No se ha llevado a cabo la inserción.
        /// != 0 Proceso realizado correctamente.
        /// </returns>
        public static int RegistrarPaciente(string pNombre, string pApellidos, string pNombreUsuario, string pNIF, string pTelefono, string pNacimiento, string pEstado)
        {
            int resultado = 0;
            int error = 0;
            SqlConnection conn;

            try
            {
                conn = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                return error;
            }

            try {
                SqlCommand comando = new SqlCommand(string.Format("Insert Into Pacientes (nombrePaciente,apellidosPaciente,usuario, nifPaciente,telefonoPaciente,nacimientoPaciente,estadoPaciente) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", pNombre, pApellidos, pNombreUsuario, pNIF, pTelefono, pNacimiento, pEstado), conn);

                resultado = comando.ExecuteNonQuery();
                conn.Close();

                return resultado;
            }
            catch (Exception ex)
            {
                return error;
            }
        }

        /// <summary>
        /// Metodo que obtiene todos los datos de la base de datos tabla Pacientes.
        /// </summary>
        /// <returns>
        /// DataTable con todos los datos de la tabla Pacientes.
        /// </returns>
        public static DataTable getPacientes()
        {
            DataTable table;
            try {
                SqlConnection con = BDComun.ObtnerConexion();
                SqlCommand comando = new SqlCommand();
                comando.Connection = con;
                comando.CommandType = CommandType.Text;
                comando.CommandText = "Select * from Pacientes";
                SqlDataReader reader = comando.ExecuteReader();
                table = new DataTable();
                table.Load(reader);
                return table;
            }
            catch(Exception ex)
            {
                return null;
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
            try {
                SqlConnection con = BDComun.ObtnerConexion();
                SqlCommand comando = new SqlCommand();
                comando.Connection = con;
                comando.CommandType = CommandType.Text;
                comando.CommandText = string.Format("Select nombrePaciente, apellidosPaciente from Pacientes where usuario = '" + usuario + "'");
                SqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    nombre = reader.GetString(0);
                    apellido = reader.GetString(1);
                    nombreCompleto[0] = nombre;
                    nombreCompleto[1] = apellido;
                }
                return nombreCompleto;
            }catch(Exception ex)
            {
                return nombreCompleto;
            }
        }
    }
}
