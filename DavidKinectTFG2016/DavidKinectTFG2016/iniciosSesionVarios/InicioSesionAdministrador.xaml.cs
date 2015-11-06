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
    }
}
