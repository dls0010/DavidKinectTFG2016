using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DavidKinectTFG2016.clases
{
    class Relacion
    {
        /// <summary>
        /// Metodo de registrar nueva relacion Paciente Terapeuta.
        /// </summary>
        /// <param name="pIdPaciente"></param> Id Paciente.
        /// <param name="pUsuarioTerapeuta"></param> Usuario del Terapeuta.
        /// <param name="pNombrePaciente"></param> Nombre del paciente.
        /// <param name="pApellidosPaciente"></param> Apellidos del paciente.
        /// <param name="pFechaInicio"></param> Fecha Inicio del tratamiento.
        /// <returns>
        /// 0: Ha ocurrido un fallo. No se ha llevado a cabo la inserción.
        /// != 0 Proceso realizado correctamente.
        /// </returns>
        public static int registrarRelacion(string pIdPaciente, string pUsuarioTerapeuta, string pNombrePaciente, string pApellidosPaciente, string pFechaInicio)
        {
            int resultado = 0;
            string pFechaFin = "en tratamiento";
            int pIdTerapeuta = obtenerIdTerapeuta(pUsuarioTerapeuta);
            string pNombreTerapeuta = obtenerNombreTerapeuta(pUsuarioTerapeuta);

            SqlConnection conn = BDComun.ObtnerConexion();
            SqlCommand comando = new SqlCommand(string.Format("Insert Into Relaciones (idPaciente,idTerapeuta,nombrePaciente,apellidosPaciente,nombreTerapeuta,fechaInicio, fechaFin) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", pIdPaciente, pIdTerapeuta, pNombrePaciente, pApellidosPaciente, pNombreTerapeuta, pFechaInicio, pFechaFin), conn);

            resultado = comando.ExecuteNonQuery();
            conn.Close();

            return resultado;
        }
        /// <summary>
        /// Metodo que nos da todos las relaciones de las Base de datos.
        /// </summary>
        /// <returns></returns>
        public static DataTable getRelaciones()
        {
            SqlConnection con = BDComun.ObtnerConexion();
            SqlCommand comando = new SqlCommand();
            comando.Connection = con;
            comando.CommandType = CommandType.Text;
            comando.CommandText = "Select * from Relaciones";
            SqlDataReader reader = comando.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return table;
        }

        /// <summary>
        /// Metodo adicional usado obtener el id del terapeuta asignado.
        /// </summary>
        /// <param name="terapeuta"></param>Nombre de usuario del Terapeuta.
        /// <returns>string: id del terapeuta asignado.
        /// </returns>
        public static int obtenerIdTerapeuta(string terapeuta)
        {
            int idTerapeuta = -3;
            using (SqlConnection conexion = BDComun.ObtnerConexion())
            {
                string query = "SELECT idTerapeuta FROM Terapeutas WHERE usuario=@Usuario";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("Usuario", terapeuta);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    idTerapeuta = reader.GetInt32(0);
                }
                reader.Close();
            }
            return idTerapeuta;
        }

        /// <summary>
        /// Metodo adicional usado para obtener el Nombre del Terapeuta.
        /// </summary>
        /// <param name="terapeuta"></param>Nombre de usuario del Terapeuta.
        /// <returns>String: nombre del terapeuta asignado.
        /// </returns>
        public static string obtenerNombreTerapeuta(string terapeuta)
        {
            string nombreTerapeuta = "";
            using (SqlConnection conexion = BDComun.ObtnerConexion())
            {
                string query = "SELECT nombreTerapeuta FROM Terapeutas WHERE usuario=@Usuario";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("Usuario", terapeuta);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    nombreTerapeuta = reader.GetString(0);
                }
                reader.Close();
            }
            return nombreTerapeuta;
        }

        /// <summary>
        /// Metodo que obtiene todos los datos de la base de datos tabla Relaciones.
        /// </summary>
        /// <param name="usuario"></param> nombre de usuario del paciente.
        /// <returns>
        /// String con el nombre del terapeuta correspondiente.
        /// </returns>
        public static string getTerapeuta(string nombrePaciente, string apellidosPaciente)
        {
            string usuarioTerapeuta = "";
            SqlConnection con = BDComun.ObtnerConexion();
            SqlCommand comando = new SqlCommand();
            comando.Connection = con;
            comando.CommandType = CommandType.Text;
            comando.CommandText = string.Format("Select nombreTerapeuta from Relaciones where nombrePaciente = '" + nombrePaciente + "' and apellidosPaciente = '"+apellidosPaciente+"'");
            SqlDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                usuarioTerapeuta = reader.GetString(0);
            }
            return usuarioTerapeuta;
        }
    }
}
