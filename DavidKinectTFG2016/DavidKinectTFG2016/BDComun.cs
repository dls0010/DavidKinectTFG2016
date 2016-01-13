using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows;
using System.IO;

namespace DavidKinectTFG2016
{
    /// <summary>
    /// Clase usada para iniciar la conexión con la Base de datos KinectBD.
    /// </summary>
    public class BDComun
    {
        /// <summary>
        /// Metodo para obtener la conexion con la base de datos.
        /// </summary>
        /// <returns>
        /// conn: Conexion con la base de datos.
        /// </returns>
        public static MySqlConnection ObtnerConexion()
        {
            string direccion = leerDireccionBD();
            MySqlConnection conn = null;
            if (accesoInternet())
            {
                try
                {
                    conn = new MySqlConnection(@direccion);

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
        /// Metodo privado que obtiene la cadena de texto.
        /// que permite la conexion con la base de datos remota.
        /// </summary>
        /// <returns>
        /// string: direccion de la base de datos.
        /// </returns>
        private static string leerDireccionBD()
        {
            string linea="";
            try
            {
                using (StreamReader sr = new StreamReader("direccionBD.txt", false))
                {
                    linea = sr.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("El archivo no se puede leer");
                MessageBox.Show("No se ha podido leer la direccion de la base de datos");
            }
            return linea;
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
