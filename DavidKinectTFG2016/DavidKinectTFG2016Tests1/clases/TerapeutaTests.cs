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
    [TestClass()]
    public class TerapeutaTests
    {
        [TestMethod()]
        public void registrarTerapeutaTest()
        {
            MySqlConnection conn = null;
            List<String[]> lista = new List<string[]>();
            //Usuario que existe
            String[] uno = { "nombreTerapeuta1", "apellidosTerapeuta1", "usuarioTerapeuta", "nif1", "12-12-1945", "96547821", "C:\\Users\\David\\Documents\\GitHubVisualStudio\\TFG\\DavidKinectTFG2016\\DavidKinectTFG2016\\bin\\Debug\\miFoto.jpg" };
            String[] usuario = { "usuarioTerapeuta", "123", "Terapeuta" };
            //Usuario que no existe
            String[] dos = { "nombreTerapeuta2", "apellidosTerapeuta2", "usuarioTerapeuta2", "nif2", "12-12-1945", "96547821", "C:\\Users\\David\\Documents\\GitHubVisualStudio\\TFG\\DavidKinectTFG2016\\DavidKinectTFG2016\\bin\\Debug\\miFoto.jpg" };
            lista.Add(uno);
            lista.Add(dos);
            foreach (String[] registro in lista)
            {
                try
                {
                    if (registro[0] == "nombreTerapeuta1")
                    {
                        int resultadoPaciente = Usuario.CrearUsuarios(usuario[0], usuario[1], usuario[2]);
                    }
                    Boolean existe = Usuario.Existe(registro[2]);
                    int resultado = Terapeuta.registrarTerapeuta(registro[0], registro[1], registro[2], registro[3], registro[4], registro[5], registro[6]);
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
                    using (MySqlCommand comandoDelete = new MySqlCommand(string.Format("Delete from terapeutas where usuario = '{0}'", registro[2]), conn))
                    {
                        comandoDelete.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
        }

        [TestMethod()]
        public void getNombreTerapeutaTest()
        {
            MySqlConnection conn = null;
            List<String[]> lista = new List<string[]>();
            //Usuario que existe
            String[] uno = { "nombreTerapeuta1", "apellidosTerapeuta1", "usuarioTerapeuta", "nif1", "12-12-1945", "96547821", "C:\\Users\\David\\Documents\\GitHubVisualStudio\\TFG\\DavidKinectTFG2016\\DavidKinectTFG2016\\bin\\Debug\\miFoto.jpg" };
            String[] usuario = { "usuarioTerapeuta", "123", "Terapeuta" };
            //Usuario que no existe
            String[] dos = { "nombreTerapeuta2", "apellidosTerapeuta2", "usuarioTerapeuta2", "nif2", "12-12-1945", "96547821", "C:\\Users\\David\\Documents\\GitHubVisualStudio\\TFG\\DavidKinectTFG2016\\DavidKinectTFG2016\\bin\\Debug\\miFoto.jpg" };
            lista.Add(uno);
            lista.Add(dos);
            foreach (String[] registro in lista)
            {
                try
                {
                    if (registro[0] == "nombreTerapeuta1")
                    {
                        int resultadoPaciente = Usuario.CrearUsuarios(usuario[0], usuario[1], usuario[2]);
                    }
                    Boolean existe = Usuario.Existe(registro[2]);
                    int resultado = Terapeuta.registrarTerapeuta(registro[0], registro[1], registro[2], registro[3], registro[4], registro[5], registro[6]);
                    string nombreTerapeuta = Terapeuta.getNombreTerapeuta(registro[2]);
                    if (resultado != 0 && existe)
                    {
                        Assert.AreEqual(nombreTerapeuta, "nombreTerapeuta1");
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
                    using (MySqlCommand comandoDelete = new MySqlCommand(string.Format("Delete from terapeutas where usuario = '{0}'", registro[2]), conn))
                    {
                        comandoDelete.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
        }

        [TestMethod()]
        public void getNombreCompletoTerapeutaTest()
        {
            MySqlConnection conn = null;
            List<String[]> lista = new List<string[]>();
            //Usuario que existe
            String[] uno = { "nombreTerapeuta1", "apellidosTerapeuta1", "usuarioTerapeuta", "nif1", "12-12-1945", "96547821", "C:\\Users\\David\\Documents\\GitHubVisualStudio\\TFG\\DavidKinectTFG2016\\DavidKinectTFG2016\\bin\\Debug\\miFoto.jpg" };
            String[] usuario = { "usuarioTerapeuta", "123", "Terapeuta" };
            //Usuario que no existe
            String[] dos = { "nombreTerapeuta2", "apellidosTerapeuta2", "usuarioTerapeuta2", "nif2", "12-12-1945", "96547821", "C:\\Users\\David\\Documents\\GitHubVisualStudio\\TFG\\DavidKinectTFG2016\\DavidKinectTFG2016\\bin\\Debug\\miFoto.jpg" };
            lista.Add(uno);
            lista.Add(dos);
            foreach (String[] registro in lista)
            {
                try
                {
                    if (registro[0] == "nombreTerapeuta1")
                    {
                        int resultadoPaciente = Usuario.CrearUsuarios(usuario[0], usuario[1], usuario[2]);
                    }
                    Boolean existe = Usuario.Existe(registro[2]);
                    int resultado = Terapeuta.registrarTerapeuta(registro[0], registro[1], registro[2], registro[3], registro[4], registro[5], registro[6]);
                    string nombreTerapeutaCompleto = Terapeuta.getNombreCompletoTerapeuta(registro[2]);
                    if (resultado != 0 && existe)
                    {
                        Assert.AreEqual(nombreTerapeutaCompleto, "nombreTerapeuta1 apellidosTerapeuta1");
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
                    using (MySqlCommand comandoDelete = new MySqlCommand(string.Format("Delete from terapeutas where usuario = '{0}'", registro[2]), conn))
                    {
                        comandoDelete.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
        }
    }
}