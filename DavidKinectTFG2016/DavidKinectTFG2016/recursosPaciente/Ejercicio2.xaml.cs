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

using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;
using System.Media;

namespace DavidKinectTFG2016.recursosPaciente
{
    /// <summary>
    /// Lógica de interacción para Ejercicio2.xaml
    /// </summary>
    public partial class Ejercicio2 : Window
    {
        KinectSensorChooser miKinect;
        KinectSensor kinect;
        string nombreUsuarioPaciente;
        byte[] bytesColor;
        Skeleton[] esqueletos = null;
        int primeraVez = 1;
        int maximoRepeticiones;
        string mensaje;
        int repeticionesD = 0;
        int repeticionesI = 0;
        //Deteccion de posturas.
        const int PostureDetectionNumber = 5;
        int accumulator = 0;
        Posture postureInDetection = Posture.None;
        Posture previousPosture = Posture.None;
        Boolean finalEjercicio = false;

        //Tiempo que comienza
        DateTime comienzo;
        DateTime final;
        TimeSpan duracion;

        public Ejercicio2(string nombreUsuario,int repeticiones)
        {
            maximoRepeticiones = repeticiones;
            nombreUsuarioPaciente = nombreUsuario;
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que arranca la kinect al abrirse la ventana.
        /// </summary>
        /// <param name="sender"></param> Abrir la ventana.
        /// <param name="e"></param> Evento de abrir ventana.
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            miKinect = new KinectSensorChooser();
            miKinect.KinectChanged += miKinect_KinectChanged;
            sensorChooserUI.KinectSensorChooser = miKinect;
            miKinect.Start();
        }

        /// <summary>
        /// Metodo que se va a ejecutar cuando el estado del kinect cambie:
        /// Conectarse, desconectarse...
        /// </summary>
        /// <param name="sender"></param> kinect.
        /// <param name="e"></param> evento del kinect. sus propiedades:
        /// - OldSensor: cambia a nulo para verificar que esta desconectado.
        /// - NewSensor: cambia a nulo para verificar que esta conectado.
        private void miKinect_KinectChanged(object sender, KinectChangedEventArgs e)
        {
            //verifica si hay error en el codigo:
            bool error = true;

            if (e.OldSensor == null)//desconectamos el Kinect de la computadora.
            {
                try
                {
                    e.OldSensor.DepthStream.Disable();
                    e.OldSensor.SkeletonStream.Disable();
                }
                catch (Exception)
                {
                    error = true;
                }
            }

            if (e.NewSensor == null) //conectamos un Kinect a la computadora.
                return;
            try
            {
                kinect = e.NewSensor;
                kinect.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                kinect.SkeletonStream.Enable();
                kinect.ColorStream.Enable();

                //Manejador de eventos:
                kinect.SkeletonFrameReady += kinect_SkeletonFrameReady;
                kinect.ColorFrameReady += kinect_ColorFrameReady;

                try
                {
                    e.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated; //cuando estamos sentados.
                    e.NewSensor.DepthStream.Range = DepthRange.Near;
                    e.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                }
                catch (InvalidOperationException)
                {
                    e.NewSensor.DepthStream.Range = DepthRange.Default;
                    e.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                }
            }
            catch (InvalidOperationException)
            {
                error = true;
            }
        }

        /// <summary>
        /// Manejador de eventos para el esqueleto.
        /// </summary>
        /// <param name="sender"></param>  Stream de esqueletos.
        /// <param name="e"></param> Eventos del stream esqueletos.
        private void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            string corregirPosicion = "";
            esqueletos = null;
            using (SkeletonFrame framesEsqueleto = e.OpenSkeletonFrame())
            {
                if (framesEsqueleto != null)
                {
                    esqueletos = new Skeleton[framesEsqueleto.SkeletonArrayLength];
                    framesEsqueleto.CopySkeletonDataTo(esqueletos);
                }
            }
            if (esqueletos == null) return;


