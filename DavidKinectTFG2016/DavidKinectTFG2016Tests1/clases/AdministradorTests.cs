using Microsoft.VisualStudio.TestTools.UnitTesting;
using DavidKinectTFG2016.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DavidKinectTFG2016.clases.Tests
{
    /// <summary>
    /// Clase que contiene las pruebas de la clase Administrador.
    /// </summary>
    [TestClass()]
    public class AdministradorTests
    {
        /// <summary>
        /// Metodo de prueba que comprueba si un usuario de tipo Administrador se puede registrar.
        /// en la base de datos.
        /// </summary>
        [TestMethod()]
        public void registrarAdministradorTest()
        {
            MySqlConnection conn = null;
            List<String[]> lista = new List<string[]>();
            //Usuario que existe
            String[] uno = { "nombreAdmin", "apellidosAdmin", "usuarioAdmin", "nifAdmin","nacimientoAdmin" };
            String[] usuario = { "usuarioAdmin", "123", "Administrador" };
            //Usuario que no existe
            String[] dos = { "nombreAdmin2", "apellidosAdmin","nombreUsuario", "nifAdmin", "nacimientoAdmin" };
            lista.Add(uno);
            lista.Add(dos);
            foreach (String[] registro in lista)
            {
                try
                {
                    if (registro[0] == "nombreAdmin")
                    {
                        int resultadoAdmin = Usuario.CrearUsuarios(usuario[0], usuario[1], usuario[2]);
                    }
                    Boolean existe = Usuario.Existe(registro[2]);
                    int resultado = Administrador.registrarAdministrador(registro[0], registro[1], registro[2], registro[3], registro[4]);
                    if (resultado != 0 && existe)
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
                finally
                {
                    Usuario.BorrarUsuario(registro[2]);
                    conn = BDComun.ObtnerConexion();
                    using (MySqlCommand comandoDelete = new MySqlCommand(string.Format("Delete from administradores where usuario = '{0}'", registro[2]), conn))
                    {
                        comandoDelete.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
        }
    }
}