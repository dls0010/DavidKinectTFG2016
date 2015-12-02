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
        public EscribirFeedbackEntrenamiento()
        {
            InitializeComponent();
        }

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
        /// <returns></returns>
        public string devolverFeedback()
        {
            return feedback;
        }
    }
}
