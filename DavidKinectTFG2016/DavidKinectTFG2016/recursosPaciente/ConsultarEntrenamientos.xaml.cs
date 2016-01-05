﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DavidKinectTFG2016.clases;

namespace DavidKinectTFG2016.recursosPaciente
{
    /// <summary>
    /// Lógica de interacción para ConsultarEntrenamientos.xaml
    /// </summary>
    public partial class ConsultarEntrenamientos : Window
    {
        string nombreUsuarioPaciente;
        SqlConnection conexion;
        string nombrePaciente;
        string nombreTerapeuta;
        string usuarioTerapeuta;
        string ejercicio1;
        string ejercicio2=null;
        string ejercicio3=null;
        string ejercicio4=null;
        string ejercicio5=null;
        List<int> listaRepeticiones;
        List<string> listaEjercicios;
    
        public ConsultarEntrenamientos(string usuario)
        {
            nombreUsuarioPaciente = usuario;
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que se ejecuta al abrirse la ventana, carga los entrenamientos.
        /// asignados al paciente en el datagrid.
        /// </summary>
        /// <param name="sender"></param> Ventana.
        /// <param name="e"></param> Eventos de la ventana.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion = BDComun.ObtnerConexion();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al conectar con la base de datos");
            }
            try
            {
                string query = "Select * from entrenamientos where usuarioPaciente = '" + nombreUsuarioPaciente + "'";
                SqlCommand comando = new SqlCommand(query, conexion);
                comando.ExecuteNonQuery();

                SqlDataAdapter adaptador = new SqlDataAdapter(comando);
                DataTable dt = new DataTable("entrenamientos");
                adaptador.Fill(dt);
                dataGridEntrenamientos.ItemsSource = dt.DefaultView;
                adaptador.Update(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener los datos en la tabla");
            }
            llenarComboBox();
        }