            foreach (Skeleton esqueleto in esqueletos)
            {
                if (esqueleto.TrackingState == SkeletonTrackingState.Tracked)
                {
                    Joint cabeza = esqueleto.Joints[JointType.Head];
                    SkeletonPoint posicionCabeza = cabeza.Position;
                    Joint manoDerecha = esqueleto.Joints[JointType.HandRight];
                    SkeletonPoint posicionManoDerecha = manoDerecha.Position;
                    Joint manoIzquierda = esqueleto.Joints[JointType.HandLeft];
                    SkeletonPoint posicionManoIzquierda = manoIzquierda.Position;
                    Joint hombroDerecho = esqueleto.Joints[JointType.ShoulderRight];
                    SkeletonPoint posicionHombroDerecho = hombroDerecho.Position;
                    Joint hombroIzquierdo = esqueleto.Joints[JointType.ShoulderLeft];
                    SkeletonPoint posicionHombroIzquierdo = hombroIzquierdo.Position;

                    rectanguloCorregir.Visibility = Visibility.Hidden;
                    if (esqueleto.ClippedEdges == 0)
                    {
                        corregirPosicion = "";
                    }
                    else
                    {
                        if ((esqueleto.ClippedEdges & FrameEdges.Right) != 0)
                        {
                            rectanguloCorregir.Visibility = Visibility.Visible;
                            corregirPosicion += "Moverse mas a la izquierda";
                        }
                        if ((esqueleto.ClippedEdges & FrameEdges.Left) != 0)
                        {
                            rectanguloCorregir.Visibility = Visibility.Visible;
                            corregirPosicion += "Moverse mas a la derecha";
                        }
                    }

                    if (primeraVez == 1) //empezar el ejercicio teniendo manos alineadas.
                    {
                        mensaje = "Estira los brazos horizontalmente para comenzar el ejercicio";
                        if (empezar(manoDerecha, manoIzquierda, hombroDerecho, hombroIzquierdo))
                        {
                            if (postureDetector(Posture.InicioBrazosExtendidos))
                            {
                                primeraVez = 2;
                                mensaje = "Vale, comienza el ejercicio.";
                                comienzo = DateTime.Now;
                            }
                        }
                    }
                    else
                    {
                        if (ejercicioManoArriba(cabeza, manoDerecha,hombroDerecho))
                        {
                            if (postureDetector(Posture.RHandUp) && corregirPosicion == "")
                            {
                                repeticionesD++;
                                textRepeticionD.Text = repeticionesD.ToString();
                                textRepeticionD.Foreground = Brushes.Green;
                                textRepeticionI.Foreground = Brushes.Red;
                                SystemSounds.Beep.Play();
                                mensaje = "Ahora baja el brazo derecho.";
                            }
                        }
                        else
                        {
                            if (ejercicioManoArriba(cabeza, manoIzquierda,hombroIzquierdo))
                            {
                                if (postureDetector(Posture.LHandUp) && corregirPosicion == "")
                                {
                                    repeticionesI++;
                                    textRepeticionI.Text = repeticionesI.ToString();
                                    textRepeticionI.Foreground = Brushes.Green;
                                    textRepeticionD.Foreground = Brushes.Red;
                                    SystemSounds.Beep.Play();
                                    mensaje = "Ahora baja el brazo izquierdo.";
                                }
                            }
                            else
                            {
                                if (postureDetector(Posture.None))
                                {
                                    mensaje = "Estira el brazo arriba.";
                                }
                            }
                        }

                    }
                    if ((repeticionesD >= maximoRepeticiones && repeticionesI >= maximoRepeticiones) && finalEjercicio == false)
                    {
                        finalEjercicio = true;
                        finalizarEjercicio();
                    }
                    textResultado.Text = mensaje;
                    textCorregir.Text = corregirPosicion;
                }
            }
        }

