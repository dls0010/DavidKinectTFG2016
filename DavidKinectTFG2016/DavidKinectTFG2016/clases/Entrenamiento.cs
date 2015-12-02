using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DavidKinectTFG2016.clases
{
    /// <summary>
    /// Clase Entrenamiento que registra los entrenamientos en la base de datos.
    /// </summary>
    public class Entrenamiento
    {
        /// <summary>
        /// Metodo que registrar un entrenamiento asignado por un terapeuta a un paciente.
        /// </summary>
        /// <param name="nombrePaciente"></param> Nombre+Apellidos del paciente.
        /// <param name="nombreUsuarioPaciente"></param> usuario del paciente.
        /// <param name="nombreTerapeuta"></param> Nombre del terapeuta.
        /// <param name="nombreUsuarioTerapeuta"></param> usuario del terapeuta.
        /// <param name="ejercicio1"></param> ejercicio1 del entrenamiento
        /// <param name = "repeticiones1" ></ param > repeticiones del ejercicio1 del entrenamiento.
        /// <param name="ejercicio2"></param> ejercicio2 del entrenamiento.
        /// <param name = "repeticiones2" ></ param > repeticiones del ejercicio2 del entrenamiento.
        /// <param name="ejercicio3"></param> ejercicio3 del entrenamiento.
        /// <param name = "repeticiones3" ></ param > repeticiones del ejercicio3 del entrenamiento.
        /// <param name="ejercicio4"></param> ejercicio4 del entrenamiento.
        /// <param name = "repeticiones4" ></ param > repeticiones del ejercicio4 del entrenamiento
        /// <param name="ejercicio5"></param> ejercicio5 del entrenamiento.
        /// <param name = "repeticiones5" ></ param > repeticiones del ejercicio5 del entrenamiento
        /// <param name="fechaEntrenamiento"></param> fecha de realizacion del entrenamiento.
        /// <param name="resultados"></param> resultados del entrenamiento.
        /// <param name="feedbackPaciente"></param> feedback por parte del paciente.
        /// <param name="feedbackTerapeuta"></param> feedback por parte del terapeuta.
        /// <returns></returns>
        public static int RegistrarEntrenamiento(string nombrePaciente, string nombreUsuarioPaciente, string nombreTerapeuta, string nombreUsuarioTerapeuta, string ejercicio1, int repeticiones1, string ejercicio2, int repeticiones2, string ejercicio3, int repeticiones3, string ejercicio4, int repeticiones4, string ejercicio5, int repeticiones5, object fechaEntrenamiento, object resultados, object feedbackPaciente, object feedbackTerapeuta)
        {
            SqlConnection conn;
            int resultado = 0;
            int error = 0;
            try
            {
                conn = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                return error;
            }
            try
            {
                string query = "Insert Into Entrenamientos (nombrePaciente, usuarioPaciente,nombreTerapeuta,usuarioTerapeuta,ejercicio1,repeticiones1,ejercicio2,repeticiones2,ejercicio3,repeticiones3, ejercicio4,repeticiones4,ejercicio5,repeticiones5, fechaEntrenamiento,resultados,feedbackPaciente,feedbackTerapeuta)" + "values('" + nombrePaciente + "','" + nombreUsuarioPaciente + "','" + nombreTerapeuta + "','" + nombreUsuarioTerapeuta + "','" + ejercicio1 + "','" + repeticiones1 + "', @EJER2,@REP2, @EJER3,@REP3,@EJER4,@REP4,@EJER5,@REP5,@FECHA,@RESULTADOS,@FEEDBACKPACIENTE,@FEEDBACKTERAPEUTA);";
                SqlCommand comando = new SqlCommand(query, conn);
                if (ejercicio1 == "")
                    return error;

                if (ejercicio2 != "")
                {
                    comando.Parameters.AddWithValue("@EJER2", ejercicio2);
                    comando.Parameters.AddWithValue("@REP2", repeticiones2);
                }
                else
                {
                    comando.Parameters.AddWithValue("@EJER2", DBNull.Value);
                    comando.Parameters.AddWithValue("@REP2", DBNull.Value);
                }

                if (ejercicio3 != "")
                {
                    comando.Parameters.AddWithValue("@EJER3", ejercicio3);
                    comando.Parameters.AddWithValue("@REP3", repeticiones3);
                }
                {
                    comando.Parameters.AddWithValue("@EJER3", DBNull.Value);
                    comando.Parameters.AddWithValue("@REP3", DBNull.Value);
                }

                if (ejercicio4 != "")
                {
                    comando.Parameters.AddWithValue("@EJER4", ejercicio4);
                    comando.Parameters.AddWithValue("@REP4", repeticiones4);
                }
                else
                {
                    comando.Parameters.AddWithValue("@EJER4", DBNull.Value);
                    comando.Parameters.AddWithValue("@REP4", DBNull.Value);
                }

                if (ejercicio5 != "")
                {
                    comando.Parameters.AddWithValue("@EJER5", ejercicio5);
                    comando.Parameters.AddWithValue("@REP5", repeticiones5);
                }
                else
                {
                    comando.Parameters.AddWithValue("@EJER5", DBNull.Value);
                    comando.Parameters.AddWithValue("@REP5", DBNull.Value);
                }

                if (fechaEntrenamiento != null)
                    comando.Parameters.AddWithValue("@FECHA", fechaEntrenamiento);
                else
                    comando.Parameters.AddWithValue("@FECHA", DBNull.Value);

                if (resultados != null)
                    comando.Parameters.AddWithValue("@RESULTADOS", resultados);
                else
                    comando.Parameters.AddWithValue("@RESULTADOS", DBNull.Value);

                if (feedbackPaciente != null)
                    comando.Parameters.AddWithValue("@FEEDBACKPACIENTE", feedbackPaciente);
                else
                    comando.Parameters.AddWithValue("@FEEDBACKPACIENTE", DBNull.Value);

                if (feedbackTerapeuta != null)
                    comando.Parameters.AddWithValue("@FEEDBACKTERAPEUTA", feedbackTerapeuta);
                else
                    comando.Parameters.AddWithValue("@FEEDBACKTERAPEUTA", DBNull.Value);

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
        /// Metodo que actualiza el registro de entrenamiento del paciente.
        /// con el feedback del terapeuta.
        /// </summary>
        /// <param name="idEntrenamiento"></param> idEntrenamiento a actualizar.
        /// <param name="nombreUsuarioTerapeuta"></param> nombreUsuarioTerapeuta que actualiza.
        /// <param name="feedbackTerapeuta"></param> Comentario del terapeuta.
        /// <returns></returns>
        public static int AñadirFeedback(int idEntrenamiento, string nombreUsuarioTerapeuta, string feedbackTerapeuta)
        {
            SqlConnection conn;
            int resultado = 0;
            int error = 0;
            try
            {
                conn = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                return error;
            }
            try
            {
                string query = "Update Entrenamientos SET feedbackTerapeuta = '"+feedbackTerapeuta+"' WHERE usuarioTerapeuta = '"+nombreUsuarioTerapeuta+"' and idEntrenamiento = "+idEntrenamiento+"";
                //SqlCommand comando = new SqlCommand(string.Format("Update Entrenamientos SET feedbackTerapeutan = '{0}' WHERE usuarioTerapeuta = '{1}' and idEntrenamiento = '{2}' )", feedbackTerapeuta,nombreUsuarioTerapeuta,idEntrenamiento ), conn);
                SqlCommand comando = new SqlCommand(query,conn);
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
        /// Metodo que registra los resultados de un entrenamiento hecho por el paciente.
        /// </summary>
        /// <param name="idEntrenamiento"></param> ID del entrenamiento a actualizar.
        /// <param name="fecha"></param> Fecha de realización del entrenamiento.
        /// <param name="resumenResultados"></param> Resumen de los resultados del entrenamiento.
        /// <param name="feedbackPaciente"></param> Feedback aportado por el paciente (opcional).
        /// <returns></returns>
        public static int modificarEntrenamiento(int idEntrenamiento, string fecha, string resumenResultados, string feedbackPaciente)
        {
            SqlConnection conn;
            int resultado = 0;
            int error = 0;
            try
            {
                conn = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                return error;
            }
            try
            {
                string query;
                if (feedbackPaciente != null)
                    query = "Update Entrenamientos SET fechaEntrenamiento = '" + fecha + "' , resultados = '"+resumenResultados+"' , feedbackPaciente='"+feedbackPaciente+"' WHERE idEntrenamiento = " + idEntrenamiento+"";
                else
                    query = "Update Entrenamientos SET fechaEntrenamiento = '" + fecha + "' , resultados = '" + resumenResultados + "' WHERE idEntrenamiento = " + idEntrenamiento + "";
                SqlCommand comando = new SqlCommand(query, conn);
                resultado = comando.ExecuteNonQuery();
                conn.Close();
                return resultado;
            }
            catch (Exception ex)
            {
                return error;
            }
        }
    }
}
