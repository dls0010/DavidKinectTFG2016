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
using DavidKinectTFG2016.recursosAdministrador;

namespace DavidKinectTFG2016.iniciosSesionVarios
{
    /// <summary>
    /// Lógica de interacción para InicioSesionAdministrador.xaml
    /// </summary>
    public partial class InicioSesionAdministrador : Window
    {
        string nombreUsuario;
        public InicioSesionAdministrador(string nombre)
        {
            nombreUsuario = nombre;
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que lanza la ventana para editar pacientes de la BD.
        /// </summary>
        /// <param name="sender"></param> Boton modificar pacientes.
        /// <param name="e"></param> Evento del boton.
        private void buttonModificarPacientes_Click(object sender, RoutedEventArgs e)
        {
            EditarPacientes editarPacientes = new EditarPacientes();
            editarPacientes.Show();
        }

        /// <summary>
        /// Metodo que lanza la ventana para editar terapeutas de la BD.
        /// </summary>
        /// <param name="sender"></param> Boton modificar terapeutas.
        /// <param name="e"></param> Evento del boton.
        private void buttonModificarTerapeutas_Click(object sender, RoutedEventArgs e)
        {
            EditarTerapeutas editarTerapeutas = new EditarTerapeutas();
            editarTerapeutas.Show();
        }
    }
}
