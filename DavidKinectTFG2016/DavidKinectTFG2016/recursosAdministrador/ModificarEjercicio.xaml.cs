using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.IO;

namespace DavidKinectTFG2016.recursosAdministrador
{
    /// <summary>
    /// Lógica de interacción para ModificarEjercicio.xaml
    /// </summary>
    public partial class ModificarEjercicio : Window
    {
        SqlConnection conexion;
        string pathImagen = null;
        public ModificarEjercicio()
        {
            InitializeComponent();
           
        }

        /// <summary>
        /// Metodo que carga la BD y rellena el combobox con todos los ejercicios.
        /// </summary>
        /// <param name="sender"></param> Cargar ventana.
        /// <param name="e"></param> Evento de carga.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion = BDComun.ObtnerConexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar con la Base de datos: " + ex.ToString());
            }
            llenarComboBox();
        }

        /// <summary>
        /// Metodo adicional para llenar el combobox con los ejercicios.
        /// </summary>
        private void llenarComboBox()
        {
            try
            {
                string query = "Select * from ejercicios";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    string nombreEjercicio = dr.GetString(1);
                    comboBoxEjercicios.Items.Add(nombreEjercicio);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos de los ejercicios: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que rellena el textbox descripcion de la pagina al seleccionar un ejercicio en el combobox.
        /// </summary>
        /// <param name="sender"></param> ComboboxEjercicios.
        /// <param name="e"></param> Evento del comboBoxEjercicios
        private void comboBoxEjercicios_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                string query = "Select descripcion,imagenEjercicio from ejercicios where ejercicio = '" + comboBoxEjercicios.Text + "'";
                SqlCommand comando = new SqlCommand(query, conexion);
                SqlDataReader dr = comando.ExecuteReader();
                while (dr.Read())
                {
                    string descripcion = dr.GetString(0);
                    textBoxDescripcion.Text = descripcion;

                    if (dr["imagenEjercicio"] != DBNull.Value)
                    {
                        byte[] imagen = (byte[])(dr["imagenEjercicio"]);

                        MemoryStream mstream = new MemoryStream(imagen);
                        BitmapImage image = new BitmapImage();
                        image.BeginInit();
                        image.StreamSource = mstream;
                        image.EndInit();
                        imagenFoto.Source = image;
                    }
                    else
                    {
                        imagenFoto.Source = null;
                    }
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener la descripcion de los ejercicios: " + ex.ToString());
            }

        }

        /// <summary>
        /// Metodo que cierra la ventana, al pulsar el boton de cancelar, cierra la conexion con la BD.
        /// </summary>
        /// <param name="sender"></param> Boton Cancelar.
        /// <param name="e"></param> Evento del boton.
        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                conexion.Close();
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error al cerrar la conexion con la BD: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que cierra la conexion con la base de datos al cerrar la ventana.
        /// </summary>
        /// <param name="sender"></param> Boton cerrar.
        /// <param name="e"></param>Evento de cerrar.
        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                conexion.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar la conexion con la BD: " + ex.ToString());
            }
        }

        /// <summary>
        /// Metodo que va a modificar tanto la descripcion como la foto descriptiva del ejercicio si es preciso.
        /// </summary>
        /// <param name="sender"></param> Boton Modificar.
        /// <param name="e"></param> Eventos del boton.
        private void buttonModificar_Click(object sender, RoutedEventArgs e)
        {
            //Por hacer.
            if (Ejercicio.modificarEjercicio(comboBoxEjercicios.Text, textBoxDescripcion.Text,pathImagen) > 0)
            {
                MessageBox.Show("Ejercicio actualizado");
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al actualizar el ejercicio");
            }
        }

        /// <summary>
        /// Metodo que permite la seleccion de una imagen en nuestro ordenador para el ejercicio.
        /// </summary>
        /// <param name="sender"></param> Boton Examinar.
        /// <param name="e"></param>Evento del botón.
        private void buttonExaminar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "Seleccione la Imagen a Mostrar";
            openFile.Filter = "Todos(*.*) | *.*| Imagenes | *.jpg; *.gif; *.png; *.bmp";
            if (openFile.ShowDialog() == true)
            {
                pathImagen = openFile.FileName.ToString();
                imagenFoto.Source = new BitmapImage(new Uri(pathImagen));
            }
        }
    }
}
