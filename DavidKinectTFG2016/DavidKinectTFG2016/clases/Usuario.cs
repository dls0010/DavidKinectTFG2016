﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DavidKinectTFG2016.clases
{
    public class Usuario
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
            int error = 0;
            try
            {
                if (Existe(pUsuario) == false && pTipoUsuario.Length != 0)
                {
                    MySqlConnection conn = BDComun.ObtnerConexion();
                    MySqlCommand comando = new MySqlCommand(string.Format("Insert Into usuarios (Usuario,Contraseña,TipoUsuario) values ('{0}',PASSWORD('{1}'),'{2}')", pUsuario, pContraseña, pTipoUsuario), conn);

                    resultado = comando.ExecuteNonQuery();
                    conn.Close();

                }
                return resultado;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return error;
            }
        }

        /// <summary>
        /// Metodo que controla la autenficacion en el login del usuario.
        /// </summary>
        /// <param name="pUsuario"></param> Nombre del Usuario.
        /// <param name="pContraseña"></param> Contraseña del usuario.
        /// <returns>
        /// 0: error en la conexión.
        /// -1: No hay usuario en esa base de datos.
        /// != -1: Usuario encontrado. Autentificacion correcta.
        /// </returns>
        public static int Autentificar(String pUsuario, String pContraseña)
        {
            int resultado = -1;
            int error = -1;
            MySqlConnection conn;
            try
            {
                conn = BDComun.ObtnerConexion();
                if (conn == null)
                {
                    error = 0;
                    return error;
                }
                MySqlCommand comando = new MySqlCommand(string.Format("Select * from usuarios where usuario = '{0}' and contraseña= password('{1}')", pUsuario, pContraseña), conn);

                MySqlDataReader reader = comando.ExecuteReader();


                while (reader.Read())
                {
                    resultado = 1;
                }
                conn.Close();
                return resultado;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return error;
            }
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
            try
            {
                string tipoUsuario = " ";
                MySqlConnection conn = BDComun.ObtnerConexion();
                MySqlCommand comando = new MySqlCommand(string.Format("Select tipoUsuario from usuarios where usuario like '{0}' ", pUsuario), conn);
                MySqlDataReader reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    tipoUsuario = reader.GetString(0);
                }
                reader.Close();
                return tipoUsuario;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return null;
            }

        }

        /// <summary>
        /// Metodo adicional usado para comprobacion de la existencia de ese usuario en la base de datos.
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns>
        /// True: Existe un usuario con ese nombre. 
        /// False: No existe un usuario con ese nombre.
        /// </returns>
        public static bool Existe(string usuario)
        {
            try
            {
                using (MySqlConnection conexion = BDComun.ObtnerConexion())
                {
                    string query = "SELECT COUNT(*) FROM usuarios WHERE Usuario=@Usuario";
                    MySqlCommand cmd = new MySqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("Usuario", usuario);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                        return false;
                    else
                        return true;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Metodo que borra el usuario, si al registrarse se arrepiente y da a cancelar.
        /// </summary>
        /// <param name="nombreUsuario"></param>
        /// <returns>
        /// 1-> bien.
        /// 0 -> mal.
        /// </returns>
        public static int BorrarUsuario(string nombreUsuario)
        {
            int resultado = 1;
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

            string query = "DELETE usuarios from usuarios where usuario='" + nombreUsuario + "'";
            MySqlCommand comando = new MySqlCommand(query, conn);

            try
            {
                comando.ExecuteNonQuery();
                return resultado;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
                return error;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
