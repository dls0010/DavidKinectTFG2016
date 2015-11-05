using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DavidKinectTFG2016.clases
{
    class Usuario
    {
        /// <summary>
        /// Metodo que controla el crear un nuevo usuario en la base de datos.
        /// </summary>
        /// <param name="pUsuario"></param> Nombre de Usuario.
        /// <param name="pContraseña"></param> Contraseña del Usuario.
        /// <param name="pTipoUsuario"></param> Tipo de Usuario.
        /// <returns>
        /// 0: Ha ocurrido un fallo. No se ha llevado a cabo la inserción.
        /// != 0 Proceso realizado correctamente.
        /// </returns>
        public static int CrearUsuarios(string pUsuario, string pContraseña, string pTipoUsuario)
        {
            int resultado = 0;

            if (Existe(pUsuario) == false && pTipoUsuario.Length != 0)
            {
                SqlConnection conn = BDComun.ObtnerConexion();
                SqlCommand comando = new SqlCommand(string.Format("Insert Into Usuarios (usuario,contraseña,tipoUsuario) values ('{0}','{1}','{2}')", pUsuario, pContraseña, pTipoUsuario), conn);

                resultado = comando.ExecuteNonQuery();
                conn.Close();

            }
            return resultado;
        }

        /// <summary>
        /// Metodo que devuelve el tipo de usuario.
        /// </summary>
        /// <param name="pUsuario"></param> nombre de usuario.
        /// <returns>
        /// string: tipo de usuario.
        /// </returns>
        public static string obtenerTipo(String pUsuario)
        {
            string tipoUsuario = " ";
            SqlConnection conn = BDComun.ObtnerConexion();
            SqlCommand comando = new SqlCommand(string.Format("Select tipoUsuario from Usuarios where usuario like '{0}' ", pUsuario), conn);
            SqlDataReader reader = comando.ExecuteReader();
            while (reader.Read())
            {
                tipoUsuario = reader.GetString(0);
            }
            reader.Close();
            return tipoUsuario;

        }

        /// <summary>
        /// Metodo adicional usado para comprobacion de la existencia de ese usuario en la base de datos.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>True: Existe un usuario con ese nombre. 
        /// False: No existe un usuario con ese nombre.
        /// </returns>
        public static bool Existe(string usuario)
        {
            using (SqlConnection conexion = BDComun.ObtnerConexion())
            {
                string query = "SELECT COUNT(*) FROM Usuarios WHERE Usuario=@Usuario";
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("Usuario", usuario);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count == 0)
                    return false;
                else
                    return true;
            }
        }
    }
}
