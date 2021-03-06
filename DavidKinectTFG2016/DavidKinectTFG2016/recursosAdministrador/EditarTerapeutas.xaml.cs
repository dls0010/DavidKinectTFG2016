﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace DavidKinectTFG2016.recursosAdministrador
{
    /// <summary>
    /// Lógica de interacción para EditarTerapeutas.xaml
    /// </summary>
    public partial class EditarTerapeutas : Window
    {
        MySqlConnection conexion;
        MySqlDataAdapter adaptador;
        DataTable dt;
        public EditarTerapeutas()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que actualiza un cambio del datagrid en la BD.
        /// </summary>
        /// <param name="sender"></param> Boton Actualizar.
        /// <param name="e"></param> Evento del boton.
        private void buttonModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MySqlCommandBuilder builder = new MySqlCommandBuilder(adaptador);
                adaptador.UpdateCommand = builder.GetUpdateCommand();
                int numeroCambios = adaptador.Update(dt);
                if (numeroCambios > 0)
                    MessageBox.Show("Actualizado");
                else
                    MessageBox.Show("No se ha actualizado ningun registro");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar la tabla:" + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que carga la tabla al abrirse la ventana.
        /// </summary>
        /// <param name="sender"></param> Ventana.
        /// <param name="e"></param>Evento de ventana.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la base de datos: " + ex.ToString());
            }

            try
            {
                adaptador = Terapeuta.getAdaptadorTerapeutas();
                dt = new DataTable("terapeutas");
                adaptador.Fill(dt);
                dataGrid.ItemsSource = dt.DefaultView;
                adaptador.Update(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar la tabla " + ex.ToString());
            }
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
                MessageBox.Show("Error al cerrar la conexion con la base de datos: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que cierra la conexion con la BD al cerrar la ventana.
        /// </summary>
        /// <param name="sender"></param> Cerrar ventana.
        /// <param name="e"></param> Evento de cerrar.
        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion.Close();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar la conexion con la base de datos: " + ex.ToString());
            }
        }
    }
}
