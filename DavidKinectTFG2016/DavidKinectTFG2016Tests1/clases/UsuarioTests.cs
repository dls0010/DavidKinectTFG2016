using Microsoft.VisualStudio.TestTools.UnitTesting;
using DavidKinectTFG2016.clases;
using DavidKinectTFG2016;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace DavidKinectTFG2016.clases.Tests
{
    /// <summary>
    /// Clase que contiene las pruebas de la clase Usuario.
    /// </summary>
    [TestClass()]
    public class UsuarioTests
    {
        /// <summary>
        /// Metodo de prueba que comprueba si se da de alta a un usuario en la base de datos.
        /// </summary>
        [TestMethod()]
        public void CrearUsuariosTest()
        {
            MySqlConnection conn = null;
            List<String[]> lista = new List<string[]>();
            String[] uno = { "usuarioPrueba", "123", "Paciente" };
            String[] dos = { "usuarioPrueba2", "123", "Terapeuta" };
            String[] tres = { "paciente1", "123", "Paciente" };
            lista.Add(uno);
            lista.Add(dos);
            lista.Add(tres);

            foreach (String[] registro in lista)
            {
                try
                {
                    if (Usuario.CrearUsuarios(registro[0], registro[1], registro[2]) > 0)
                    {
                        conn = BDComun.ObtnerConexion();
                        int contador = 0;
                        MySqlCommand comandoSelect = new MySqlCommand(string.Format("Select * from usuarios where usuario = '{0}' and contraseña= password('{1}')", registro[0], registro[1]), conn);
                        MySqlDataReader readerSelect = comandoSelect.ExecuteReader();

                        while (readerSelect.Read())
                        {
                            contador++;
                        }
                        conn.Close();
                        conn = BDComun.ObtnerConexion();
                        using (MySqlCommand comandoDelete = new MySqlCommand(string.Format("Delete from usuarios where usuario = '{0}'", registro[0]), conn))
                        {
                            comandoDelete.ExecuteNonQuery();
                        }
                        Assert.AreEqual(contador, 1);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Metodo de prueba que comprueba si se autentifica correctamente un usuario.
        /// en la base de datos para acceder a la aplicación.
        /// </summary>
        [TestMethod()]
        public void AutentificarTest()
        {
            List<String[]> lista = new List<string[]>();
            //Usuario que existe
            String[] uno = { "paciente1", "123", "Paciente" };
            //Usuario que no existe
            String[] dos = { "nousuario123", "123", "Terapeuta" };
            lista.Add(uno);
            lista.Add(dos);
            foreach (String[] registro in lista)
            {
                try
                {
                    int resultado = Usuario.Autentificar(registro[0], registro[1]);
                    if (resultado> 0)
                    {
                        Assert.AreEqual(resultado, 1);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Metodo que comprueba si se puede obtener correctamente el tipo de usuario.
        /// al que pertenece un usuario registrado en la base de datos.
        /// </summary>
        [TestMethod()]
        public void obtenerTipoTest()
        {
            List<String[]> lista = new List<string[]>();
            //Usuario que existe
            String[] uno = { "paciente1", "123", "Paciente" };
            //Usuario que no existe
            String[] dos = { "nousuario123", "123", "Terapeuta" };
            lista.Add(uno);
            lista.Add(dos);
            foreach (String[] registro in lista)
            {
                try
                {
                    string tipo = Usuario.obtenerTipo(registro[0]);
                    if (tipo != null)
                    {
                        Assert.AreEqual(tipo, "Paciente");
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Metodo que comprueba si esta registrado un usuario en la base de datos.
        /// </summary>
        [TestMethod()]
        public void ExisteTest()
        {
            List<String[]> lista = new List<string[]>();
            //Usuario que existe
            String[] uno = { "paciente1", "123", "Paciente" };
            //Usuario que no existe
            String[] dos = { "nousuario123", "123", "Terapeuta" };
            lista.Add(uno);
            lista.Add(dos);
            foreach (String[] registro in lista)
            {
                try
                {
                    Boolean resultado = Usuario.Existe(registro[0]);
                    if (resultado)
                    {
                        Assert.AreEqual(resultado, true);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        /// <summary>
        /// Metodo que comprueba si funciona correctamente el borrar un usuario de la base de datos.
        /// </summary>
        [TestMethod()]
        public void BorrarUsuarioTest()
        {
            List<String[]> lista = new List<string[]>();
            //Usuario que se registrará y se borrará
            String[] uno = { "usuarioBorrar", "123", "Paciente" };
            //Usuario que no se registrará y se pedirá borrar
            String[] dos = { "nousuario123", "123", "Terapeuta" };
            lista.Add(uno);
            lista.Add(dos);
            foreach (String[] registro in lista)
            {
                try
                {
                    if (registro[0] == "usuarioBorrar")
                    {
                        Usuario.CrearUsuarios(registro[0], registro[1], registro[2]);
                    }

                    int resultado = Usuario.BorrarUsuario(registro[0]);
                    if (resultado > 0)
                    {
                        Assert.AreEqual(resultado, 1);
                    }
                    else
                    {
                        Assert.Fail();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
    }
}