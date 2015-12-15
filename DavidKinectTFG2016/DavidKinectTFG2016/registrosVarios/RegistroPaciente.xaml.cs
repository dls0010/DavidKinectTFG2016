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
using Microsoft.Kinect;

namespace DavidKinectTFG2016.registrosVarios
{
    /// <summary>
    /// Lógica de interacción para RegistroPaciente.xaml
    /// </summary>
    public partial class RegistroPaciente : Window
    {
        private KinectSensor kinect;
        private byte[] pixelData;
        string nombreUsuario;
        string path = null;
        public RegistroPaciente(string nombre)
        {
            nombreUsuario = nombre;
            InitializeComponent();
        }

        /// <summary>
        ///  Comportamiento de registro del paciente en la base de datos.
        /// </summary>
        /// <param name="sender"></param> Boton de registro.
        /// <param name="e"></param> Argumento del evento.
        private void buttonRegistrar_Click(object sender, RoutedEventArgs e)
        {
            //Recogemos los datos.
            string nombrePaciente = textBoxNombre.Text;
            string apellidosPaciente = textBoxApellidos.Text;
            string nifPaciente = textBoxNIF.Text;
            string telefonoPaciente = textBoxTelefono.Text;
            string nacimientoPaciente = textBoxNacimiento.Text;
            string estadoPaciente = textBoxEstado.Text;
            string descripcionPaciente = textBoxDescripcion.Text;

            if (path == "miFoto.jpg")
            {
                path = AppDomain.CurrentDomain.BaseDirectory.ToString() + "miFoto.jpg";
            }
            if (Paciente.RegistrarPaciente(nombrePaciente, apellidosPaciente, nombreUsuario, nifPaciente, telefonoPaciente, nacimientoPaciente, estadoPaciente, descripcionPaciente, path) > 0)
            {
                MessageBox.Show("Paciente registrado con exito.");
                this.Close();
            }
            else
            {
                if(path == null)
                    MessageBox.Show("El paciente debe de insertar una foto.");
                else
                    MessageBox.Show("El paciente ha completado mal el formulario.");
            }
        }

        /// <summary>
        /// Metodo que cierra la ventana.
        /// </summary>
        /// <param name="sender"></param> Boton Cancelar.
        /// <param name="e"></param> Evento del boton.
        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            if (Usuario.BorrarUsuario(nombreUsuario) > 0)
            {
                MessageBox.Show("Has cancelado el proceso de registro.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Error al cancelar el proceso de registro.");
            }
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

        /// <summary>
        /// Metodo que obtiene una foto del paciente a traves de la Kinect.
        /// </summary>
        /// <param name="sender"></param> Boton obtener fotografía.
        /// <param name="e"></param> Evento del botón.
        private void buttonHacerFoto_Click(object sender, RoutedEventArgs e)
        {
            buttonHacerFoto.IsEnabled = false;
            kinect = KinectSensor.KinectSensors.FirstOrDefault(sensorItem => sensorItem.Status == KinectStatus.Connected);
            kinect.Start();
            kinect.ColorStream.Enable();
            kinect.ColorFrameReady += kinect_ColorFrameReady;
        }

        /// <summary>
        /// Manejador de evento para detectar el stream de la Kinect y sacar la foto.
        /// </summary>
        /// <param name="sender"></param> steam de la kinect.
        /// <param name="e"></param> eventos de la kinect.
        private void kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame imageFrame = e.OpenColorImageFrame())
            {
                if(imageFrame == null)
                {
                    return;
                }
                else
                {
                    pixelData = new byte[imageFrame.PixelDataLength];
                    imageFrame.CopyPixelDataTo(this.pixelData);
                    int stride = imageFrame.Width * imageFrame.BytesPerPixel;
                    this.imagenFoto.Source = BitmapSource.Create(imageFrame.Width, imageFrame.Height,
                        96,
                        96,
                        PixelFormats.Bgr32,
                        null,
                        pixelData,
                        stride);
                }
            }
        }

        /// <summary>
        /// Metodo que guarda la imagen tomada por parte de la kinect en nuestro ordenador.
        /// dejando la ruta de la nueva foto en path.
        /// </summary>
        /// <param name="sender"></param> Boton Tomar Foto.
        /// <param name="e"></param> Eventos del boton.
        private void buttonTomarFoto_Click(object sender, RoutedEventArgs e)
        {
            path = "miFoto.jpg";
            if (File.Exists(path))
                File.Delete(path);

            using(FileStream fotoGuardada = new FileStream(path, FileMode.CreateNew))
            {
                BitmapSource imagen = (BitmapSource)imagenFoto.Source;
                JpegBitmapEncoder jpg = new JpegBitmapEncoder();
                jpg.QualityLevel = 70;
                jpg.Frames.Add(BitmapFrame.Create(imagen));
                jpg.Save(fotoGuardada);
                fotoGuardada.Close();
                buttonHacerFoto.IsEnabled = true;
            }
        }
    }
}
