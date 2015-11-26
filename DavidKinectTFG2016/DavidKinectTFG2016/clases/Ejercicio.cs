using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;

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

        /// <summary>
        /// Metodo que actualiza tanto la descripcion, como la imagen descriptiva de un ejercicio ya creado.
        /// </summary>
        /// <param name="ejercicio"></param> String que identifica al ejercicio que hay que modificar.
        /// <param name="descripcion"></param> String de la descripcion a modificar.
        /// <param name="pathImagen"></param> Ruta de la imagen que hay que guardar en la base de datos.
        /// <returns></returns>
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
            SqlConnection conn;

            try
            {
                conn = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                return error;
            }

            string query = "UPDATE ejercicios set descripcion='"+descripcion+"', imagenEjercicio = @IMG where ejercicio = '"+ejercicio+"'";

            SqlCommand comando = new SqlCommand(query, conn);
            //SqlDataReader reader;
            try
            {
                if (pathImagen != null)
                    comando.Parameters.Add(new SqlParameter("@IMG", imagen));

                //reader = comando.ExecuteReader();
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