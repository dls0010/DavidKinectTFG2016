using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DavidKinectTFG2016.clases
{
    /// <summary>
    /// Clase Ejercicio que registra los ejercicios en la base de datos.
    /// </summary>
    class Ejercicio
    {
        /// <summary>
        /// Metodo que controla el registro una nueva actividad del paciente en la base de datos.
        /// </summary>
        /// <param name="nombreUsuarioPaciente"></param> Nombre de usuario del paciente
        /// <param name="ejercicio"></param> ejercicio realizado
        /// <param name="repeticiones"></param> repeticiones hechas 
        /// <param name="duracion"></param> duracion del ejercicio
        /// <returns>
        /// 1-> bien
        /// 0 -> mal
        /// </returns>
        public static int registrarEjercicio(string nombreUsuarioPaciente, string ejercicio, int repeticiones, string duracion)
        {
            SqlConnection conn;
            DateTime hoy = DateTime.Today;
            int resultado = 0;
            int error = 0;
            string fecha = hoy.ToString("yyyy/MM/dd");
            string[] nombrePaciente = Paciente.getPaciente(nombreUsuarioPaciente);
            string nombreUsuarioTerapeuta = Relacion.getTerapeuta(nombrePaciente[0], nombrePaciente[1]);
            if (nombrePaciente==null || nombreUsuarioTerapeuta== null)
            {
                return error;
            }

            string nombreCompletoUsuario = nombrePaciente[0] + " " + nombrePaciente[1];

            try {
               conn = BDComun.ObtnerConexion();
            }
            catch(Exception ex)
            {
                return error;
            }
            try {
                SqlCommand comando = new SqlCommand(string.Format("Insert Into Historial (ejercicio,nombrePaciente,usuarioPaciente, usuarioTerapeuta,repeticiones,duracion,fecha) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", ejercicio, nombreCompletoUsuario, nombreUsuarioPaciente, nombreUsuarioTerapeuta, repeticiones, duracion, fecha), conn);

                resultado = comando.ExecuteNonQuery();
                conn.Close();
                return resultado;
            }
            catch(Exception ex)
            {
                return error;
            }
        }

    }
}