﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;

namespace DavidKinectTFG2016.clases
{
    /// <summary>
    /// Clase Administrador que registrar en la base de datos.
    /// </summary>
    public class Administrador
    {
        /// <summary>
        /// Metodo que controla el registro un nuevo administrador en la base de datos.
        /// </summary>
        /// <param name="pNombre"></param> Nombre del administrador.
        /// <param name="pApellidos"></param> Apellidos del administrador.
        /// <param name="pNombreUsuario"></param> Nombre Usuario del administrador 
        /// <param name="pNIF"></param> NIF del administrador.
        /// <param name="pNacimiento"></param> Nacimiento del administrador.
        /// <param name="pEstado"></param> Estado del administrador.
        /// <returns>
        /// 0: Ha ocurrido un fallo. No se ha llevado a cabo la inserción.
        /// != 0 Proceso realizado correctamente.
        /// </returns>
        public static int registrarAdministrador(string pNombre, string pApellidos, string pNombreUsuario, string pNIF, string pNacimiento)
        {
            int resultado = 0;
            int error = 0;
            MySqlConnection conn;
            try
            {
                conn = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return error;
            }
            try
            {
                MySqlCommand comando = new MySqlCommand(string.Format("Insert Into administradores (nombreAdministrador,apellidosAdministrador,usuario,nifAdministrador,nacimientoAdministrador) values ('{0}','{1}','{2}','{3}','{4}')", pNombre, pApellidos, pNombreUsuario, pNIF, pNacimiento), conn);

                resultado = comando.ExecuteNonQuery();
                conn.Close();
                return resultado;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return error;
            }
        }
    }
}