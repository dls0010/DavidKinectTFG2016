using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace DavidKinectTFG2016.clases
{
    public class Relacion
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
            int error = 0;
            string pFechaFin = "en tratamiento";
            int pIdTerapeuta = obtenerIdTerapeuta(pUsuarioTerapeuta);
            string pNombreTerapeuta = obtenerNombreTerapeuta(pUsuarioTerapeuta);

            if (pIdTerapeuta < 0 || pNombreTerapeuta == null)
                return error;

            MySqlConnection conn;
            try
            {
                conn = BDComun.ObtnerConexion();

                MySqlCommand comando = new MySqlCommand(string.Format("Insert Into relaciones (idPaciente,idTerapeuta,nombrePaciente,apellidosPaciente,nombreTerapeuta,fechaInicio, fechaFin) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", pIdPaciente, pIdTerapeuta, pNombrePaciente, pApellidosPaciente, pNombreTerapeuta, pFechaInicio, pFechaFin), conn);

                resultado = comando.ExecuteNonQuery();
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
        /// Metodo que nos da todos las relaciones de las Base de datos.
        /// </summary>
        /// <returns>
        /// DataTable que contiene las relaciones que hay en la base de datos.
        /// </returns>
        public static DataTable getRelaciones()
        {
            try
            {
                MySqlConnection con = BDComun.ObtnerConexion();
                MySqlCommand comando = new MySqlCommand();
                comando.Connection = con;
                comando.CommandType = CommandType.Text;
                comando.CommandText = "Select * from relaciones";
                MySqlDataReader reader = comando.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// Metodo adicional usado obtener el id del terapeuta asignado.
        /// </summary>
        /// <param name="terapeuta"></param>Nombre de usuario del Terapeuta.
        /// <returns>
        /// int: id del terapeuta asignado.
        /// </returns>
        public static int obtenerIdTerapeuta(string terapeuta)
        {
            int error = -1;
            try
            {
                int idTerapeuta = -3;
                using (MySqlConnection conexion = BDComun.ObtnerConexion())
                {
                    string query = "SELECT idTerapeuta FROM terapeutas WHERE usuario=@Usuario";
                    MySqlCommand cmd = new MySqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("Usuario", terapeuta);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        idTerapeuta = reader.GetInt32(0);
                    }
                    reader.Close();
                }
                return idTerapeuta;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return error;
            }
        }

        /// <summary>
        /// Metodo adicional usado obtener el id del paciente asignado.
        /// </summary>
        /// <param name="paciente"></param>Nombre de usuario del Paciente.
        /// <returns>
        /// int: id del paciente asignado.
        /// </returns>
        public static int obtenerIdPaciente(string paciente)
        {
            int error = -1;
            try
            {
                int idPaciente = -3;
                using (MySqlConnection conexion = BDComun.ObtnerConexion())
                {
                    string query = "SELECT idPaciente FROM pacientes WHERE usuario=@Usuario";
                    MySqlCommand cmd = new MySqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("Usuario", paciente);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        idPaciente = reader.GetInt32(0);
                    }
                    reader.Close();
                }
                return idPaciente;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return error;
            }
        }

        /// <summary>
        /// Metodo adicional usado para obtener el Nombre del Terapeuta.
        /// </summary>
        /// <param name="terapeuta"></param>Nombre de usuario del Terapeuta.
        /// <returns>
        /// String: nombre del terapeuta asignado.
        /// </returns>
        public static string obtenerNombreTerapeuta(string terapeuta)
        {
            try
            {
                string nombreTerapeuta = "";
                using (MySqlConnection conexion = BDComun.ObtnerConexion())
                {
                    string query = "SELECT nombreTerapeuta FROM terapeutas WHERE usuario=@Usuario";
                    MySqlCommand cmd = new MySqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("Usuario", terapeuta);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        nombreTerapeuta = reader.GetString(0);
                    }
                    reader.Close();
                }
                return nombreTerapeuta;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return null;
            }
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
            try
            {
                string usuarioTerapeuta = "";
                MySqlConnection con = BDComun.ObtnerConexion();
                MySqlCommand comando = new MySqlCommand();
                comando.Connection = con;
                comando.CommandType = CommandType.Text;
                comando.CommandText = string.Format("Select nombreTerapeuta from relaciones where nombrePaciente = '" + nombrePaciente + "' and apellidosPaciente = '" + apellidosPaciente + "'");
                MySqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    usuarioTerapeuta = reader.GetString(0);
                }
                return usuarioTerapeuta;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return null;
            }
        }

        /// <summary>
        /// Metodo que establece la fecha de hoy como fecha Fin del Tratamiento.
        /// </summary>
        /// <param name="idPaciente"></param> id del Paciente.
        /// <param name="pUsuarioTerapeuta"></param> Nombre de usuario del terapeuta.
        /// <param name="nombrePaciente"></param>Nombre del paciente.
        /// <param name="apellidosPaciente"></param> Apellidos del paciente.
        /// <param name="fechaFin"></param> Fecha que finaliza el tratamiento.
        /// <returns>
        /// 1-> bien.
        /// 0 -> mal.
        /// </returns>
        public static int finalizarRelacion(string idPaciente, string pUsuarioTerapeuta, string nombrePaciente, string apellidosPaciente, string fechaFin)
        {
            int resultado = 0;
            int error = 0;
            int pIdTerapeuta = obtenerIdTerapeuta(pUsuarioTerapeuta);
            string pNombreTerapeuta = obtenerNombreTerapeuta(pUsuarioTerapeuta);

            if (pIdTerapeuta < 0 || pNombreTerapeuta == null)
                return error;

            MySqlConnection conn;
            try
            {
                conn = BDComun.ObtnerConexion();
                string query = "Update relaciones set fechaFin ='" + fechaFin + "' where nombrePaciente = '" + nombrePaciente + "'and idTerapeuta ='" + pIdTerapeuta + "' ";

                MySqlCommand comando = new MySqlCommand(query, conn);
                resultado = comando.ExecuteNonQuery();
                conn.Close();

                return resultado;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return error;
            }
        }
    }
}
