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
        /// <param name="ejercicio1"></param> ejercicio1 del entrenamiento.
        /// <param name="ejercicio2"></param> ejercicio2 del entrenamiento.
        /// <param name="ejercicio3"></param> ejercicio3 del entrenamiento.
        /// <param name="ejercicio4"></param> ejercicio4 del entrenamiento.
        /// <param name="ejercicio5"></param> ejercicio5 del entrenamiento.
        /// <param name="fechaEntrenamiento"></param> fecha de realizacion del entrenamiento.
        /// <param name="resultados"></param> resultados del entrenamiento.
        /// <param name="feedbackPaciente"></param> feedback por parte del paciente.
        /// <param name="feedbackTerapeuta"></param> feedback por parte del terapeuta.
        /// <returns></returns>
        public static int RegistrarEntrenamiento(string nombrePaciente, string nombreUsuarioPaciente, string nombreTerapeuta, string nombreUsuarioTerapeuta, string ejercicio1, string ejercicio2, string ejercicio3, string ejercicio4, string ejercicio5, object fechaEntrenamiento, object resultados, object feedbackPaciente, object feedbackTerapeuta)
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
                //SqlCommand comando = new SqlCommand(string.Format("Insert Into Entrenamientos (nombrePaciente, usuarioPaciente,nombreTerapeuta,usuarioTerapeuta,ejercicio1,ejercicio2,ejercicio3,ejercicio4,ejercicio5,fechaEntrenamiento,resultados,feedbackPaciente,feedbackTerapeuta) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}')",nombrePaciente,nombreUsuarioPaciente,nombreTerapeuta, nombreUsuarioTerapeuta, ejercicio1, ejercicio2, ejercicio3, ejercicio4, ejercicio5, fechaEntrenamiento, resultados, feedbackPaciente, feedbackTerapeuta), conn);
                string query = "Insert Into Entrenamientos (nombrePaciente, usuarioPaciente,nombreTerapeuta,usuarioTerapeuta,ejercicio1,ejercicio2,ejercicio3,ejercicio4,ejercicio5,fechaEntrenamiento,resultados,feedbackPaciente,feedbackTerapeuta)" + "values('" + nombrePaciente + "','" + nombreUsuarioPaciente + "','" + nombreTerapeuta + "','" + nombreUsuarioTerapeuta + "','" + ejercicio1 + "', @EJER2, @EJER3,@EJER4,@EJER5,@FECHA,@RESULTADOS,@FEEDBACKPACIENTE,@FEEDBACKTERAPEUTA);";
                SqlCommand comando = new SqlCommand(query, conn);
                if (ejercicio1 == "")
                    return error;

                if(ejercicio2 != "")
                    comando.Parameters.AddWithValue("@EJER2", ejercicio2);
                else
                    comando.Parameters.AddWithValue("@EJER2", DBNull.Value);

                if (ejercicio3 != "")
                    comando.Parameters.AddWithValue("@EJER3", ejercicio3);
                else
                    comando.Parameters.AddWithValue("@EJER3", DBNull.Value);

                if (ejercicio4 != "")
                    comando.Parameters.AddWithValue("@EJER4", ejercicio4);
                else
                    comando.Parameters.AddWithValue("@EJER4", DBNull.Value);

                if (ejercicio5 != "")
                    comando.Parameters.AddWithValue("@EJER5", ejercicio5);
                else
                    comando.Parameters.AddWithValue("@EJER5", DBNull.Value);

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
    }
}
