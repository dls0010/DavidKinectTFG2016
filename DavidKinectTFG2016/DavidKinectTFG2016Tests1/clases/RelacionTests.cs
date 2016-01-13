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
    public class RelacionTests
    {
        [TestMethod()]
        public void registrarRelacionTest()
        {
            MySqlConnection conn = null;
            List<String[]> lista = new List<string[]>();
            //Usuario que existe
            String[] uno = { "nombrePaciente1", "apellidosPaciente1", "usuarioPaciente", "nif1", "96547821", "12-12-1945", "Libre", "descripcion1", "C:\\Users\\David\\Documents\\GitHubVisualStudio\\TFG\\DavidKinectTFG2016\\DavidKinectTFG2016\\bin\\Debug\\miFoto.jpg" };
            String[] usuario = { "usuarioPaciente", "123", "Paciente" };
            //Usuario que no existe
            String[] dos = { "nombrePaciente2", "apellidosPaciente2", "usuarioPaciente2", "nif2", "94123547", "12-12-1945", "Libre", "descripcion1", "C:\\Users\\David\\Documents\\GitHubVisualStudio\\TFG\\DavidKinectTFG2016\\DavidKinectTFG2016\\bin\\Debug\\miFoto.jpg" };
            lista.Add(uno);
            lista.Add(dos);
            foreach (String[] registro in lista)
            {
                try
                {
                    if (registro[0] == "nombrePaciente1")
                    {
                        int resultadoPaciente = Usuario.CrearUsuarios(usuario[0], usuario[1], usuario[2]);
                    }
                    Boolean existe = Usuario.Existe(registro[2]);
                    int resultado = Paciente.RegistrarPaciente(registro[0], registro[1], registro[2], registro[3], registro[4], registro[5], registro[6], registro[7], registro[8]);
                    int id = Relacion.obtenerIdPaciente(registro[2]);
                    int resultado1 = Relacion.registrarRelacion(Convert.ToString(id), "terapeuta1", registro[0], registro[1], "12-12-1900");
                    if (resultado != 0 && existe && resultado1 != 0)
                    {
                        Assert.AreEqual(resultado1, 1);
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
                    using (MySqlCommand comandoDelete = new MySqlCommand(string.Format("Delete from relaciones where fechaInicio = '{0}'", "12-12-1900"), conn))
                    {
                        comandoDelete.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
        }
    }
}