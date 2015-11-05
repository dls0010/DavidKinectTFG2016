using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DavidKinectTFG2016.clases
{
    /// <summary>
    /// Clase Terapeuta que registrar en la base de datos.
    /// </summary>
    class Terapeuta
    {
        /// <summary>
        /// Metodo que permite registrar al Terapeuta en la base de datos
        /// </summary>
        /// <param name="pNombre"></param> Nombre del Terapeuta
        /// <param name="pApellidos"></param> Apellidos del Terapeuta
        /// <param name="pNombreUsuario"></param> Nombre Usuario del Terapeuta.
        /// <param name="pNIF"></param> Nif del Terapeuta.
        /// <param name="pNacimiento"></param> Nacimiento del Terapeuta.
        /// <returns>
        /// 0: Ha ocurrido un fallo. No se ha llevado a cabo la inserción.
        /// != 0 Proceso realizado correctamente.
        /// </returns>
        public static int registrarTerapeuta(string pNombre, string pApellidos, string pNombreUsuario, string pNIF, string pNacimiento)
        {
            int resultado = 0;

            SqlConnection conn = BDComun.ObtnerConexion();
            SqlCommand comando = new SqlCommand(string.Format("Insert Into Terapeutas (nombreTerapeuta,apellidosTerapeuta,usuario,nifTerapeuta,nacimientoTerapeuta) values ('{0}','{1}','{2}','{3}','{4}')", pNombre, pApellidos, pNombreUsuario, pNIF, pNacimiento), conn);

            resultado = comando.ExecuteNonQuery();
            conn.Close();

            return resultado;
        }
    }
}
