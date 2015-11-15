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
        public static int registrarEjercicio(string nombreUsuarioPaciente, string ejercicio, int repeticiones, string duracion)
        {
            DateTime hoy = DateTime.Today;
            int resultado = 0;
            string fecha = hoy.ToString("yyyy/MM/dd");
            string[] nombrePaciente = Paciente.getPaciente(nombreUsuarioPaciente);
            string nombreUsuarioTerapeuta = Relacion.getTerapeuta(nombrePaciente[0], nombrePaciente[1]);

            string nombreCompletoUsuario = nombrePaciente[0] + " " + nombrePaciente[1];

            SqlConnection conn = BDComun.ObtnerConexion();
            SqlCommand comando = new SqlCommand(string.Format("Insert Into Historial (ejercicio,nombrePaciente,usuarioPaciente, usuarioTerapeuta,repeticiones,duracion,fecha) values ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", ejercicio, nombreCompletoUsuario, nombreUsuarioPaciente,nombreUsuarioTerapeuta,repeticiones,duracion,fecha), conn);

            resultado = comando.ExecuteNonQuery();
            conn.Close();

            return resultado;
        }

    }
}