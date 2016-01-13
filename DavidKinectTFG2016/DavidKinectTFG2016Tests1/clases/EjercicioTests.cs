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
    public class EjercicioTests
    { 
        [TestMethod()]
        public void registrarEjercicioTest()
        {
            MySqlConnection conn = null;
            List<String[]> lista = new List<string[]>();
            //Ejercicio correcto
            String[] uno = { "usuarioPaciente", "Ejercicio 1", "20", "30", "feedback" };
            //ejercico incorrecto
            String[] dos = { "usuarioPaciente2", "Ejercicio 1", "20", "30",null};
            lista.Add(uno);
            lista.Add(dos);
            foreach (String[] registro in lista)
            {
                try
                {
                    int resultado = Ejercicio.registrarEjercicio(registro[0], registro[1], Convert.ToInt32(registro[2]), registro[3], registro[4]);
                    if (resultado != 0)
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
                    conn = BDComun.ObtnerConexion();
                    using (MySqlCommand comandoDelete = new MySqlCommand(string.Format("Delete from historial where usuarioPaciente = '{0}'", registro[0]), conn))
                    {
                        comandoDelete.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
        }
    }
}