        /// <summary>
        /// Metodo que cierra la conexion con la BD al cerrar la ventana.
        /// </summary>
        /// <param name="sender"></param> Cerrar ventana.
        /// <param name="e"></param> Evento de cerrar.
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar la conexion con la base de datos");
            }
        }

        /// <summary>
        /// Metodo que se produce al pulsar el boton cancelar:
        /// cerrar la ventana.
        /// cerrar conexion con la base de datos.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar la conexion con la base de datos");
            }
        }

        /// <summary>
        /// Metodo adicional para llenar el combobox con los ejercicios.
        /// </summary>
        private void llenarComboBox()
        {
            try
            {
                string query = "Select idEntrenamiento from entrenamientos where fechaEntrenamiento IS NULL and usuarioPaciente='"+nombreUsuarioPaciente+"'";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    int idEntrenamiento = dr.GetInt32(0);
                    comboBoxIDEntrenamiento.Items.Add(idEntrenamiento);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos de los entrenamientos en el combobox");
            }
        }

        /// <summary>
        /// Metodo que escoge del combobox el entrenamiento seleccionado por el paciente.
        /// </summary>
        /// <param name="sender"></param> ComboboxIDEntrenamiento 
        /// <param name="e"></param> Evento del combobox
        private void comboBoxIDEntrenamiento_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                string query = "Select * from entrenamientos where idEntrenamiento =" + comboBoxIDEntrenamiento.Text + "";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    listaEjercicios = new List<string>();
                    listaRepeticiones = new List<int>();
                    nombrePaciente = dr.GetString(1);
                    nombreTerapeuta  = dr.GetString(3);
                    usuarioTerapeuta = dr.GetString(4);
                    ejercicio1 = dr.GetString(6);
                    listaEjercicios.Add(ejercicio1);
                    listaRepeticiones.Add(dr.GetInt32(7));
                    if (dr["ejercicio2"] != DBNull.Value)
                    {
                        ejercicio2 = dr.GetString(9);
                        listaRepeticiones.Add(dr.GetInt32(10));
                        listaEjercicios.Add(ejercicio2);
                    }
                    if (dr["ejercicio3"] != DBNull.Value)
                    {
                        ejercicio3 = dr.GetString(12);
                        listaRepeticiones.Add(dr.GetInt32(13));
                        listaEjercicios.Add(ejercicio3);
                    }
                    if (dr["ejercicio4"] != DBNull.Value)
                    {
                        ejercicio4 = dr.GetString(15);
                        listaRepeticiones.Add(dr.GetInt32(16));
                        listaEjercicios.Add(ejercicio4);
                    }
                    if (dr["ejercicio5"] != DBNull.Value)
                    {
                        ejercicio5 = dr.GetString(18);
                        listaRepeticiones.Add(dr.GetInt32(19));
                        listaEjercicios.Add(ejercicio5);
                    }
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al seleccionar el entrenamiento");
            }
        }

        /// <summary>
        /// Metodo que lanza la ejecución de un entrenamiento.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEmpezar_Click(object sender, RoutedEventArgs e)
        {
            string resumenResultados= "Resumen resultados: \n";
            string feedbackPaciente;
            List<string> listaDescripciones = new List<string>();
            try
            {
                string query = "Select descripcion from ejercicios";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    string descripcion = dr.GetString(0);
                    listaDescripciones.Add(descripcion);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos de los entrenamientos en el combobox");
            }

            foreach (string ejercicio in listaEjercicios)
            {
                string nuevo = "NUEVO EJERCICIO: \n";
                if(ejercicio == "Ejercicio1")
                {
                    Ejercicio1 ejer1 = new Ejercicio1(nombreUsuarioPaciente,listaRepeticiones[listaEjercicios.IndexOf(ejercicio)]);
                    MessageBox.Show(nuevo + listaDescripciones[0] + "\n LAS REPETICIONES SON: " + listaRepeticiones[listaEjercicios.IndexOf(ejercicio)].ToString());
                    ejer1.ShowDialog();
                    resumenResultados += "\n Ejercicio 1: \n" + ejer1.devolverResumen();
                }
                if (ejercicio == "Ejercicio2")
                {
                    Ejercicio2 ejer2 = new Ejercicio2(nombreUsuarioPaciente, listaRepeticiones[listaEjercicios.IndexOf(ejercicio)]);
                    MessageBox.Show(nuevo + listaDescripciones[1] + "\n LAS REPETICIONES SON: " + listaRepeticiones[listaEjercicios.IndexOf(ejercicio)].ToString());
                    ejer2.ShowDialog();
                    resumenResultados += "\n Ejercicio 2: \n" + ejer2.devolverResumen();

                }
                if (ejercicio == "Ejercicio3")
                {
                    Ejercicio3 ejer3 = new Ejercicio3(nombreUsuarioPaciente, listaRepeticiones[listaEjercicios.IndexOf(ejercicio)]);
                    MessageBox.Show(nuevo + listaDescripciones[2] + "\n LAS REPETICIONES SON: " + listaRepeticiones[listaEjercicios.IndexOf(ejercicio)].ToString());
                    ejer3.ShowDialog();
                    resumenResultados += "\n Ejercicio 3: \n" + ejer3.devolverResumen();
                }
                if (ejercicio == "Ejercicio4")
                {
                    Ejercicio4 ejer4 = new Ejercicio4(nombreUsuarioPaciente, listaRepeticiones[listaEjercicios.IndexOf(ejercicio)]);
                    MessageBox.Show(nuevo + listaDescripciones[3] + "\n LAS REPETICIONES SON: " + listaRepeticiones[listaEjercicios.IndexOf(ejercicio)].ToString());
                    ejer4.ShowDialog();
                    resumenResultados += "\n Ejercicio 4: \n" + ejer4.devolverResumen();
                }
                if (ejercicio == "Ejercicio5")
                {
                    Ejercicio5 ejer5 = new Ejercicio5(nombreUsuarioPaciente, listaRepeticiones[listaEjercicios.IndexOf(ejercicio)]);
                    MessageBox.Show(nuevo + listaDescripciones[4] + "\n LAS REPETICIONES SON: " + listaRepeticiones[listaEjercicios.IndexOf(ejercicio)].ToString());
                    ejer5.ShowDialog();
                    resumenResultados += "\n Ejercicio 5: \n" + ejer5.devolverResumen();
                }
                if (ejercicio == "Ejercicio6")
                {
                    Ejercicio6 ejer6 = new Ejercicio6(nombreUsuarioPaciente, listaRepeticiones[listaEjercicios.IndexOf(ejercicio)]);
                    MessageBox.Show(nuevo + listaDescripciones[5] + "\n LAS REPETICIONES SON: " + listaRepeticiones[listaEjercicios.IndexOf(ejercicio)].ToString());
                    ejer6.ShowDialog();
                    resumenResultados += "\n Ejercicio 6: \n" + ejer6.devolverResumen();
                }
            }

            DateTime hoy = DateTime.Today;
            string fecha = hoy.ToString("yyyy/MM/dd");

            if (MessageBox.Show("¿Quieres escribir feedback acerca del entrenamiento?", "Pregunta", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                EscribirFeedbackEntrenamiento feedback = new EscribirFeedbackEntrenamiento();
                feedback.ShowDialog();
                feedbackPaciente = feedback.devolverFeedback();
            }
            else
            {
                feedbackPaciente = null;
            }
            if (Entrenamiento.modificarEntrenamiento(Convert.ToInt32(comboBoxIDEntrenamiento.Text), fecha, resumenResultados, feedbackPaciente) > 0)
            {
                MessageBox.Show("Enhorabuena, has completado el entrenamiento y tu terapeuta podrá ver tus resultados");
                this.Close();
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error al registrar tu entrenamiento");
            }
        }
    }
}
