using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;

namespace DavidKinectTFG2016
{
    /// <summary>
    /// Clase usada para iniciar la conexión con la Base de datos KinectBD.
    /// </summary>
    class BDComun
    {
        /// <summary>
        /// Metodo para obtener la conexion con la base de datos.
        /// </summary>
        /// <returns>
        /// conn: Conexion con la base de datos.
        /// </returns>
        public static MySqlConnection ObtnerConexion()
        {
            MySqlConnection conn = null;
            if (accesoInternet())
            {
                try
                {
                    conn = new MySqlConnection(@"server=db4free.net; uid=davidlopez; pwd=123456; database=kinectbd; port=3306");

                    conn.Open();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex);
                }
            }
            
            return conn;
        }

        /// <summary>
        /// Metodo que verifica si el ordenador esta conectado a Internet
        /// Ya que la BD se encuentra alojada en un servidor web.
        /// Y se necesita acceso a internet para poder acceder a los datos de la BD.
        /// </summary>
        /// <returns>
        /// true: Conexion correcta
        /// false: conexion incorrecta
        /// </returns>
        private static bool accesoInternet()
        {
            try
            {
                System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry("www.google.com");
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
