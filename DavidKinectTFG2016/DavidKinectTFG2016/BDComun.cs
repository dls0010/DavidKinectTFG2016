using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

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
        public static SqlConnection ObtnerConexion()
        {
            SqlConnection conn = new SqlConnection(@"Data source=DAVID-PORTATIL\DAVID; Initial Catalog = KinectTFGBD; Integrated Security=True");
            conn.Open();

            return conn;
        }
    }
}
