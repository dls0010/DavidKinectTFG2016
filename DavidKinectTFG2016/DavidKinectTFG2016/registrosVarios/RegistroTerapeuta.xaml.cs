using System;
using System.Collections.Generic;
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

namespace DavidKinectTFG2016.registrosVarios
{
    /// <summary>
    /// Lógica de interacción para RegistroTerapeuta.xaml
    /// </summary>
    public partial class RegistroTerapeuta : Window
    {
        string nombreUsuario;
        string path = null;
        public RegistroTerapeuta(string nombre)
        {
            nombreUsuario = nombre;
            InitializeComponent();
        }

        /// <summary>
        /// Comportamiento del boton cancelar. Cerrará la ventana.
        /// </summary>
        /// <param name="sender"></param> Boton cancelar.
        /// <param name="e"></param> Argumento del evento.
        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Inserccion de la fecha seleccionada por el usuario en un texbox.
        /// </summary>
        /// <param name="sender"></param> DateTimePicker
        /// <param name="e"></param> Argumento del evento.
        private void buttonNacimiento_Click(object sender, RoutedEventArgs e)
        {
            textBoxNacimiento.Text = dateCalendario.SelectedDate.Value.ToString("yyyy/MM/dd");
        }

        /// <summary>
        ///  Comportamiento de registro del terapeuta en la base de datos.
        /// </summary>
        /// <param name="sender"></param> Boton de registro.
        /// <param name="e"></param> Argumento del evento.
        private void buttonRegistrar_Click(object sender, RoutedEventArgs e)
        {
            //Recogemos los datos.
            string nombreTerapeuta = textBoxNombre.Text;
            string apellidosTerapeuta = textBoxApellidos.Text;
            string nifTerapeuta = textBoxNIF.Text;
            string nacimientoTerapeuta = textBoxNacimiento.Text;
            string telefonoTerapeuta = textBoxTelefono.Text;

            if (Terapeuta.registrarTerapeuta(nombreTerapeuta, apellidosTerapeuta, nombreUsuario, nifTerapeuta, nacimientoTerapeuta,telefonoTerapeuta,path) > 0)
            {
                MessageBox.Show("Terapeuta registrado con exito.");
                this.Close();
            }
            else
            {
                if (path == null)
                    MessageBox.Show("El terapeuta debe de insertar una foto.");
                else
                    MessageBox.Show("El terapeuta ha completado mal el formulario.");
            }
        }

        /// <summary>
        /// Metodo que permite la seleccion de una imagen en nuestro ordenador para colocarla de perfil.
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
                path = openFile.FileName.ToString();
                imagenFoto.Source = new BitmapImage(new Uri(path));
            }
        }
    }
}
