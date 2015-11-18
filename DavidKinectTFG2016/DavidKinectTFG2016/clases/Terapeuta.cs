using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Media;
using System.IO;

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
        /// <param name="pTelefono"></param> Telefono del Terapeuta.
        /// <param name="pathImagen"></param> Imagen del Terapeuta.
        /// <returns>
        /// 0: Ha ocurrido un fallo. No se ha llevado a cabo la inserción.
        /// != 0 Proceso realizado correctamente.
        /// </returns>
        public static int registrarTerapeuta(string pNombre, string pApellidos, string pNombreUsuario, string pNIF, string pNacimiento, string pTelefono, string pathImagen)
        {
            int resultado = 1;
            int error = 0;
            SqlConnection conn;
            byte[] imagen = null;

            if (pathImagen != null)
            {
                FileStream fstream = new FileStream(pathImagen, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fstream);
                imagen = br.ReadBytes((int)fstream.Length);
            }

            try
            {
                conn = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                return error;
            }

            string query = "Insert Into Terapeutas (nombreTerapeuta,apellidosTerapeuta,usuario,nifTerapeuta,nacimientoTerapeuta,telefonoTerapeuta,imagenTerapeuta)" + "values('" + pNombre + "','" + pApellidos + "','" + pNombreUsuario + "','" + pNIF + "','" + pNacimiento + "','" + pTelefono + "',@IMG);";
            SqlCommand comando = new SqlCommand(query, conn);
            SqlDataReader reader;
            try
            {
                if (pathImagen != null)
                    comando.Parameters.Add(new SqlParameter("@IMG", imagen));

                reader = comando.ExecuteReader();
                
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
