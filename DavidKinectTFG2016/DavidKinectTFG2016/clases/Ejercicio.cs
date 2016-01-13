using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace DavidKinectTFG2016.clases
{
    /// <summary>
    /// Clase Ejercicio que registra los ejercicios en la base de datos.
    /// </summary>
    public class Ejercicio
    {
        /// <summary>
        /// Metodo que controla el registro una nueva actividad del paciente en la base de datos.
        /// </summary>
        /// <param name="nombreUsuarioPaciente"></param> Nombre de usuario del paciente.
        /// <param name="ejercicio"></param> ejercicio realizado.
        /// <param name="repeticiones"></param> repeticiones hechas. 
        /// <param name="duracion"></param> duracion del ejercicio.
        /// <param name="feedback"></param> feedback del paciente sobre el ejercicio.
        /// <returns>
        /// 1-> bien.
        /// 0 -> mal.
        /// </returns>
        public static int registrarEjercicio(string nombreUsuarioPaciente, string ejercicio, int repeticiones, string duracion, string feedback)
        {
            MySqlConnection conn;
            DateTime hoy = DateTime.Now;
            int resultado = 0;
            int error = 0;
            string fecha = hoy.ToString("yyyy/MM/dd HH:mm:ss tt");
            string[] nombrePaciente = Paciente.getPaciente(nombreUsuarioPaciente);
            string nombreUsuarioTerapeuta = Relacion.getTerapeuta(nombrePaciente[0], nombrePaciente[1]);

            int idEjercicio = getIdEjercicio(ejercicio);

            if (nombrePaciente == null || nombreUsuarioTerapeuta == null)
            {
                return error;
            }

            string nombreCompletoUsuario = nombrePaciente[0] + " " + nombrePaciente[1];

            try
            {
                conn = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return error;
            }
            try
            {
                MySqlCommand comando;
                if (feedback == "")
                    comando = new MySqlCommand(string.Format("Insert Into historial (idEjercicio,ejercicio,nombrePaciente,usuarioPaciente, usuarioTerapeuta,repeticiones,duracion,fecha,feedbackPaciente) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", idEjercicio, ejercicio, nombreCompletoUsuario, nombreUsuarioPaciente, nombreUsuarioTerapeuta, repeticiones, duracion, fecha, DBNull.Value), conn);
                else
                    comando = new MySqlCommand(string.Format("Insert Into historial (idEjercicio,ejercicio,nombrePaciente,usuarioPaciente, usuarioTerapeuta,repeticiones,duracion,fecha,feedbackPaciente) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", idEjercicio, ejercicio, nombreCompletoUsuario, nombreUsuarioPaciente, nombreUsuarioTerapeuta, repeticiones, duracion, fecha, feedback), conn);

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
        /// Metodo publico que obtiene el id del ejercicio pasado
        /// </summary>
        /// <param name="ejercicio">ejercicio elegido</param>
        /// <returns>id del ejercicio</returns>
        public static int getIdEjercicio(string ejercicio)
        {
            int id = -1;
            try
            {
                MySqlConnection con = BDComun.ObtnerConexion();
                MySqlCommand comando = new MySqlCommand();
                comando.Connection = con;
                comando.CommandType = CommandType.Text;
                comando.CommandText = string.Format("Select id from ejercicios where ejercicio = '" + ejercicio + "'");
                MySqlDataReader reader = comando.ExecuteReader();
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                }
                return id;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return id;
            }
        }

        /// <summary>
        /// Metodo que actualiza tanto la descripcion, como la imagen descriptiva de un ejercicio ya creado.
        /// </summary>
        /// <param name="ejercicio"></param> String que identifica al ejercicio que hay que modificar.
        /// <param name="descripcion"></param> String de la descripcion a modificar.
        /// <param name="pathImagen"></param> Ruta de la imagen que hay que guardar en la base de datos.
        /// <returns>
        /// 1-> bien.
        /// 0 -> mal.
        /// </returns>
        public static int modificarEjercicio(string ejercicio, string descripcion, string pathImagen)
        {
            byte[] imagen = null;
            if (pathImagen != null)
            {
                FileStream fstream = new FileStream(pathImagen, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fstream);
                imagen = br.ReadBytes((int)fstream.Length);
            }

            int resultado = 0;
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

            try
            {
                MySqlCommand comando;
                string query;
                if (pathImagen != null)
                {
                    query = "UPDATE ejercicios set descripcion='" + descripcion + "', imagenEjercicio = @IMG where ejercicio = '" + ejercicio + "'";
                    comando = new MySqlCommand(query, conn);
                    comando.Parameters.Add(new MySqlParameter("@IMG", imagen));
                }
                else
                {
                    query = "UPDATE ejercicios set descripcion='" + descripcion + "' where ejercicio = '" + ejercicio + "'";
                    comando = new MySqlCommand(query, conn);
                }

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
        /// Metodo publico que devuelve los datos de todos los ejercicios
        /// de la base de datos.
        /// </summary>
        /// <returns>
        /// MySqlDataReader de todos los ejercicios de la base de datos.
        /// </returns>
        public static MySqlDataReader getEjercicios()
        {
            MySqlConnection conexion=null;
            try
            {
                conexion = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la Base de datos: " + ex.ToString());
            }

            string query = "Select * from ejercicios";
            MySqlCommand comando = new MySqlCommand(query, conexion);
            MySqlDataReader dr = comando.ExecuteReader();
            return dr;
        }

        public static MySqlDataReader getEjercicio(string ejercicio)
        {
            MySqlConnection conexion = null;
            try
            {
                conexion = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la Base de datos: " + ex.ToString());
            }
            string query = "Select descripcion,imagenEjercicio from ejercicios where ejercicio = '" + ejercicio + "'";
            MySqlCommand comando = new MySqlCommand(query, conexion);
            MySqlDataReader dr = comando.ExecuteReader();
            return dr;
        }
    }
}