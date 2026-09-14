using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Monopoly.App
{
    public partial class FormHost : Form
    {
        private Host host;
        public FormHost(Host host, string nombreJugador)
        {
            InitializeComponent();
            this.host = host;

            lbl_nombre1H.Text = nombreJugador;

            host.Cliente.ActualizacionJuego += texto =>
            {
                this.Invoke(new Action(() => MessageBox.Show(texto)));
            };
        }

        private void FormEspera_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lbl_jugador1_Click(object sender, EventArgs e)
        {

        }
    }
}
