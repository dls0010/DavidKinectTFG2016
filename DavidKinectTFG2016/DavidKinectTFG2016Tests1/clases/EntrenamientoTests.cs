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
    public class EntrenamientoTests
    {
        [TestMethod()]
        public void RegistrarEntrenamientoTest()
        {
            MySqlConnection conn = null;
            List<String[]> lista = new List<string[]>();
            //entrenamiento correcto
            String[] uno = {"nombrePaciente", "usuarioPaciente", "nombreTerapeuta","usuarioTerapeuta", "Ejercicio1", "20", "Ejercicio2","20", "Ejercicio3", "20", "Ejercicio4", "20", "Ejercicio5", "20", null,null,null,null};
            //entrenamiento incorrecto
            String[] dos = { "nombrePaciente", "usuarioPaciente", "nombreTerapeuta", "usuarioTerapeuta", "Ejercicio1", "20", "Ejercicio4", "20", "Ejercicio2", "20", "Ejercicio3", "20", "Ejercicio2", "20", null, null, null, null };
            lista.Add(uno);
            lista.Add(dos);
            foreach (String[] registro in lista)
            {
                try
                {
                    int resultado = Entrenamiento.RegistrarEntrenamiento(registro[0], registro[1], registro[2], registro[3], registro[4], Convert.ToInt32(registro[5]), registro[6], Convert.ToInt32(registro[7]), registro[8], Convert.ToInt32(registro[9]), registro[10], Convert.ToInt32(registro[11]), registro[12], Convert.ToInt32(registro[13]), registro[14], registro[15], registro[16], registro[17]);
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
                    using (MySqlCommand comandoDelete = new MySqlCommand(string.Format("Delete from entrenamientos where usuarioPaciente = '{0}'", registro[1]), conn))
                    {
                        comandoDelete.ExecuteNonQuery();
                    }
                    conn.Close();
                }
            }
        }
    }
}