        /// <summary>
        /// Metodo que comprueba la realizacion correcta del movimiento pedido.
        /// </summary>
        /// <param name="cabeza"></param> Joint de la cabeza.
        /// <param name="mano"></param> Joint de la mano.
        /// <param name="hombro"></param> Joint del hombro.
        /// <returns></returns>
        private Boolean ejercicioManoArriba(Joint cabeza, Joint mano,Joint hombro)
        {
            float distancia;
            distancia = (mano.Position.X - hombro.Position.X);
            if (Math.Abs(distancia) > 0.10f)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Metodo que hace la transaccion a la BD de los resultados del paciente.
        /// </summary>
        private void finalizarEjercicio()
        {
            final = DateTime.Now;
            duracion = new TimeSpan(final.Ticks - comienzo.Ticks);
            if(MessageBox.Show("¿Quieres escribir feedback acerca del ejercicio?", "Pregunta", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning)== MessageBoxResult.Yes)
            {
                EscribirFeedbackEjercicio feedback = new EscribirFeedbackEjercicio(nombreUsuarioPaciente, "Ejercicio 2", repeticionesD + repeticionesI, duracion.ToString());
                feedback.Show();
                this.Close();
            }
            else
            {
                if (Ejercicio.registrarEjercicio(nombreUsuarioPaciente, "Ejercicio 2", repeticionesD + repeticionesI, duracion.ToString(), "") > 0)
                {
                    textTitulo.Text = "ENHORABUENA";
                    textResultado.Text = "EJERCICIO COMPLETADO";
                    this.Close();
                }
                else
                {
                    textTitulo.Text = "ERROR";
                    textResultado.Text = "AL GUARDAR RESULTADOS";
                }
            }       
        }

        /// <summary>
        /// Metodo que detecta si se esta realizando una determinada postura.
        /// </summary>
        /// <param name="posture"></param> Postura del esqueleto.
        /// <returns>
        /// true: postura esperada.
        /// false: postura no esperada.
        /// </returns>
        private Boolean postureDetector(Posture posture)
        {
            if (postureInDetection != posture)
            {
                accumulator = 0;
                postureInDetection = posture;
                return false;
            }

            if (accumulator < PostureDetectionNumber)
            {
                accumulator++;
                return false;
            }
            if (posture != previousPosture)
            {
                previousPosture = posture;
                accumulator = 0;
                return true;
            }
            else
                accumulator = 0;
            return false;
        }

        /// <summary>
        /// Metodo que obliga al usuario a fijar las manos a la misma altura para comenzar el ejercicio.
        /// </summary>
        /// <param name="manoDerecha"></param> Joint que contiene la mano derecha.
        /// <param name="manoIzquierda"></param> Joint que contiene la mano izquierda.
        /// <returns>
        /// true: comenzar ejercicio.
        /// false: aun no comenzar ejercicio.
        /// </returns>
        private Boolean empezar(Joint manoDerecha, Joint manoIzquierda, Joint hombroDerecho, Joint hombroIzquierdo)
        {
            float numeroManoDerecha = (float)Math.Round(manoDerecha.Position.Y, 2);
            float numeroManoIzquierda = (float)Math.Round(manoIzquierda.Position.Y, 2);
            float numeroHombroDerecho = (float)Math.Round(hombroDerecho.Position.Y, 2);
            float numeroHombroIzquierdo = (float)Math.Round(hombroIzquierdo.Position.Y, 2);
            //Comprobamos diferencia de manos y hombros.
            float restaDerecha = numeroManoDerecha - numeroHombroDerecho;
            float restaIzquierda = numeroManoIzquierda - numeroHombroIzquierdo;
            float restaManos = numeroManoDerecha - numeroManoIzquierda;

            restaManos = Math.Abs(restaManos);
            restaDerecha = Math.Abs(restaManos);
            restaIzquierda = Math.Abs(restaIzquierda);
            if ((restaManos > 0 && restaManos <= 0.07) && (restaDerecha > 0 && restaDerecha <= 0.05) && (restaIzquierda > 0 && restaIzquierda <= 0.05))
            {
                textTitulo.Text = "Ejercicio EMPEZADO";
                textTitulo.Foreground = Brushes.Green;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Manejador de evento que muestra video por pantalla.
        /// </summary>
        /// <param name="sender"></param> Video del kinect.
        /// <param name="e"></param> Evento del video.
        private void kinect_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame frameImagen = e.OpenColorImageFrame())
            {
                if (frameImagen == null) return;

                bytesColor = new byte[frameImagen.PixelDataLength];
                frameImagen.CopyPixelDataTo(bytesColor);

                videoKinect.Source = BitmapSource.Create(
                    frameImagen.Width, frameImagen.Height,
                    96,
                    96,
                    PixelFormats.Bgr32,
                    null,
                    bytesColor,
                    frameImagen.Width * frameImagen.BytesPerPixel
                    );
            }
        }

        /// <summary>
        /// Metodo que se ejecuta al cerrar la ventana.
        /// </summary>
        /// <param name="sender"></param> Ventana.
        /// <param name="e"></param> Evento de cerrar.
        private void Window_Closed(object sender, EventArgs e)
        {
            if (kinect != null)
            {
                kinect.Stop();
                kinect = null;
            }
            if (miKinect != null)
            {
                miKinect.Stop();
                miKinect = null;
            }
        }

        /// <summary>
        /// Metodo que para el ejercicio y guarda los resultados tal y como estaban.
        /// </summary>
        /// <param name="sender"></param> Boton parar ejercicio.
        /// <param name="e"></param> Evento del boton.
        private void buttonParar_Click(object sender, RoutedEventArgs e)
        {
            finalizarEjercicio();
        }
    }
}
