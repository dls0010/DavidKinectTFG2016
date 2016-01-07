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

namespace DavidKinectTFG2016.recursosPaciente
{
    /// <summary>
    /// Lógica de interacción para EscribirFeedbackEntrenamiento.xaml
    /// </summary>
    public partial class EscribirFeedbackEntrenamiento : Window
    {
        private string feedback;
        /// <summary>
        /// Clase que obtiene la valoracion del entrenamiento por parte del paciente.
        /// </summary>
        public EscribirFeedbackEntrenamiento()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Boton cuya accion es mandar el texto de la valoracion del entrenamiento.
        /// introducido por parte del paciente a la variable feedback.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonMandar_Click(object sender, RoutedEventArgs e)
        {
            feedback = textBoxFeedback.Text;
            this.Close();
        }

        /// <summary>
        /// Boton cancelar que se encarga de cerrar la ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Metodo que obtiene el feedback del paciente
        /// </summary>
        /// <returns>
        /// string de la valoracion del paciente acerca del entrenamiento.
        /// </returns>
        public string devolverFeedback()
        {
            return feedback;
        }
    }
}
