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

using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.Controls;

namespace DavidKinectTFG2016.recursosPaciente
{
    /// <summary>
    /// Lógica de interacción para Ejercicio1Paciente.xaml
    /// </summary>
    public partial class Ejercicio1Paciente : Window
    {
        //Sensores conectados al ordenador.
        KinectSensorChooser miKinect;
        KinectSensor kinect;
        string mensajeP1;
        string mensajeP2;
        string mensaje1;
        public Ejercicio1Paciente()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            miKinect = new KinectSensorChooser();
            //detecta si el kinect se ha desconectado o ha sido conectado y lanza evento:
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
                setKinect(e.NewSensor);
                e.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                e.NewSensor.SkeletonStream.Enable();
                //Manejador de eventos esqueleto:
                e.NewSensor.SkeletonFrameReady += miKinect_SkeletonFrameReady;

                try
                {
                    e.NewSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated; //cuando estamos sentados.
                    e.NewSensor.DepthStream.Range = DepthRange.Near;
                    e.NewSensor.SkeletonStream.EnableTrackingInNearRange = true;
                }
                catch(InvalidOperationException)
                {
                    e.NewSensor.DepthStream.Range = DepthRange.Default;
                    e.NewSensor.SkeletonStream.EnableTrackingInNearRange = false;
                }
            }
            catch (InvalidOperationException)
            {
                error = true;
            }
            
            zonaCursor.KinectSensor = e.NewSensor;
           
        }

        /// <summary>
        /// Metodo que asigna el kinect a la variable kinect.
        /// </summary>
        /// <param name="newSensor"></param> el sensor activo.
        private void setKinect(KinectSensor newSensor)
        {
            kinect = newSensor;
        }

        /// <summary>
        /// Manejador de eventos para el esqueleto.
        /// </summary>
        /// <param name="sender"></param>  Stream de esqueletos.
        /// <param name="e"></param> Eventos del stream esqueletos.
        private void miKinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
           Skeleton[] esqueletos = null;
            using (SkeletonFrame framesEsqueleto = e.OpenSkeletonFrame())
            {
                if(framesEsqueleto!= null)
                {
                    esqueletos = new Skeleton[framesEsqueleto.SkeletonArrayLength];
                    framesEsqueleto.CopySkeletonDataTo(esqueletos);
                }
            }
            if (esqueletos == null) return;
            
            
            foreach (Skeleton esqueleto in esqueletos)
            {
                if(esqueleto.TrackingState == SkeletonTrackingState.Tracked)
                {
                    empezar(esqueleto);
                }
            }
            textPosicion1.Text = mensajeP1;
            textPosicion2.Text = mensajeP2;
            textResultado.Text = mensaje1;
        }

        /// <summary>
        /// Metodo que obliga al paciente a comenzar el ejercicio.
        /// con los brazos totalmente abiertos y a la misma altura.
        /// </summary>
        /// <param name="esqueleto"></param> Esqueleto del paciente.
        private void empezar(Skeleton esqueleto)
        {
            Joint jointCabeza = esqueleto.Joints[JointType.Head];
            SkeletonPoint posicionCabeza = jointCabeza.Position;

            Joint jointManoDerecha = esqueleto.Joints[JointType.HandRight];
            SkeletonPoint posicionManoDerecha = jointManoDerecha.Position;

            Joint jointManoIzquierda = esqueleto.Joints[JointType.HandLeft];
            SkeletonPoint posicionManoIzquierda = jointManoIzquierda.Position;


            float numeroDerecha = (float)Math.Round(posicionManoDerecha.Y, 2);
            float numeroIzquierda = (float)Math.Round(posicionManoIzquierda.Y, 2);
            float numeroCabeza = (float)Math.Round(posicionCabeza.Y, 2);
            //Comprobamos diferencia de manos.
            float restaManos = numeroDerecha - numeroIzquierda;
            float restaCabezaD = numeroCabeza - numeroDerecha;
            float restaCabezaI = numeroCabeza - numeroIzquierda;


            if ((restaManos >= -0.07 && restaManos < 0) || (restaManos > 0 && restaManos <= 0.07))
            {
                mensaje1 = "Vale!";
                mensajeP1 = numeroDerecha.ToString();
                mensajeP2 = numeroIzquierda.ToString();

            }
            else
            {
                mensaje1 = "No!";
                mensajeP1 = numeroDerecha.ToString();
                mensajeP2 = numeroIzquierda.ToString();
            }
            //mensajeP1 = string.Format("X:{0:0.0#} Y:{1:0.0#} Z:{2:0:0#}", posicionManoIzquierda.X, posicionManoIzquierda.Y, posicionManoIzquierda.Z);
            //mensajeP2 = string.Format("X:{0:0.0#} Y:{1:0.0#} Z:{2:0:0#}", posicionManoDerecha.X, posicionManoDerecha.Y, posicionManoDerecha.Z);

        }

        /// <summary>
        /// Metodo que detiene la camara.
        /// </summary>
        /// <param name="sender"></param> Boton cerrar
        /// <param name="e"></param> Evento de cerrar.
        private void Window_Closed(object sender, EventArgs e)
        {
            miKinect.Stop();
        }
    }
}
