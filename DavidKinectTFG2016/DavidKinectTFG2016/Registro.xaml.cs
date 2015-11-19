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
using DavidKinectTFG2016.registrosVarios;

namespace DavidKinectTFG2016
{
    /// <summary>
    /// Lógica de interacción para Registro.xaml
    /// </summary>
    public partial class Registro : Window
    {
        /// <summary>
        /// Clase que permite el registro de un usuario en la base de datos.
        /// </summary>
        public Registro()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Metodo que cierra la ventana.
        /// </summary>
        /// <param name="sender"></param>Boton Cancelar.
        /// <param name="e"></param> Evento cancelar.
        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Metodo que guarda los datos en la base de datos del usuario.
        /// </summary>
        /// <param name="sender"></param> Boton Registrar.
        /// <param name="e"></param> Evento registrar.
        private void buttonRegistrar_Click(object sender, RoutedEventArgs e)
        {
            //Obtenemos los datos introducidos:
            string usuarioIntroducido = textBoxUsuario.Text;
            string contraseña1Introducida = passwordBoxContraseña1.Password;
            string contraseña2Introducida = passwordBoxContraseña2.Password;
            string tipoUsuario = comboBoxTipoUsuario.Text;

            if (contraseña1Introducida == contraseña2Introducida)
            {
                if (Usuario.CrearUsuarios(usuarioIntroducido, contraseña1Introducida, tipoUsuario) > 0)
                {
                    MessageBox.Show("Cuenta creada con exito.");
                    if (tipoUsuario == "Paciente")
                    {
                        RegistroPaciente regPaciente = new RegistroPaciente(usuarioIntroducido);
                        regPaciente.Show();
                        this.Close();
                    }
                    if (tipoUsuario == "Terapeuta")
                    {
                        RegistroTerapeuta regTerapeuta = new RegistroTerapeuta(usuarioIntroducido);
                        regTerapeuta.Show();
                        this.Close();
                    }
                    if (tipoUsuario == "Administrador")
                    {
                        RegistroAdministrador regAdministrador = new RegistroAdministrador(usuarioIntroducido);
                        regAdministrador.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Usuario ya existe. No puede registrarse con ese nombre.");
                }      
            }
            else
            {
                MessageBox.Show("Las contraseñas no coinciden");
                passwordBoxContraseña1.Clear();
                passwordBoxContraseña2.Clear();
            }

        }
    }
}
